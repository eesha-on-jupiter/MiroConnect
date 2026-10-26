using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Items
{
    public class TagJsonComponent : GH_Component
    {
        public TagJsonComponent()
            : base("Tag JSON", "TagJSON",
                "Builds Miro tag request body JSONs from lists of inputs.",
                "MiroConnect", "Items JSON")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Title", "T", "List or tree of tag label texts.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Fill Color", "Fill", "List or tree of named tag colors: red, light_green, cyan, yellow, magenta, green, blue, gray, violet, dark_green, dark_blue or black (optional, Miro defaults to red).", GH_ParamAccess.tree);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Tag JSON", "J", "List of tag request body JSONs for use with Create Tag.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var titleTree = new GH_Structure<GH_String>();
            var colorTree = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out titleTree)) return;
            DA.GetDataTree(1, out colorTree);

            var titles = Flatten(titleTree);
            var colors = Flatten(colorTree);

            if (titles.Count == 0) return;

            var results = new List<string>(titles.Count);
            for (int i = 0; i < titles.Count; i++)
                results.Add(TagBuilders.BuildTagBody(
                    Pick(titles, i), Pick(colors, i)));

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

        private static string Pick(IList<string> list, int i) =>
            list.Count == 0 ? null : list[Math.Min(i, list.Count - 1)];

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_TagJson;
        public override Guid ComponentGuid => new Guid("d6e7f8a9-b0c1-4d2e-3f4a-5b6c7d8e9f0a");
    }
}
