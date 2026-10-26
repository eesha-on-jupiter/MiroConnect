using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Items
{
    public class ShapeJsonComponent : GH_Component
    {
        public ShapeJsonComponent()
            : base("Shape JSON", "ShapeJSON",
                "Builds Miro shape request body JSONs from lists of inputs.",
                "MiroConnect", "Items JSON")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Shape Type", "T", "List or tree of shape type strings. Use the ShapeType preset.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Content", "C", "List or tree of text contents inside each shape (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Position JSON", "G", "List or tree of Position JSONs from the Position component.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Style JSON", "S", "Style JSON from the Shape Style component (optional).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Parent JSON", "P", "List or tree of Parent JSONs from the Parent component (optional).", GH_ParamAccess.tree);
            pManager[1].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Item JSON", "J", "List of shape request body JSONs for use with Create Item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var shapeTree    = new GH_Structure<GH_String>();
            var contentTree  = new GH_Structure<GH_String>();
            var positionTree = new GH_Structure<GH_String>();
            var styleTree    = new GH_Structure<GH_String>();
            var parentTree   = new GH_Structure<GH_String>();

            DA.GetDataTree(0, out shapeTree);
            DA.GetDataTree(1, out contentTree);
            if (!DA.GetDataTree(2, out positionTree)) return;
            DA.GetDataTree(3, out styleTree);
            DA.GetDataTree(4, out parentTree);

            var shapes    = Flatten(shapeTree);
            var contents  = Flatten(contentTree);
            var positions = Flatten(positionTree);
            var styles    = Flatten(styleTree);
            var parents   = Flatten(parentTree);

            if (positions.Count == 0) return;

            var results = new List<string>(positions.Count);
            for (int i = 0; i < positions.Count; i++)
                results.Add(ShapeBuilders.BuildShapeBody(
                    Pick(shapes, i) ?? "rectangle",
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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_ShapeJson;
        public override Guid ComponentGuid => new Guid("d0e1f2a3-b4c5-4d6e-7f8a-9b0c1d2e3f4a");
    }
}
