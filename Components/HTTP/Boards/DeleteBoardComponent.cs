using System;
using Grasshopper.Kernel;
using MiroConnect.Utilities;

namespace MiroConnect.HTTP.Boards
{
    public class DeleteBoardComponent : ButtonComponent
    {
        public DeleteBoardComponent()
            : base("Delete Board", "DeleteBoard",
                "Deletes a Miro board by its ID.",
                "MiroConnect", "Boards")
        { }

        public override string ButtonLabel => "Delete";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Miro bearer token.", GH_ParamAccess.item);
            pManager.AddTextParameter("Board ID", "ID", "ID of the board to delete.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Response", "R", "Raw JSON response from the Miro API.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = null, boardId = null;
            if (!DA.GetData(0, ref token) || !DA.GetData(1, ref boardId)) return;

            try
            {
                string response = MiroClient.Delete(token, $"/boards/{Uri.EscapeDataString(boardId)}");
                DA.SetData(0, response);
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MC_DeleteBoard;
        public override Guid ComponentGuid => new Guid("f1a2b3c4-d5e6-4f7a-8b9c-0d1e2f3a4b5c");
    }
}
