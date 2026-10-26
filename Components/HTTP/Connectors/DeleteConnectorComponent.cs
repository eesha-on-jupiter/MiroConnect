using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.Utilities;

namespace MiroConnect.HTTP.Connectors
{
    public class DeleteConnectorComponent : ButtonComponent
    {
        public DeleteConnectorComponent()
            : base("Delete Connector", "DeleteConnector",
                "Deletes one or more Miro connectors by ID.",
                "MiroConnect", "Connectors")
        { }

        public override string ButtonLabel => "Delete";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board containing the connectors.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Connector IDs", "IDs", "List or tree of connector IDs to delete.", GH_ParamAccess.tree);
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
            var idTree    = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out tokenTree) || tokenTree.IsEmpty) return;
            if (!DA.GetDataTree(1, out boardTree) || boardTree.IsEmpty) return;
            if (!DA.GetDataTree(2, out idTree)    || idTree.IsEmpty)    return;

            string token   = tokenTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;
            string boardId = boardTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(boardId)) return;

            var idList = new List<string>();
            foreach (var branch in idTree.Branches)
                foreach (var goo in branch)
                    if (goo != null && !string.IsNullOrWhiteSpace(goo.Value))
                        idList.Add(goo.Value);

            if (idList.Count == 0) return;

            var responses = new List<string>();

            foreach (string connectorId in idList)
            {
                try
                {
                    string response = MiroClient.Delete(token,
                        $"/boards/{Uri.EscapeDataString(boardId)}/connectors/{Uri.EscapeDataString(connectorId)}");
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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_DeleteConnector;
        public override Guid ComponentGuid => new Guid("a8b9c0d1-e2f3-4a4b-5c6d-7e8f9a0b1c2d");
    }
}
