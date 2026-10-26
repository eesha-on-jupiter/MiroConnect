using System;
using Grasshopper.Kernel;
using MiroConnect.Utilities;

namespace MiroConnect.Auth
{
    public class MiroAuthComponent : GH_Component
    {
        public MiroAuthComponent()
            : base("Miro Auth", "MiroAuth",
                "Validates a Miro personal access token and confirms API connectivity.",
                "MiroConnect", "Auth")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro personal access token or OAuth bearer token.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "S", "Connection status: 'Connected' or an error message.", GH_ParamAccess.item);
            pManager.AddTextParameter("Base URL", "URL", "Miro API base URL in use.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null;
            if (!DA.GetData(0, ref token) || string.IsNullOrWhiteSpace(token))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No token provided.");
                return;
            }

            try
            {
                // Validate by fetching a minimal board list — cheap, read-only, no side effects.
                MiroClient.Get(token, "/boards?limit=1");
                DA.SetData(0, "Connected");
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
                DA.SetData(0, $"Error: {ex.Message}");
            }

            DA.SetData(1, MiroClient.BaseUrl);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_MiroAuth;
        public override Guid ComponentGuid => new Guid("7e4a5b8c-9d2f-4c1e-a3b7-f8e9d6c5b4a3");
    }
}
