using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiroConnect.Presets
{
    // Shape type strings accepted by the Miro v2 shapes API.
    public class ShapeTypePresetComponent : GH_ValueList
    {
        public ShapeTypePresetComponent()
        {
            Category    = "MiroConnect";
            SubCategory = "Presets";
            Name        = "Shape Type";
            NickName    = "ST";
            Description = "Preset list of Miro shape type values for use with Shape JSON.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Rectangle",        "rectangle"        },
            { "Round Rectangle",  "round_rectangle"  },
            { "Circle",           "circle"           },
            { "Triangle",         "triangle"         },
            { "Rhombus",          "rhombus"          },
            { "Parallelogram",    "parallelogram"     },
            { "Trapezoid",        "trapezoid"        },
            { "Pentagon",         "pentagon"         },
            { "Hexagon",          "hexagon"          },
            { "Octagon",          "octagon"          },
            { "Star",             "star"             },
            { "Arrow Left",       "arrow_left"       },
            { "Arrow Right",      "arrow_right"      },
            { "Arrow Up Down",    "arrow_up_down"    },
            { "Cross",            "cross"            },
            { "Cloud",            "cloud"            },
            { "Can",              "can"              },
            { "Message Bubble",   "message_bubble"   },
        };

        protected override Bitmap Icon => Properties.Resources.MC_ShapeTypePreset;
        public override Guid ComponentGuid => new Guid("a1b2c3d4-e5f6-4a7b-8c9d-e0f1a2b3c4d5");
    }
}
