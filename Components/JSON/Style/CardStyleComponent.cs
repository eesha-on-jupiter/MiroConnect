using System;
using Grasshopper.Kernel;
using MiroConnect.JsonBuilders;

namespace MiroConnect.JSON.Style
{
    public class CardStyleComponent : GH_Component
    {
        public CardStyleComponent()
            : base("Card Style", "CardStyle",
                "Builds a Miro card style JSON object.",
                "MiroConnect", "Style")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Card Color", "Color", "Card theme color as hex string (e.g. #2d9bf0). Use the CardColor preset.", GH_ParamAccess.item, "#2d9bf0");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Style JSON", "S", "Card style JSON for use with Card JSON component.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string cardColor = "#2d9bf0";
            DA.GetData(0, ref cardColor);

            DA.SetData(0, CardBuilders.BuildCardStyle(cardColor));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_CardStyle;
        public override Guid ComponentGuid => new Guid("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c");
    }
}
