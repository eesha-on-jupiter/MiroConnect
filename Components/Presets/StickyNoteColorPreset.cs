using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiroConnect.Presets
{
    // Predefined fill color names accepted by the Miro v2 sticky notes API.
    public class StickyNoteColorPresetComponent : GH_ValueList
    {
        public StickyNoteColorPresetComponent()
        {
            Category    = "MiroConnect";
            SubCategory = "Presets";
            Name        = "Sticky Note Color";
            NickName    = "SNC";
            Description = "Preset list of Miro sticky note fill color values for use with Sticky Note Style.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Gray",         "gray"         },
            { "Light Yellow", "light_yellow" },
            { "Yellow",       "yellow"       },
            { "Orange",       "orange"       },
            { "Light Green",  "light_green"  },
            { "Green",        "green"        },
            { "Dark Green",   "dark_green"   },
            { "Cyan",         "cyan"         },
            { "Light Pink",   "light_pink"   },
            { "Pink",         "pink"         },
            { "Violet",       "violet"       },
            { "Red",          "red"          },
            { "Light Blue",   "light_blue"   },
            { "Blue",         "blue"         },
            { "Dark Blue",    "dark_blue"    },
            { "Black",        "black"        },
        };

        protected override Bitmap Icon => Properties.Resources.MC_StickyNoteColorPreset;
        public override Guid ComponentGuid => new Guid("b2c3d4e5-f6a7-4b8c-9d0e-f1a2b3c4d5e6");
    }
}
