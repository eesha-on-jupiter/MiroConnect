using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Items
{
    public class ConnectorJsonComponent : GH_Component
    {
        public ConnectorJsonComponent()
            : base("Connector JSON", "ConnJSON",
                "Builds Miro connector request body JSONs from lists of inputs.",
                "MiroConnect", "Items JSON")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Start Item ID", "Start", "List or tree of item IDs where connectors start.", GH_ParamAccess.tree);
            pManager.AddTextParameter("End Item ID", "End", "List or tree of item IDs where connectors end.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Style JSON", "S", "Style JSON from the Connector Style component (optional).", GH_ParamAccess.tree);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Connector JSON", "J", "List of connector request body JSONs for use with Create Connector.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var startTree = new GH_Structure<GH_String>();
            var endTree   = new GH_Structure<GH_String>();
            var styleTree = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out startTree) || !DA.GetDataTree(1, out endTree)) return;
            DA.GetDataTree(2, out styleTree);

            var starts = Flatten(startTree);
            var ends   = Flatten(endTree);
            var styles = Flatten(styleTree);

            int count = Math.Max(starts.Count, ends.Count);
            if (count == 0) return;

            var results = new List<string>(count);
            for (int i = 0; i < count; i++)
                results.Add(ConnectorBuilders.BuildConnectorBody(
                    Pick(starts, i), Pick(ends, i), Pick(styles, i)));

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_ConnectorJson;
        public override Guid ComponentGuid => new Guid("c5d6e7f8-a9b0-4c1d-2e3f-4a5b6c7d8e9f");
    }
}
