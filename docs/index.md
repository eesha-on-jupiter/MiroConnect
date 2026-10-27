# MiroConnect

MiroConnect is a Grasshopper plugin for Rhino 7/8 (Windows and Mac) that wraps the [Miro REST API](https://developers.miro.com/reference) to build Miro boards.
It builds sticky notes, shapes, text, cards, frames, images, connectors and tags as JSON in dedicated builder components, lays them out with position and grid helpers, and pushes them to a live board with a button click. It can also read boards and items back, turn a CSV or Excel sheet into a wall of notes or cards, and clear a board between runs.

- **Platform:** [Miro REST API](https://developers.miro.com/reference) v2
- **Auth:** Miro access token required, see [Authentication](authentication.md)
- **Version:** 1.0.0 (pre-release)
- **Rhino support:** Rhino 7 (.NET Framework 4.8), Rhino 8 Windows (.NET 7), Rhino 8 Mac (.NET 7)
- **License:** MIT
- **GitHub:** https://github.com/eesha-on-jupiter/MiroConnect
- **Food4Rhino:** _(added once listed on food4rhino.com)_
- **Released:** _(added at first tagged release)_
- **Made with:** [PleaseREST](https://github.com/eesha-on-jupiter/PleaseREST)

_(Space for a demo video or GIF; drop it above or below this list once one exists.)_

## Features

- **Item builders:** one JSON component per item type ([Items JSON tab](components/JSON/Items/index.md)): sticky note, shape, text, card, frame, image, plus connector and tag bodies.
- **Style builders:** one style component per item type ([Style tab](components/JSON/Style/index.md)), reused across a whole batch.
- **Layout helpers:** [Position](components/JSON/Geometry/GeometryComponent.md), [Grid](components/JSON/Geometry/GridComponent.md), [Offset](components/JSON/Geometry/OffsetComponent.md) and [Parent](components/JSON/Geometry/ParentComponent.md) place items by center point, in rows, or inside frames.
- **Bulk create:** [Create Item](components/HTTP/Items/CreateItemComponent.md) flattens any JSON tree and posts every item in one click, returning IDs in input order.
- **Images from anywhere:** [Image JSON](components/JSON/Items/ImageJsonComponent.md) takes public URLs, local files or Bitmaps; file and Bitmap images are uploaded as multipart binary.
- **Spreadsheet input:** [Table Reader](components/Data/TableReader.md) turns a CSV or .xlsx into one branch per column.
- **Read back:** find boards by name, list and inspect items, read connectors and item tags.
- **Cleanup:** delete items, connectors, tags from items, or whole boards.
- **Presets:** value lists for shape types, sticky note colors, card colors, font families and connector dash patterns ([Presets tab](components/Presets/index.md)).

## Install

Install via the Rhino Package Manager (`PackageManager` command in Rhino) and search for **miroconnect**, or unzip the release `.zip` for your platform (`rh7-win`, `rh8-win`, `rh8-mac`) into Grasshopper's Components Folder and restart Rhino. See [quickstart.md](quickstart.md) for the first board, and [authentication.md](authentication.md) for how to get a token before your first live call.

## Components by tab

| Tab | Type | Components | What it's for |
|---|---|---|---|
| [Auth](components/Auth/index.md) | HTTP | 1 | Validate a token |
| [Data](components/Data/index.md) | Local | 1 | Read a CSV or .xlsx into a column tree |
| [Boards](components/HTTP/Boards/index.md) | HTTP | 4 | Find, read, create and delete boards |
| [Items](components/HTTP/Items/index.md) | HTTP | 4 | Create, read, list and delete board items |
| [Connectors](components/HTTP/Connectors/index.md) | HTTP | 3 | Create, read and delete connectors |
| [Tags](components/HTTP/Tags/index.md) | HTTP | 4 | Create tags, attach and detach them, read an item's tags |
| [Geometry](components/JSON/Geometry/index.md) | JSON | 4 | Positions, grids, offsets and frame parents |
| [Items JSON](components/JSON/Items/index.md) | JSON | 8 | Request bodies per item type, connector and tag |
| [Style](components/JSON/Style/index.md) | JSON | 6 | Style objects per item type |
| [Presets](components/Presets/index.md) | Preset | 5 | Value lists for Miro's enum-like fields |

40 components total.

## How writes are triggered

- Every component that changes a board (Create/Delete Board, Create/Delete Item, Create/Delete Connector, Create/Attach/Detach Tag) has a button drawn on it and only fires on a click, never on recompute.
- Read components (Miro Auth, Find Board, Get Board, Get Item, Get All Items, Get Connector, Get Item Tags) call the API on every recompute.
- JSON, Geometry, Style, Data and Preset components make no network calls and are safe to leave wired up.
- Write components take tree inputs, so each click runs exactly once whatever the upstream data structure.

## Where to go next

- New to the plugin? Start with [quickstart.md](quickstart.md).
- Looking for a wiring pattern? [workflows.md](workflows.md) draws the common ones as component chains.
- Want a full demo canvas plan? See [demo-workflows.md](demo-workflows.md).
- Need a token? See [authentication.md](authentication.md).
- What changed release to release: [changelog.md](changelog.md).
