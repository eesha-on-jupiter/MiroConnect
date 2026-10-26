<!-- pr:begin header -->
<img src="MC_Plugin_Icon.png" alt="MiroConnect" width="96" align="left" style="margin-right:16px">

# MiroConnect
> Grasshopper plugin for the Miro REST API — by Eesha Jain
>
> Built with the [PleaseREST](https://github.com/eesha-on-jupiter/PleaseREST) Claude Code plugin — an AI pipeline that turns a REST API reference into a Grasshopper plugin.

Build Miro boards from a Grasshopper canvas: lay out sticky notes, shapes, text, cards, frames and images with position and grid helpers, join them with connectors, tag them, and push the whole batch to a live board with one button click.

<br clear="left">

## What you can do with it

Turn design data into a board the team can work on, and regenerate it whenever the data changes:

- **Spreadsheets to sticky walls** — a CSV or Excel sheet becomes a grid of sticky notes or cards, one per row, in a single click.
- **Diagrams from the definition** — shapes laid out by Grid or Position, placed inside frames and chained with connectors, so a process or zoning diagram follows the model.
- **Kanban and review boards** — cards with titles, descriptions and due dates under text column headers, coloured and tagged for status.
- **Images on the board** — public URLs, local files or Grasshopper bitmaps uploaded straight into a frame.
- **Read back and clean up** — find boards by name, list and inspect items, read connectors and tags, and clear a board between runs.

Component chains for each of these are in [docs/workflows.md](docs/workflows.md); a full demo canvas plan is in [docs/demo-workflows.md](docs/demo-workflows.md).

**Under the hood**

- JSON and HTTP are separate — Geometry, Style and Items JSON components only build request bodies; nothing reaches Miro until an HTTP component sends them.
- One click, many items — every write is a button; Create Item flattens any JSON tree and posts every item in order, returning IDs aligned with the input.
- Center-based coordinates — positions are item centers on Miro's infinite canvas, origin at the board center, Y down; frames are filled by wiring a frame's ID through **Parent**.
<!-- pr:end header -->

<!-- pr:begin install -->
## Installation

**Food4Rhino:** (pending first release)
**Yak:** `_PackageManager` → search `miroconnect` (pending first release)

**Rhino 8 note:** the Rhino 8 packages are built for .NET 7 and load only when Rhino runs on the .NET Core runtime (its default). If the MiroConnect tab is missing, run `SetDotNetRuntime` in Rhino, choose **.NET Core**, and restart — Rhino.Inside Revit setups often switch this to .NET Framework.

**From source:** `dotnet build MiroConnect.csproj`, then drop the `.gha` for your Rhino version into Grasshopper's `Libraries` folder together with the `.dll` files beside it (Rhino 7: `bin/Debug/net48`; Rhino 8 Windows: `bin/Debug/net7.0-windows`; Rhino 8 Mac: `bin/Debug/net7.0`) and restart Rhino — or point Grasshopper's developer-folder setting at the build folder. Close Rhino before rebuilding; it locks the `.gha`.
<!-- pr:end install -->

<!-- pr:begin quickstart -->
## Quick start

1. Create a Miro app, give it `boards:read` and `boards:write`, install it on your team and copy the access token — see [docs/authentication.md](docs/authentication.md).
2. Drop **Miro Auth** (Auth tab) and plug your token into `Token` — `Status` reads `Connected`.
3. **Find Board** (Boards tab) with a board name → `Board ID`.
4. Build the content: **Position** or **Grid** (Geometry) → a style component (Style) → **Sticky Note JSON**, **Shape JSON**, **Card JSON**… (Items JSON) → `Item JSON`.
5. **Create Item** (Items tab) with the token, the Board ID, the item type (`sticky_note`, `shape`, …) and the `Item JSON` — click the button.

Full walkthrough: [docs/quickstart.md](docs/quickstart.md). Common wirings as component chains: [docs/workflows.md](docs/workflows.md). Reference for every component: [docs/index.md](docs/index.md).
<!-- pr:end quickstart -->

<!-- pr:begin components -->
## Components

| Tab | Component | Inputs | Outputs | Description |
|---|---|---|---|---|
| Auth | Miro Auth | Token | Status, Base URL | Validates a Miro personal access token and confirms API connectivity. |
| Data | Table Reader | File Path, Sheet, Has Headers | Headers, Data, Row Count, Col Count | Reads a CSV or .xlsx file and outputs headers and cell data as a column tree. |
| Boards | Create Board | Token, Board Name, Team ID | Board ID, Response | Creates a new Miro board. |
| Boards | Delete Board | Token, Board ID | Response | Deletes a Miro board by its ID. |
| Boards | Find Board | Token, Board Name | Board IDs, Board Names, Board ID | Lists all accessible Miro boards. Optionally filter by name to get a matched board ID. |
| Boards | Get Board | Token, Board ID | Name, Team, Created At | Retrieves details for a Miro board by its ID. |
| Items | Create Item | Token, Board ID, Item Type, Item JSON | Item IDs, Responses | Creates one or more Miro board items from a list of JSON payloads. Item type must match the JSON bodies. |
| Items | Delete Item | Token, Board ID, Item IDs | Responses | Deletes one or more Miro board items by ID. |
| Items | Get All Items | Token, Board ID, Type Filter | Item IDs | Retrieves all item IDs from a Miro board, with optional type filter. |
| Items | Get Item | Token, Board ID, Item ID | Type, X, Y, Width, Height, Response | Retrieves a single Miro board item by its ID. |
| Connectors | Create Connector | Token, Board ID, Connector JSON | Connector IDs, Responses | Creates one or more Miro connectors from a list of JSON payloads. |
| Connectors | Delete Connector | Token, Board ID, Connector IDs | Responses | Deletes one or more Miro connectors by ID. |
| Connectors | Get Connector | Token, Board ID, Connector ID | Start Item ID, End Item ID, Response | Retrieves a Miro connector by its ID. |
| Tags | Attach Tag | Token, Board ID, Item IDs, Tag ID | Responses | Attaches a Miro tag to one or more board items. |
| Tags | Create Tag | Token, Board ID, Tag JSON | Tag IDs, Responses | Creates one or more Miro tags on a board. |
| Tags | Detach Tag | Token, Board ID, Item IDs, Tag ID | Responses | Detaches a Miro tag from one or more board items. |
| Tags | Get Item Tags | Token, Board ID, Item ID | Tag IDs, Tag Titles | Retrieves all tags attached to a Miro board item. |
| Geometry | Grid | Count, Items Per Row, Width, Height, Gap X, Gap Y, Origin X, Origin Y | Position JSON | Generates a list of Position JSONs arranged in a grid. Items fill left to right, then top to bottom. |
| Geometry | Offset | Position JSON, X Offset, Y Offset | Position JSON | Shifts a Position JSON by an X and Y offset. Width, height, and rotation are preserved. |
| Geometry | Parent | Frame IDs | Parent JSON | Builds Miro parent JSON objects from a list of frame IDs, placing items inside frames. |
| Geometry | Position | X, Y, Width, Height, Rotation | Position JSON | Builds a Miro position JSON object from center coordinates, size, and rotation. |
| Items JSON | Card JSON | Title, Description, Due Date, Position JSON, Style JSON, Parent JSON | Item JSON | Builds Miro card request body JSONs from lists of inputs. |
| Items JSON | Connector JSON | Start Item ID, End Item ID, Style JSON | Connector JSON | Builds Miro connector request body JSONs from lists of inputs. |
| Items JSON | Frame JSON | Title, Position JSON, Style JSON | Item JSON | Builds Miro frame request body JSONs from lists of inputs. |
| Items JSON | Image JSON | Image URL, File Path, Bitmap, Position JSON, Parent JSON | Item JSON | Builds Miro image request bodies from lists of URLs, file paths, or Bitmaps. |
| Items JSON | Shape JSON | Shape Type, Content, Position JSON, Style JSON, Parent JSON | Item JSON | Builds Miro shape request body JSONs from lists of inputs. |
| Items JSON | Sticky Note JSON | Content, Position JSON, Style JSON, Parent JSON | Item JSON | Builds Miro sticky note request body JSONs from lists of inputs. |
| Items JSON | Tag JSON | Title, Fill Color | Tag JSON | Builds Miro tag request body JSONs from lists of inputs. |
| Items JSON | Text JSON | Content, Position JSON, Style JSON, Parent JSON | Item JSON | Builds Miro text item request body JSONs from lists of inputs. |
| Style | Card Style | Card Color | Style JSON | Builds a Miro card style JSON object. |
| Style | Connector Style | Line Style, Stroke Color, Stroke Width | Style JSON | Builds a Miro connector style JSON object. |
| Style | Frame Style | Fill Color, Border Color | Style JSON | Builds a Miro frame style JSON object. |
| Style | Shape Style | Fill Color, Stroke Color, Stroke Width, Font Size | Style JSON | Builds a Miro shape style JSON object. |
| Style | Sticky Note Style | Fill Color, Text Align, Font Size | Style JSON | Builds a Miro sticky note style JSON object. |
| Style | Text Style | Fill Color, Font Family, Font Size, Text Align | Style JSON | Builds a Miro text item style JSON object. |
| Presets | Card Color | — | Value list | Preset list of Miro card theme hex color values for use with Card Style. |
| Presets | Font Family | — | Value list | Preset list of Miro font family values for use with Text Style. |
| Presets | Shape Type | — | Value list | Preset list of Miro shape type values for use with Shape JSON. |
| Presets | Sticky Note Color | — | Value list | Preset list of Miro sticky note fill color values for use with Sticky Note Style. |
| Presets | Stroke Style | — | Value list | Preset list of Miro connector dash patterns for Connector Style's Line Style input. |
<!-- pr:end components -->

<!-- pr:begin requirements -->
## Requirements

- Rhino 7 or 8 (Windows or Mac — Rhino 8 Mac is untested in 1.0.0, see the [changelog](docs/changelog.md#known-limitations))
- A Miro account and a Miro app installed on the team whose boards you want to use
- OAuth scopes: `boards:read` (Miro Auth, Find/Get Board, Get Item, Get All Items, Get Connector, Get Item Tags); `boards:write` (Create/Delete Board, Create/Delete Item, Create/Delete Connector, Create/Attach/Detach Tag)

## Documentation

- [docs/index.md](docs/index.md) — overview and every component by tab
- [docs/quickstart.md](docs/quickstart.md) — first sticky note in six steps
- [docs/workflows.md](docs/workflows.md) — common wirings as component chains
- [docs/demo-workflows.md](docs/demo-workflows.md) — an eight-workflow demo canvas plan
- [docs/authentication.md](docs/authentication.md) — tokens and scopes
- [docs/changelog.md](docs/changelog.md)

## Development

```
dotnet build MiroConnect.csproj                          # net48, net7.0-windows, net7.0
dotnet test  MiroConnect.Tests/MiroConnect.Tests.csproj   # builder logic, no Rhino needed
```

This plugin was generated with the **PleaseREST** Claude Code plugin (`please-rest` v1.0.0); `CLAUDE.md` holds the build state and design notes.
<!-- pr:end requirements -->

<!-- pr:begin license -->
## License

MiroConnect is released under the MIT License — Copyright (c) 2026 Eesha Jain. See [LICENSE](LICENSE).
<!-- pr:end license -->
