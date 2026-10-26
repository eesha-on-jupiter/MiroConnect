using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Items
{
    public class GetAllItemsComponent : GH_Component
    {
        public GetAllItemsComponent()
            : base("Get All Items", "GetAllItems",
                "Retrieves all item IDs from a Miro board, with optional type filter.",
                "MiroConnect", "Items")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board to query.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type Filter", "Type", "Item type to filter by (e.g. sticky_note, shape, frame). Leave empty for all.", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Item IDs", "IDs", "List of item IDs on the board.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null, boardId = null, typeFilter = null;
            if (!DA.GetData(0, ref token) || !DA.GetData(1, ref boardId)) return;
            DA.GetData(2, ref typeFilter);

            try
            {
                string path = $"/boards/{Uri.EscapeDataString(boardId)}/items";
                if (!string.IsNullOrEmpty(typeFilter))
                    path += $"?type={Uri.EscapeDataString(typeFilter)}";

                string response = MiroClient.Get(token, path);
                var root = JObject.Parse(response);
                var data = root["data"] as JArray;

                var ids = new List<string>();
                if (data != null)
                    foreach (var item in data)
                        ids.Add(item["id"]?.ToString());

                DA.SetDataList(0, ids);
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_GetAllItems;
        public override Guid ComponentGuid => new Guid("b3c4d5e6-f7a8-4b9c-0d1e-2f3a4b5c6d7e");
    }
}
