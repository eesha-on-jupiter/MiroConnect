using System;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Style
{
    public class TextStyleComponent : GH_Component
    {
        public TextStyleComponent()
            : base("Text Style", "TextStyle",
                "Builds a Miro text item style JSON object.",
                "MiroConnect", "Style")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Fill Color", "Fill", "Background fill color as hex string, or 'transparent'.", GH_ParamAccess.item, "transparent");
            pManager.AddTextParameter("Font Family", "Font", "Font family name (e.g. arial, open_sans). Use the FontFamily preset.", GH_ParamAccess.item, "arial");
            pManager.AddIntegerParameter("Font Size", "Size", "Font size in points.", GH_ParamAccess.item, 14);
            pManager.AddTextParameter("Text Align", "Align", "Horizontal text alignment: left, center, or right.", GH_ParamAccess.item, "center");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Style JSON", "S", "Text style JSON for use with Text JSON component.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string fillColor = "transparent", fontFamily = "arial", textAlign = "center";
            int fontSize = 14;
            DA.GetData(0, ref fillColor);
            DA.GetData(1, ref fontFamily);
            DA.GetData(2, ref fontSize);
            DA.GetData(3, ref textAlign);

            DA.SetData(0, TextBuilders.BuildTextStyle(fillColor, fontFamily, fontSize, textAlign));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_TextStyle;
        public override Guid ComponentGuid => new Guid("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b");
    }
}
