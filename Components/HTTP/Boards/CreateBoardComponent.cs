using System;
using Grasshopper.Kernel;
using MiroConnect.Utilities;
using Newtonsoft.Json.Linq;

namespace MiroConnect.HTTP.Boards
{
    public class CreateBoardComponent : ButtonComponent
    {
        public CreateBoardComponent()
            : base("Create Board", "CreateBoard",
                "Creates a new Miro board.",
                "MiroConnect", "Boards")
        { }

        public override string ButtonLabel => "Create";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board Name", "Name", "Name for the new board.", GH_ParamAccess.item);
            pManager.AddTextParameter("Team ID", "Team", "ID of the team to create the board in.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Board ID", "ID", "ID of the newly created board.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from the Miro API.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = null, name = null, teamId = null;
            if (!DA.GetData(0, ref token) || !DA.GetData(1, ref name) || !DA.GetData(2, ref teamId)) return;

            try
            {
                var body = new JObject
                {
                    ["name"] = name,
                    ["teamId"] = teamId
                };

                string response = MiroClient.Post(token, "/boards", body.ToString());
                var root = JObject.Parse(response);

                DA.SetData(0, root["id"]?.ToString());
                DA.SetData(1, response);
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_CreateBoard;
        public override Guid ComponentGuid => new Guid("e0f1a2b3-c4d5-4e6f-7a8b-9c0d1e2f3a4b");
    }
}
