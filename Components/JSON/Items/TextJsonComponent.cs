using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Items
{
    public class TextJsonComponent : GH_Component
    {
        public TextJsonComponent()
            : base("Text JSON", "TextJSON",
                "Builds Miro text item request body JSONs from lists of inputs.",
                "MiroConnect", "Items JSON")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Content", "C", "List or tree of text contents.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Position JSON", "G", "List or tree of Position JSONs from the Position component.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Style JSON", "S", "Style JSON from the Text Style component (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Parent JSON", "P", "List or tree of Parent JSONs from the Parent component (optional).", GH_ParamAccess.tree);
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Item JSON", "J", "List of text request body JSONs for use with Create Item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var contentTree  = new GH_Structure<GH_String>();
            var positionTree = new GH_Structure<GH_String>();
            var styleTree    = new GH_Structure<GH_String>();
            var parentTree   = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out contentTree) || !DA.GetDataTree(1, out positionTree)) return;
            DA.GetDataTree(2, out styleTree);
            DA.GetDataTree(3, out parentTree);

            var contents  = Flatten(contentTree);
            var positions = Flatten(positionTree);
            var styles    = Flatten(styleTree);
            var parents   = Flatten(parentTree);

            int count = Math.Max(contents.Count, positions.Count);
            if (count == 0) return;

            var results = new List<string>(count);
            for (int i = 0; i < count; i++)
                results.Add(TextBuilders.BuildTextBody(
                    Pick(contents, i), Pick(positions, i), Pick(styles, i), Pick(parents, i)));

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_TextJson;
        public override Guid ComponentGuid => new Guid("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b");
    }
}
