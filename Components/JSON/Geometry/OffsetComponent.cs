using System;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;
using Newtonsoft.Json.Linq;

namespace MiroConnect.JSON.Geometry
{
    // Shifts an existing position by an X and Y offset, keeping size and rotation unchanged.
    public class OffsetComponent : GH_Component
    {
        public OffsetComponent()
            : base("Offset", "Offset",
                "Shifts a Position JSON by an X and Y offset. Width, height, and rotation are preserved.",
                "MiroConnect", "Geometry")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Position JSON", "G", "Position JSON from the Position component.", GH_ParamAccess.item);
            pManager.AddNumberParameter("X Offset", "X", "Amount to shift along the X axis (positive = right).", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Y Offset", "Y", "Amount to shift along the Y axis (positive = down).", GH_ParamAccess.item, 0.0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Position JSON", "G", "Offset position JSON for use with item JSON components.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string positionJson = null;
            double offsetX = 0, offsetY = 0;
            if (!DA.GetData(0, ref positionJson)) return;
            DA.GetData(1, ref offsetX);
            DA.GetData(2, ref offsetY);

            var g = JObject.Parse(positionJson);
            double x        = g["x"]?.Value<double>()        ?? 0;
            double y        = g["y"]?.Value<double>()        ?? 0;
            double width    = g["width"]?.Value<double>()    ?? 200;
            double height   = g["height"]?.Value<double>()   ?? 200;
            double rotation = g["rotation"]?.Value<double>() ?? 0;

            DA.SetData(0, SharedBuilders.BuildGeometry(x + offsetX, y + offsetY, width, height, rotation));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_Offset;
        public override Guid ComponentGuid => new Guid("f8a9b0c1-d2e3-4f4a-5b6c-7d8e9f0a1b2c");
    }
}
