using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Items
{
    public class ImageJsonComponent : GH_Component
    {
        public ImageJsonComponent()
            : base("Image JSON", "ImageJSON",
                "Builds Miro image request bodies from lists of URLs, file paths, or Bitmaps.",
                "MiroConnect", "Items JSON")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Image URL", "URL", "List or tree of public image URLs (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("File Path", "F", "List or tree of local file paths (optional). Takes priority over URL.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Bitmap", "B", "List or tree of Bitmap objects (optional). Takes priority over File Path and URL.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Position JSON", "G", "List or tree of Position JSONs from the Position component (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Parent JSON", "P", "List or tree of Parent JSONs from the Parent component (optional).", GH_ParamAccess.tree);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Item JSON", "J", "List of image request body JSONs for use with Create Item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var urlTree      = new GH_Structure<GH_String>();
            var pathTree     = new GH_Structure<GH_String>();
            var bitmapTree   = new GH_Structure<IGH_Goo>();
            var positionTree = new GH_Structure<GH_String>();
            var parentTree   = new GH_Structure<GH_String>();

            DA.GetDataTree(0, out urlTree);
            DA.GetDataTree(1, out pathTree);
            DA.GetDataTree(2, out bitmapTree);
            DA.GetDataTree(3, out positionTree);
            DA.GetDataTree(4, out parentTree);

            var urls      = Flatten(urlTree);
            var paths     = Flatten(pathTree);
            var bitmaps   = FlattenGoo(bitmapTree);
            var positions = Flatten(positionTree);
            var parents   = Flatten(parentTree);

            int count = Math.Max(Math.Max(urls.Count, paths.Count),
                        Math.Max(bitmaps.Count, positions.Count));
            if (count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Provide an Image URL, File Path, or Bitmap.");
                return;
            }

            var results = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                string posJson    = Pick(positions, i);
                string parentJson = Pick(parents, i);

                // Priority: Bitmap > File Path > URL
                Bitmap bmp = Pick(bitmaps, i)?.ScriptVariable() as Bitmap;
                if (bmp != null)
                {
                    results.Add(ImageBuilders.BuildImageUploadSentinel(
                        BitmapToBytes(bmp), "image/png", posJson, parentJson));
                    continue;
                }

                string filePath = Pick(paths, i);
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    if (!File.Exists(filePath))
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"[{i}] File not found: {filePath}");
                        results.Add(null);
                        continue;
                    }
                    results.Add(ImageBuilders.BuildImageUploadSentinel(
                        File.ReadAllBytes(filePath), MimeTypeFromPath(filePath), posJson, parentJson));
                    continue;
                }

                string url = Pick(urls, i);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    results.Add(ImageBuilders.BuildImageBody(url, posJson, parentJson));
                    continue;
                }

                results.Add(null);
            }

            DA.SetDataList(0, results);
        }

        private static List<string> Flatten(GH_Structure<GH_String> tree)
        {
            var list = new List<string>();
            foreach (var branch in tree.Branches)
                foreach (var g in branch)
                    if (g != null) list.Add(g.Value);
            return list;
        }

        private static List<IGH_Goo> FlattenGoo(GH_Structure<IGH_Goo> tree)
        {
            var list = new List<IGH_Goo>();
            foreach (var branch in tree.Branches)
                foreach (var g in branch)
                    list.Add(g);
            return list;
        }

        private static string Pick(IList<string> list, int i) =>
            list.Count == 0 ? null : list[Math.Min(i, list.Count - 1)];

        private static T Pick<T>(IList<T> list, int i) where T : class =>
            list.Count == 0 ? null : list[Math.Min(i, list.Count - 1)];

        private static byte[] BitmapToBytes(Bitmap bmp)
        {
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static string MimeTypeFromPath(string path)
        {
            switch (Path.GetExtension(path).ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".gif":  return "image/gif";
                case ".webp": return "image/webp";
                case ".bmp":  return "image/bmp";
                default:      return "image/png";
            }
        }

        protected override Bitmap Icon => Properties.Resources.MC_ImageJson;
        public override Guid ComponentGuid => new Guid("b4c5d6e7-f8a9-4b0c-1d2e-3f4a5b6c7d8e");
    }
}
