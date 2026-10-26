using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Boards
{
    public class FindBoardComponent : GH_Component
    {
        public FindBoardComponent()
            : base("Find Board", "FindBoard",
                "Lists all accessible Miro boards. Optionally filter by name to get a matched board ID.",
                "MiroConnect", "Boards")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board Name", "Name", "Name of the board to find (optional). Returns matched Board ID when provided.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Board IDs", "IDs", "IDs of all accessible boards.", GH_ParamAccess.list);
            pManager.AddTextParameter("Board Names", "Names", "Names of all accessible boards.", GH_ParamAccess.list);
            pManager.AddTextParameter("Board ID", "ID", "ID of the first board matching the given name (only set when Name input is provided).", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null, name = null;
            if (!DA.GetData(0, ref token)) return;
            DA.GetData(1, ref name);

            try
            {
                var allIds   = new List<string>();
                var allNames = new List<string>();

                // Paginate through all boards
                int offset = 0;
                const int limit = 50;
                while (true)
                {
                    string json = MiroClient.Get(token, $"/boards?limit={limit}&offset={offset}");
                    var root = JObject.Parse(json);
                    var data = root["data"] as JArray;

                    if (data == null || data.Count == 0) break;

                    foreach (var board in data)
                    {
                        allIds.Add(board["id"]?.ToString());
                        allNames.Add(board["name"]?.ToString());
                    }

                    int total = root["total"]?.Value<int>() ?? 0;
                    offset += data.Count;
                    if (offset >= total) break;
                }

                DA.SetDataList(0, allIds);
                DA.SetDataList(1, allNames);

                // Matched ID — only resolve when name input was supplied
                if (!string.IsNullOrWhiteSpace(name))
                {
                    string matchedId = null;
                    for (int i = 0; i < allNames.Count; i++)
                    {
                        if (string.Equals(allNames[i], name, StringComparison.OrdinalIgnoreCase))
                        {
                            matchedId = allIds[i];
                            break;
                        }
                    }

                    if (matchedId == null)
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"No board found with name: {name}");
                    else
                        DA.SetData(2, matchedId);
                }
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_FindBoard;
        public override Guid ComponentGuid => new Guid("2f8b3c7d-1e4a-4b9f-8c2e-a5d6e7f8b9c0");
    }
}
