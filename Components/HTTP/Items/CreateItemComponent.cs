using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Items
{
    public class CreateItemComponent : ButtonComponent
    {
        // Maps the user-facing type string to the Miro API path segment.
        private static readonly Dictionary<string, string> TypeToPath = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "sticky_note",  "sticky_notes" },
            { "sticky",       "sticky_notes" },
            { "shape",        "shapes" },
            { "text",         "texts" },
            { "card",         "cards" },
            { "frame",        "frames" },
            { "image",        "images" },
        };

        public CreateItemComponent()
            : base("Create Item", "CreateItem",
                "Creates one or more Miro board items from a list of JSON payloads. Item type must match the JSON bodies.",
                "MiroConnect", "Items")
        { }

        public override string ButtonLabel => "Create";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board to create the items on.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Item Type", "Type", "Item type: sticky_note, shape, text, card, frame, or image.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Item JSON", "J", "List or tree of request body JSONs from the matching JSON component.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Item IDs", "IDs", "IDs of the newly created items.", GH_ParamAccess.list);
            pManager.AddTextParameter("Responses", "R", "Raw JSON responses from the Miro API.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            // All inputs are tree access so this component always runs exactly once,
            // regardless of how paths are structured upstream.
            var tokenTree    = new GH_Structure<GH_String>();
            var boardTree    = new GH_Structure<GH_String>();
            var typeTree     = new GH_Structure<GH_String>();
            var itemJsonTree = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out tokenTree)    || tokenTree.IsEmpty) return;
            if (!DA.GetDataTree(1, out boardTree)    || boardTree.IsEmpty) return;
            if (!DA.GetDataTree(2, out typeTree)     || typeTree.IsEmpty) return;
            if (!DA.GetDataTree(3, out itemJsonTree) || itemJsonTree.IsEmpty) return;

            string token    = tokenTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;
            string boardId  = boardTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;
            string itemType = typeTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(boardId) ||
                string.IsNullOrWhiteSpace(itemType)) return;

            var itemJsonList = new List<string>();
            foreach (var branch in itemJsonTree.Branches)
                foreach (var goo in branch)
                    if (goo != null && !string.IsNullOrWhiteSpace(goo.Value))
                        itemJsonList.Add(goo.Value);

            if (itemJsonList.Count == 0) return;

            if (!TypeToPath.TryGetValue(itemType, out string segment))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                    $"Unknown item type '{itemType}'. Use: sticky_note, shape, text, card, frame, image.");
                return;
            }

            string path = $"/boards/{Uri.EscapeDataString(boardId)}/{segment}";
            var ids       = new List<string>();
            var responses = new List<string>();

            foreach (string itemJson in itemJsonList)
            {
                try
                {
                    string response;

                    // Detect binary image upload sentinel produced by ImageJsonComponent
                    var parsed = JObject.Parse(itemJson);
                    if (parsed["_imageUpload"]?.Value<bool>() == true)
                    {
                        byte[] imageBytes = Convert.FromBase64String(parsed["_data"].ToString());
                        string mimeType   = parsed["_mime"]?.ToString() ?? "image/png";

                        var metadata = new JObject();
                        if (parsed["position"] != null) metadata["position"] = parsed["position"];
                        if (parsed["geometry"] != null) metadata["geometry"] = parsed["geometry"];
                        if (parsed["parent"]   != null) metadata["parent"]   = parsed["parent"];

                        response = MiroClient.PostImageUpload(token, path, imageBytes, mimeType,
                            metadata.Count > 0 ? metadata.ToString(Newtonsoft.Json.Formatting.None) : null);
                    }
                    else
                    {
                        response = MiroClient.Post(token, path, itemJson);
                    }

                    var root = JObject.Parse(response);
                    ids.Add(root["id"]?.ToString());
                    responses.Add(response);
                }
                catch (Exception ex)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Item {ids.Count}: {ex.Message}");
                    ids.Add(null);
                    responses.Add(null);
                }
            }

            DA.SetDataList(0, ids);
            DA.SetDataList(1, responses);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_CreateItem;
        public override Guid ComponentGuid => new Guid("c4d5e6f7-a8b9-4c0d-1e2f-3a4b5c6d7e8f");
    }
}
