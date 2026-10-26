using System;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Style
{
    public class ShapeStyleComponent : GH_Component
    {
        public ShapeStyleComponent()
            : base("Shape Style", "ShapeStyle",
                "Builds a Miro shape style JSON object.",
                "MiroConnect", "Style")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Fill Color", "Fill", "Fill color as hex string (e.g. #ffffff).", GH_ParamAccess.item, "#ffffff");
            pManager.AddTextParameter("Stroke Color", "Stroke", "Border color as hex string (e.g. #1a1a1a).", GH_ParamAccess.item, "#1a1a1a");
            pManager.AddNumberParameter("Stroke Width", "Width", "Border width in pixels.", GH_ParamAccess.item, 2.0);
            pManager.AddIntegerParameter("Font Size", "Size", "Font size in points.", GH_ParamAccess.item, 14);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Style JSON", "S", "Shape style JSON for use with Shape JSON component.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string fillColor = "#ffffff", strokeColor = "#1a1a1a";
            double strokeWidth = 2.0;
            int fontSize = 14;
            DA.GetData(0, ref fillColor);
            DA.GetData(1, ref strokeColor);
            DA.GetData(2, ref strokeWidth);
            DA.GetData(3, ref fontSize);

            DA.SetData(0, ShapeBuilders.BuildShapeStyle(fillColor, strokeColor, strokeWidth, fontSize));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_ShapeStyle;
        public override Guid ComponentGuid => new Guid("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a");
    }
}
