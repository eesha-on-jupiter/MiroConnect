using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiroConnect.Presets
{
    // Font family strings accepted by the Miro v2 text style API
    public class FontFamilyPresetComponent : GH_ValueList
    {
        public FontFamilyPresetComponent()
        {
            Category    = "MiroConnect";
            SubCategory = "Presets";
            Name        = "Font Family";
            NickName    = "FF";
            Description = "Preset list of Miro font family values for use with Text Style.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Arial",            "arial"            },
            { "Open Sans",        "open_sans"        },
            { "Roboto",           "roboto"           },
            { "PT Sans",          "pt_sans"          },
            { "PT Serif",         "pt_serif"         },
            { "Roboto Mono",      "roboto_mono"      },
            { "Caveat",           "caveat"           },
            { "Permanent Marker", "permanent_marker" },
            { "Georgia",          "georgia"          },
            { "Times New Roman",  "times_new_roman"  },
        };

        protected override Bitmap Icon => Properties.Resources.MC_FontFamilyPreset;
        public override Guid ComponentGuid => new Guid("d4e5f6a7-b8c9-4d0e-1f20-b3c4d5e6f7a8");
    }
}
