using System;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Style
{
    public class ConnectorStyleComponent : GH_Component
    {
        public ConnectorStyleComponent()
            : base("Connector Style", "ConnStyle",
                "Builds a Miro connector style JSON object.",
                "MiroConnect", "Style")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Line Style", "Style", "Line routing (curved, elbowed, straight) or dash pattern (normal, dashed, dotted) from the Stroke Style preset.", GH_ParamAccess.item, "curved");
            pManager.AddTextParameter("Stroke Color", "Color", "Stroke color as hex string (e.g. #1a1a1a).", GH_ParamAccess.item, "#1a1a1a");
            pManager.AddNumberParameter("Stroke Width", "Width", "Stroke width in pixels.", GH_ParamAccess.item, 2.0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Style JSON", "S", "Connector style JSON for use with Connector JSON component.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string lineStyle = "curved", strokeColor = "#1a1a1a";
            double strokeWidth = 2.0;
            DA.GetData(0, ref lineStyle);
            DA.GetData(1, ref strokeColor);
            DA.GetData(2, ref strokeWidth);

            DA.SetData(0, ConnectorBuilders.BuildConnectorStyle(lineStyle, strokeColor, strokeWidth));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_ConnectorStyle;
        public override Guid ComponentGuid => new Guid("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e");
    }
}
