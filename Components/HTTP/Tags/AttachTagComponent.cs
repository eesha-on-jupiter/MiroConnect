using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.Utilities;

namespace MiroConnect.HTTP.Tags
{
    public class AttachTagComponent : ButtonComponent
    {
        public AttachTagComponent()
            : base("Attach Tag", "AttachTag",
                "Attaches a Miro tag to one or more board items.",
                "MiroConnect", "Tags")
        { }

        public override string ButtonLabel => "Attach";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Item IDs", "Items", "List or tree of item IDs to attach the tag to.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tag ID", "Tag", "ID of the tag to attach.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Responses", "R", "Raw JSON responses from the Miro API.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            var tokenTree = new GH_Structure<GH_String>();
            var boardTree = new GH_Structure<GH_String>();
            var itemTree  = new GH_Structure<GH_String>();
            var tagTree   = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out tokenTree) || tokenTree.IsEmpty) return;
            if (!DA.GetDataTree(1, out boardTree) || boardTree.IsEmpty) return;
            if (!DA.GetDataTree(2, out itemTree)  || itemTree.IsEmpty)  return;
            if (!DA.GetDataTree(3, out tagTree)   || tagTree.IsEmpty)   return;

            string token   = tokenTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;
            string boardId = boardTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;
            string tagId   = tagTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(boardId) ||
                string.IsNullOrWhiteSpace(tagId)) return;

            var itemIds = new List<string>();
            foreach (var branch in itemTree.Branches)
                foreach (var goo in branch)
                    if (goo != null && !string.IsNullOrWhiteSpace(goo.Value))
                        itemIds.Add(goo.Value);

            if (itemIds.Count == 0) return;

            var responses = new List<string>();

            foreach (string itemId in itemIds)
            {
                try
                {
                    string path = $"/boards/{Uri.EscapeDataString(boardId)}/items/{Uri.EscapeDataString(itemId)}/tags/{Uri.EscapeDataString(tagId)}";
                    string response = MiroClient.Post(token, path, null);
                    responses.Add(response);
                }
                catch (Exception ex)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Item {responses.Count}: {ex.Message}");
                    responses.Add(null);
                }
            }

            DA.SetDataList(0, responses);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_AttachTag;
        public override Guid ComponentGuid => new Guid("d1e2f3a4-b5c6-4d7e-8f9a-0b1c2d3e4f5a");
    }
}
