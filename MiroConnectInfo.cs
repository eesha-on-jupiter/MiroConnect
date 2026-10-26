using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace MiroConnect
{
    // assembly metadata shown in Grasshopper's loaded-libraries list and the Yak package
    public class MiroConnectInfo : GH_AssemblyInfo
    {
        public override string Name => "MiroConnect";

        // same logo as the category icon
        public override Bitmap Icon => Properties.Resources.MC_MiroConnectLogo;

        public override string Description =>
            "Grasshopper components for the Miro REST API — build sticky notes, shapes, text, cards, frames, images, connectors and tags on a Miro board from Rhino/Grasshopper. Made with the PleaseREST Claude Code plugin.";

        public override Guid Id => new Guid("481cda9e-bd2d-48ed-bb3c-9dc1e7f83d55");

        public override string AuthorName => "Eesha Jain";

        public override string AuthorContact => "toeesha@gmail.com";

        // single source of truth is the .csproj <Version> (CI overrides it with -p:Version from the tag)
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString(3);
    }

    // sets the ribbon category icon and symbol at load time
    public class MiroConnectCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon("MiroConnect", Properties.Resources.MC_MiroConnectLogo);
            Instances.ComponentServer.AddCategorySymbolName("MiroConnect", 'M');
            return GH_LoadingInstruction.Proceed;
        }
    }
}
