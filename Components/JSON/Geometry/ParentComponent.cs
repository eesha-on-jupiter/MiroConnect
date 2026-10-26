using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Geometry
{
    public class ParentComponent : GH_Component
    {
        public ParentComponent()
            : base("Parent", "Parent",
                "Builds Miro parent JSON objects from a list of frame IDs, placing items inside frames.",
                "MiroConnect", "Geometry")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Frame IDs", "F", "List or tree of frame item IDs.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Parent JSON", "P", "List of parent JSON objects for use with item JSON components.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var idTree = new GH_Structure<GH_String>();
            if (!DA.GetDataTree(0, out idTree)) return;

            var results = new List<string>();
            foreach (var branch in idTree.Branches)
                foreach (var goo in branch)
                    if (goo != null && !string.IsNullOrWhiteSpace(goo.Value))
                        results.Add(SharedBuilders.BuildParent(goo.Value));

            if (results.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No frame IDs provided.");
                return;
            }

            DA.SetDataList(0, results);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_Parent;
        public override Guid ComponentGuid => new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e");
    }
}
