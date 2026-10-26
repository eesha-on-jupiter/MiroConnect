using System;
using Grasshopper.Kernel;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Boards
{
    public class GetBoardComponent : GH_Component
    {
        public GetBoardComponent()
            : base("Get Board", "GetBoard",
                "Retrieves details for a Miro board by its ID.",
                "MiroConnect", "Boards")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board ID", "ID", "ID of the board to retrieve.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Board name.", GH_ParamAccess.item);
            pManager.AddTextParameter("Team", "Team", "Name of the team that owns the board.", GH_ParamAccess.item);
            pManager.AddTextParameter("Created At", "Date", "Board creation date (ISO 8601).", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null, boardId = null;
            if (!DA.GetData(0, ref token) || !DA.GetData(1, ref boardId)) return;

            try
            {
                string json = MiroClient.Get(token, $"/boards/{Uri.EscapeDataString(boardId)}");
                var root = JObject.Parse(json);

                DA.SetData(0, root["name"]?.ToString());
                DA.SetData(1, root["team"]?["name"]?.ToString());
                DA.SetData(2, root["createdAt"]?.ToString());
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_GetBoard;
        public override Guid ComponentGuid => new Guid("5c9d4e8f-2b3a-4c7d-9e1f-b2a3c4d5e6f7");
    }
}
