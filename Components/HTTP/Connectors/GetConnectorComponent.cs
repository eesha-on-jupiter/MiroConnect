using System;
using Grasshopper.Kernel;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Connectors
{
    public class GetConnectorComponent : GH_Component
    {
        public GetConnectorComponent()
            : base("Get Connector", "GetConnector",
                "Retrieves a Miro connector by its ID.",
                "MiroConnect", "Connectors")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board containing the connector.", GH_ParamAccess.item);
            pManager.AddTextParameter("Connector ID", "ID", "ID of the connector to retrieve.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Start Item ID", "Start", "ID of the item at the connector start.", GH_ParamAccess.item);
            pManager.AddTextParameter("End Item ID", "End", "ID of the item at the connector end.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from the Miro API.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null, boardId = null, connectorId = null;
            if (!DA.GetData(0, ref token) || !DA.GetData(1, ref boardId) || !DA.GetData(2, ref connectorId)) return;

            try
            {
                string response = MiroClient.Get(token,
                    $"/boards/{Uri.EscapeDataString(boardId)}/connectors/{Uri.EscapeDataString(connectorId)}");
                var root = JObject.Parse(response);

                DA.SetData(0, root["startItem"]?["id"]?.ToString());
                DA.SetData(1, root["endItem"]?["id"]?.ToString());
                DA.SetData(2, response);
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_GetConnector;
        public override Guid ComponentGuid => new Guid("e6f7a8b9-c0d1-4e2f-3a4b-5c6d7e8f9a0b");
    }
}
