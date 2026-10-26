using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Items
{
    public class CardJsonComponent : GH_Component
    {
        public CardJsonComponent()
            : base("Card JSON", "CardJSON",
                "Builds Miro card request body JSONs from lists of inputs.",
                "MiroConnect", "Items JSON")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Title", "T", "List or tree of card titles.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Description", "D", "List or tree of card descriptions (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Due Date", "Due", "List or tree of due dates in ISO 8601 format (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Position JSON", "G", "List or tree of Position JSONs from the Position component.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Style JSON", "S", "Style JSON from the Card Style component (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Parent JSON", "P", "List or tree of Parent JSONs from the Parent component (optional).", GH_ParamAccess.tree);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Item JSON", "J", "List of card request body JSONs for use with Create Item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var titleTree    = new GH_Structure<GH_String>();
            var descTree     = new GH_Structure<GH_String>();
            var dueTree      = new GH_Structure<GH_String>();
            var positionTree = new GH_Structure<GH_String>();
            var styleTree    = new GH_Structure<GH_String>();
            var parentTree   = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out titleTree) || !DA.GetDataTree(3, out positionTree)) return;
            DA.GetDataTree(1, out descTree);
            DA.GetDataTree(2, out dueTree);
            DA.GetDataTree(4, out styleTree);
            DA.GetDataTree(5, out parentTree);

            var titles    = Flatten(titleTree);
            var descs     = Flatten(descTree);
            var dues      = Flatten(dueTree);
            var positions = Flatten(positionTree);
            var styles    = Flatten(styleTree);
            var parents   = Flatten(parentTree);

            int count = Math.Max(titles.Count, positions.Count);
            if (count == 0) return;

            var results = new List<string>(count);
            for (int i = 0; i < count; i++)
                results.Add(CardBuilders.BuildCardBody(
                    Pick(titles, i), Pick(descs, i), Pick(dues, i),
                    Pick(positions, i), Pick(styles, i), Pick(parents, i)));

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_CardJson;
        public override Guid ComponentGuid => new Guid("f2a3b4c5-d6e7-4f8a-9b0c-1d2e3f4a5b6c");
    }
}
