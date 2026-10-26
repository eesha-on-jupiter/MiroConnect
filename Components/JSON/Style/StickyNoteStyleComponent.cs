using System;
using System.Drawing;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Style
{
    public class StickyNoteStyleComponent : GH_Component
    {
        public StickyNoteStyleComponent()
            : base("Sticky Note Style", "StickyStyle",
                "Builds a Miro sticky note style JSON object.",
                "MiroConnect", "Style")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Fill Color", "Fill", "Named fill color (e.g. light_yellow, cyan, pink). Use the StickyNoteColor preset.", GH_ParamAccess.item, "light_yellow");
            pManager.AddTextParameter("Text Align", "Align", "Horizontal text alignment: left, center, or right.", GH_ParamAccess.item, "center");
            pManager.AddIntegerParameter("Font Size", "Size", "Font size in points.", GH_ParamAccess.item, 14);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Style JSON", "S", "Sticky note style JSON for use with Sticky Note JSON component.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string fillColor = "light_yellow", textAlign = "center";
            int fontSize = 14;
            DA.GetData(0, ref fillColor);
            DA.GetData(1, ref textAlign);
            DA.GetData(2, ref fontSize);

            DA.SetData(0, StickyNoteBuilders.BuildStickyNoteStyle(fillColor, textAlign, fontSize));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_StickyNoteStyle;
        public override Guid ComponentGuid => new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f");
    }
}
