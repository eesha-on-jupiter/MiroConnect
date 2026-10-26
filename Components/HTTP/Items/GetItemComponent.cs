using System;
using Grasshopper.Kernel;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Items
{
    public class GetItemComponent : GH_Component
    {
        public GetItemComponent()
            : base("Get Item", "GetItem",
                "Retrieves a single Miro board item by its ID.",
                "MiroConnect", "Items")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board containing the item.", GH_ParamAccess.item);
            pManager.AddTextParameter("Item ID", "ID", "ID of the item to retrieve.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Type", "Type", "Item type (e.g. sticky_note, shape, frame).", GH_ParamAccess.item);
            pManager.AddNumberParameter("X", "X", "Item center X position.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Y", "Y", "Item center Y position.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "W", "Item width.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height", "H", "Item height.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from the Miro API.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null, boardId = null, itemId = null;
            if (!DA.GetData(0, ref token) || !DA.GetData(1, ref boardId) || !DA.GetData(2, ref itemId)) return;

            try
            {
                string response = MiroClient.Get(token,
                    $"/boards/{Uri.EscapeDataString(boardId)}/items/{Uri.EscapeDataString(itemId)}");
                var root = JObject.Parse(response);

                DA.SetData(0, root["type"]?.ToString());
                DA.SetData(1, root["position"]?["x"]?.ToObject<double>() ?? 0.0);
                DA.SetData(2, root["position"]?["y"]?.ToObject<double>() ?? 0.0);
                DA.SetData(3, root["geometry"]?["width"]?.ToObject<double>() ?? 0.0);
                DA.SetData(4, root["geometry"]?["height"]?.ToObject<double>() ?? 0.0);
                DA.SetData(5, response);
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_GetItem;
        public override Guid ComponentGuid => new Guid("a2b3c4d5-e6f7-4a8b-9c0d-1e2f3a4b5c6d");
    }
}
