using System;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Style
{
    public class FrameStyleComponent : GH_Component
    {
        public FrameStyleComponent()
            : base("Frame Style", "FrameStyle",
                "Builds a Miro frame style JSON object.",
                "MiroConnect", "Style")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Fill Color", "Fill", "Frame background fill color as hex string.", GH_ParamAccess.item, "#ffffff");
            pManager.AddTextParameter("Border Color", "Border", "Frame border color as hex string.", GH_ParamAccess.item, "#1a1a1a");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Style JSON", "S", "Frame style JSON for use with Frame JSON component.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string fillColor = "#ffffff", borderColor = "#1a1a1a";
            DA.GetData(0, ref fillColor);
            DA.GetData(1, ref borderColor);

            DA.SetData(0, FrameBuilders.BuildFrameStyle(fillColor, borderColor));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_FrameStyle;
        public override Guid ComponentGuid => new Guid("a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d");
    }
}
