using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace MiroConnect
{
    // Reads a CSV or Excel (.xlsx) file and outputs headers and cell data.
    // Rows output is column-oriented: branch {i} holds all values for column i.
    public class TableReaderComponent : GH_Component
    {
        public TableReaderComponent()
            : base("Table Reader", "TblRd",
                "Reads a CSV or .xlsx file and outputs headers and cell data as a column tree.",
                "MiroConnect", "Data")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "P", "Full path to a .csv or .xlsx file.", GH_ParamAccess.item, "");
            pManager.AddIntegerParameter("Sheet", "Sh", "Sheet index (0-based) for Excel files. Ignored for CSV.", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("Has Headers", "H", "When true, the first row is treated as column headers.", GH_ParamAccess.item, true);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Headers", "Hd", "Column header strings in order.", GH_ParamAccess.list);
            pManager.AddTextParameter("Data", "D", "Cell values as a tree. Branch {i} contains all values for column i.", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Row Count", "RC", "Number of data rows (excluding the header row if present).", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Col Count", "CC", "Number of columns.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            int sheetIndex = 0;
            bool hasHeaders = true;

            DA.GetData(0, ref path);
            DA.GetData(1, ref sheetIndex);
            DA.GetData(2, ref hasHeaders);

            if (string.IsNullOrWhiteSpace(path))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No file path provided.");
                return;
            }

            if (!File.Exists(path))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "File not found: " + path);
                return;
            }

            List<List<string>> allRows;
            string ext = Path.GetExtension(path).ToLowerInvariant();

            try
            {
                if (ext == ".csv")
                    allRows = ReadCsv(path);
                else if (ext == ".xlsx" || ext == ".xls")
                    allRows = ReadExcel(path, sheetIndex);
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unsupported file type. Use .csv or .xlsx.");
                    return;
                }
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error reading file: " + ex.Message);
                return;
            }

            if (allRows.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "File is empty.");
                return;
            }

            int colCount = 0;
            foreach (var row in allRows)
                if (row.Count > colCount) colCount = row.Count;

            foreach (var row in allRows)
                while (row.Count < colCount) row.Add("");

            List<string> headers;
            List<List<string>> dataRows;

            if (hasHeaders)
            {
                headers = allRows[0];
                dataRows = allRows.GetRange(1, allRows.Count - 1);
            }
            else
            {
                headers = new List<string>(colCount);
                for (int c = 0; c < colCount; c++) headers.Add("Column " + (c + 1));
                dataRows = allRows;
            }

            var tree = new GH_Structure<GH_String>();
            for (int c = 0; c < colCount; c++)
            {
                var branchPath = new GH_Path(c);
                foreach (var row in dataRows)
                    tree.Append(new GH_String(row[c]), branchPath);
            }

            DA.SetDataList(0, headers);
            DA.SetDataTree(1, tree);
            DA.SetData(2, dataRows.Count);
            DA.SetData(3, colCount);
        }

        private static List<List<string>> ReadCsv(string path)
        {
            var rows = new List<List<string>>();
            using (var reader = new StreamReader(path, Encoding.UTF8, true))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line != null) rows.Add(ParseCsvLine(line));
                }
            }
            return rows;
        }

        // Parses a single CSV line per RFC 4180 (quoted fields, escaped double-quotes).
        // Does not support multi-line quoted fields.
        private static List<string> ParseCsvLine(string line)
        {
            var cells = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                    { sb.Append('"'); i++; }
                    else if (c == '"')
                    { inQuotes = false; }
                    else
                    { sb.Append(c); }
                }
                else
                {
                    if (c == '"')
                    { inQuotes = true; }
                    else if (c == ',')
                    { cells.Add(sb.ToString()); sb.Clear(); }
                    else
                    { sb.Append(c); }
                }
            }
            cells.Add(sb.ToString());
            return cells;
        }

        private static List<List<string>> ReadExcel(string path, int sheetIndex)
        {
            var rows = new List<List<string>>();

            using (var package = new OfficeOpenXml.ExcelPackage(new FileInfo(path)))
            {
                // EPPlus worksheets are 1-based; user passes 0-based
                int wsIndex = sheetIndex + 1;
                int sheetCount = package.Workbook.Worksheets.Count;

                if (sheetCount == 0)
                    throw new Exception("Workbook contains no worksheets.");
                if (wsIndex > sheetCount)
                    throw new Exception($"Sheet index {sheetIndex} is out of range. Workbook has {sheetCount} sheet(s).");

                var sheet = package.Workbook.Worksheets[wsIndex];
                if (sheet.Dimension == null) return rows;

                int rowCount = sheet.Dimension.Rows;
                int colCount = sheet.Dimension.Columns;

                for (int r = 1; r <= rowCount; r++)
                {
                    var row = new List<string>(colCount);
                    for (int c = 1; c <= colCount; c++)
                        row.Add(sheet.Cells[r, c].Text ?? "");
                    rows.Add(row);
                }
            }
            return rows;
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_TableReader;
        public override Guid ComponentGuid => new Guid("b7c4d2e1-f3a5-4b6c-9d8e-0f1a2b3c4d5e");
    }
}
