using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Items
{
    public class FrameJsonComponent : GH_Component
    {
        public FrameJsonComponent()
            : base("Frame JSON", "FrameJSON",
                "Builds Miro frame request body JSONs from lists of inputs.",
                "MiroConnect", "Items JSON")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Title", "T", "List or tree of frame titles.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Position JSON", "G", "List or tree of Position JSONs from the Position component.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Style JSON", "S", "Style JSON from the Frame Style component (optional).", GH_ParamAccess.tree);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Item JSON", "J", "List of frame request body JSONs for use with Create Item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var titleTree    = new GH_Structure<GH_String>();
            var positionTree = new GH_Structure<GH_String>();
            var styleTree    = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out titleTree) || !DA.GetDataTree(1, out positionTree)) return;
            DA.GetDataTree(2, out styleTree);

            var titles    = Flatten(titleTree);
            var positions = Flatten(positionTree);
            var styles    = Flatten(styleTree);

            int count = Math.Max(titles.Count, positions.Count);
            if (count == 0) return;

            var results = new List<string>(count);
            for (int i = 0; i < count; i++)
                results.Add(FrameBuilders.BuildFrameBody(
                    Pick(titles, i), Pick(positions, i), Pick(styles, i)));

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_FrameJson;
        public override Guid ComponentGuid => new Guid("a3b4c5d6-e7f8-4a9b-0c1d-2e3f4a5b6c7d");
    }
}
