using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiroConnect.Presets
{
    // Dash pattern strings accepted by the Miro v2 connector strokeStyle field
    public class StrokeStylePresetComponent : GH_ValueList
    {
        public StrokeStylePresetComponent()
        {
            Category    = "MiroConnect";
            SubCategory = "Presets";
            Name        = "Stroke Style";
            NickName    = "SS";
            Description = "Preset list of Miro connector dash patterns for Connector Style's Line Style input.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Solid",  "normal" },
            { "Dashed", "dashed" },
            { "Dotted", "dotted" },
        };

        protected override Bitmap Icon => Properties.Resources.MC_StrokeStylePreset;
        public override Guid ComponentGuid => new Guid("e5f6a7b8-c9d0-4e1f-2031-c4d5e6f7a8b9");
    }
}
