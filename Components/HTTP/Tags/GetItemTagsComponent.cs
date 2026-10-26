using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Tags
{
    public class GetItemTagsComponent : GH_Component
    {
        public GetItemTagsComponent()
            : base("Get Item Tags", "GetItemTags",
                "Retrieves all tags attached to a Miro board item.",
                "MiroConnect", "Tags")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board.", GH_ParamAccess.item);
            pManager.AddTextParameter("Item ID", "Item", "ID of the item to retrieve tags from.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Tag IDs", "IDs", "IDs of all tags attached to the item.", GH_ParamAccess.list);
            pManager.AddTextParameter("Tag Titles", "Titles", "Titles of all tags attached to the item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null, boardId = null, itemId = null;
            if (!DA.GetData(0, ref token) || !DA.GetData(1, ref boardId) || !DA.GetData(2, ref itemId)) return;

            try
            {
                string response = MiroClient.Get(token,
                    $"/boards/{Uri.EscapeDataString(boardId)}/items/{Uri.EscapeDataString(itemId)}/tags");
                var root = JObject.Parse(response);
                var data = root["tags"] as JArray ?? root["data"] as JArray;

                var ids = new List<string>();
                var titles = new List<string>();

                if (data != null)
                    foreach (var tag in data)
                    {
                        ids.Add(tag["id"]?.ToString());
                        titles.Add(tag["title"]?.ToString());
                    }

                DA.SetDataList(0, ids);
                DA.SetDataList(1, titles);
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_GetItemTags;
        public override Guid ComponentGuid => new Guid("b9c0d1e2-f3a4-4b5c-6d7e-8f9a0b1c2d3e");
    }
}
