using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Connectors
{
    public class CreateConnectorComponent : ButtonComponent
    {
        public CreateConnectorComponent()
            : base("Create Connector", "CreateConnector",
                "Creates one or more Miro connectors from a list of JSON payloads.",
                "MiroConnect", "Connectors")
        { }

        public override string ButtonLabel => "Create";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Board ID", "Board", "ID of the board to create the connectors on.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Connector JSON", "J", "List or tree of request body JSONs from the Connector JSON component.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Connector IDs", "IDs", "IDs of the newly created connectors.", GH_ParamAccess.list);
            pManager.AddTextParameter("Responses", "R", "Raw JSON responses from the Miro API.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            var tokenTree = new GH_Structure<GH_String>();
            var boardTree = new GH_Structure<GH_String>();
            var jsonTree  = new GH_Structure<GH_String>();

            if (!DA.GetDataTree(0, out tokenTree) || tokenTree.IsEmpty) return;
            if (!DA.GetDataTree(1, out boardTree) || boardTree.IsEmpty) return;
            if (!DA.GetDataTree(2, out jsonTree)  || jsonTree.IsEmpty)  return;

            string token   = tokenTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;
            string boardId = boardTree.AllData(true).OfType<GH_String>().FirstOrDefault()?.Value;

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(boardId)) return;

            var jsonList = new List<string>();
            foreach (var branch in jsonTree.Branches)
                foreach (var goo in branch)
                    if (goo != null && !string.IsNullOrWhiteSpace(goo.Value))
                        jsonList.Add(goo.Value);

            if (jsonList.Count == 0) return;

            string path   = $"/boards/{Uri.EscapeDataString(boardId)}/connectors";
            var ids       = new List<string>();
            var responses = new List<string>();

            foreach (string connectorJson in jsonList)
            {
                try
                {
                    string response = MiroClient.Post(token, path, connectorJson);
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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_CreateConnector;
        public override Guid ComponentGuid => new Guid("f7a8b9c0-d1e2-4f3a-4b5c-6d7e8f9a0b1c");
    }
}
