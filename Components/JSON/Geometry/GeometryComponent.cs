using System;
using System.Drawing;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Geometry
{
    public class PositionComponent : GH_Component
    {
        public PositionComponent()
            : base("Position", "Position",
                "Builds a Miro position JSON object from center coordinates, size, and rotation.",
                "MiroConnect", "Geometry")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("X", "X", "Center X position on the board canvas.", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Y", "Y", "Center Y position on the board canvas.", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Width", "W", "Item width in pixels.", GH_ParamAccess.item, 200.0);
            pManager.AddNumberParameter("Height", "H", "Item height in pixels.", GH_ParamAccess.item, 200.0);
            pManager.AddNumberParameter("Rotation", "R", "Rotation in degrees (clockwise).", GH_ParamAccess.item, 0.0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Position JSON", "G", "Position JSON for use with item JSON components.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double x = 0, y = 0, width = 200, height = 200, rotation = 0;
            DA.GetData(0, ref x);
            DA.GetData(1, ref y);
            DA.GetData(2, ref width);
            DA.GetData(3, ref height);
            DA.GetData(4, ref rotation);

            DA.SetData(0, SharedBuilders.BuildGeometry(x, y, width, height, rotation));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_Position;
        public override Guid ComponentGuid => new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d");
    }
}
