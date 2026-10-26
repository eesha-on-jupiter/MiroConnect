using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiroConnect.Presets
{
    // Hex color values for the Miro v2 card cardTheme field.
    public class CardColorPresetComponent : GH_ValueList
    {
        public CardColorPresetComponent()
        {
            Category    = "MiroConnect";
            SubCategory = "Presets";
            Name        = "Card Color";
            NickName    = "CC";
            Description = "Preset list of Miro card theme hex color values for use with Card Style.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Default",    "#e6e9ef" },
            { "Red",        "#f24726" },
            { "Orange",     "#ff9f00" },
            { "Yellow",     "#ffd500" },
            { "Green",      "#1db747" },
            { "Dark Green", "#0ca678" },
            { "Cyan",       "#00c2e0" },
            { "Blue",       "#2d9bf0" },
            { "Violet",     "#9510ac" },
            { "Pink",       "#ff5c74" },
        };

        protected override Bitmap Icon => Properties.Resources.MC_CardColorPreset;
        public override Guid ComponentGuid => new Guid("c3d4e5f6-a7b8-4c9d-0e1f-a2b3c4d5e6f7");
    }
}
