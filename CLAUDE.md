<!-- pr:begin header -->
# MiroConnect — CLAUDE.md

Grasshopper plugin for the Miro REST API v2, built with the **PleaseREST** pipeline (`please-rest` plugin, v1.0.0).

## Skills

Always load these skills before writing any code:
- conventions
- component

Load these skills when the file type matches:
- button — when writing any component that has a write/create/delete operation
- preset — when writing any GH_ValueList component
- builders — when writing any Builders.cs file
- client — when writing [PluginName]Client.cs
- assembly — when writing [PluginName]Info.cs

The project-local copies live in `.claude/skills/` (gitignored); the `please-rest` plugin ships the same skills as `pr-*`.

- **Build:** `dotnet build MiroConnect.csproj` (net48 / net7.0-windows / net7.0). Grasshopper loads `bin/Debug/<tfm>/MiroConnect.gha` via its developer-folder setting — **close Rhino before building** (it locks the file) and restart it to pick up a build. If Rhino holds `bin/Debug`, build `-c Release`.
- **Tests:** `dotnet test MiroConnect.Tests/MiroConnect.Tests.csproj` — pure builder logic, net8.0, no Rhino needed.
- **Package:** run `"C:\Program Files\Rhino 8\System\Yak.exe" build` in each `bin/Release/<tfm>` (`--platform win` / `--platform mac` for the two Rhino 8 targets); CI does this on a `v*.*.*` tag (`.github/workflows/publish.yml`).
<!-- pr:end header -->

<!-- pr:begin state -->
## Current build state

- **Version:** 1.0.0 — pre-release; the first tag will be `v1.0.0`
- **Phase:** 15 — Package + Ship. 40 components, builder unit tests, `docs/`, `.yak` packages, Components-folder zips and CI workflows (`build.yml` on PR, `publish.yml` on tag) are complete. Repo: https://github.com/eesha-on-jupiter/MiroConnect (not yet pushed).
- **Next step:** push `main` and tag `v1.0.0` for the GitHub Release; optional `yak login` + `yak push` for `_PackageManager`; Food4Rhino listing; live re-test of the style fields listed under Known limitations; Mac test.
- **Last active:** 2026-10-26
<!-- pr:end state -->

<!-- pr:begin platform -->
## Platform context

- **API:** Miro REST API v2 — `https://api.miro.com/v2` — reference: https://developers.miro.com/reference
- **Auth:** Bearer token (access token from an installed Miro app, sent as `Authorization: Bearer {token}`); the plugin runs no OAuth flow. Scopes: `boards:read`, `boards:write`.
- **Coordinate system:** infinite canvas, origin `(0, 0)` at the board center, `x` right, `y` down. Item position is the item **center** (`"origin":"center"`). Items have `x`, `y`, `width`, `height`, optional `rotation` (degrees). `parent.id` (a frame ID) places an item inside a frame. Connectors have no position — they reference `startItem` / `endItem` by ID, and routing is the top-level `shape` field.
- **Resource hierarchy:**

```
Boards                         ← top-level object; all operations require a Board ID
  └── Items                    ← sticky note, shape, text, card, frame, image
  └── Connectors               ← lines between items; reference start + end item IDs
  └── Tags                     ← labels created on the board, then attached to items
```
<!-- pr:end platform -->

<!-- pr:begin structure -->
## Plugin structure

Tab `MiroConnect`, 40 components. HTTP components (Auth, Boards, Items, Connectors, Tags) call the API; the 9 write operations extend `ButtonComponent` and fire only on a click. JSON components (Geometry, Items JSON, Style), Table Reader (Data) and the Presets make no network calls.

**Auth** (1)

- `MiroAuthComponent` — **Miro Auth** (`MiroAuth`): Token → Status, Base URL

**Data** (1)

- `TableReader` — **Table Reader** (`TblRd`): File Path, Sheet, Has Headers → Headers, Data, Row Count, Col Count

**Boards** (4)

- `CreateBoardComponent` — **Create Board** (`CreateBoard`) [button]: Token, Board Name, Team ID → Board ID, Response
- `DeleteBoardComponent` — **Delete Board** (`DeleteBoard`) [button]: Token, Board ID → Response
- `FindBoardComponent` — **Find Board** (`FindBoard`): Token, Board Name → Board IDs, Board Names, Board ID
- `GetBoardComponent` — **Get Board** (`GetBoard`): Token, Board ID → Name, Team, Created At

**Items** (4)

- `CreateItemComponent` — **Create Item** (`CreateItem`) [button]: Token, Board ID, Item Type, Item JSON → Item IDs, Responses
- `DeleteItemComponent` — **Delete Item** (`DeleteItem`) [button]: Token, Board ID, Item IDs → Responses
- `GetAllItemsComponent` — **Get All Items** (`GetAllItems`): Token, Board ID, Type Filter → Item IDs
- `GetItemComponent` — **Get Item** (`GetItem`): Token, Board ID, Item ID → Type, X, Y, Width, Height, Response

**Connectors** (3)

- `CreateConnectorComponent` — **Create Connector** (`CreateConnector`) [button]: Token, Board ID, Connector JSON → Connector IDs, Responses
- `DeleteConnectorComponent` — **Delete Connector** (`DeleteConnector`) [button]: Token, Board ID, Connector IDs → Responses
- `GetConnectorComponent` — **Get Connector** (`GetConnector`): Token, Board ID, Connector ID → Start Item ID, End Item ID, Response

**Tags** (4)

- `AttachTagComponent` — **Attach Tag** (`AttachTag`) [button]: Token, Board ID, Item IDs, Tag ID → Responses
- `CreateTagComponent` — **Create Tag** (`CreateTag`) [button]: Token, Board ID, Tag JSON → Tag IDs, Responses
- `DetachTagComponent` — **Detach Tag** (`DetachTag`) [button]: Token, Board ID, Item IDs, Tag ID → Responses
- `GetItemTagsComponent` — **Get Item Tags** (`GetItemTags`): Token, Board ID, Item ID → Tag IDs, Tag Titles

**Geometry** (4)

- `GeometryComponent` — **Position** (`Position`): X, Y, Width, Height, Rotation → Position JSON
- `GridComponent` — **Grid** (`Grid`): Count, Items Per Row, Width, Height, Gap X, Gap Y, Origin X, Origin Y → Position JSON
- `OffsetComponent` — **Offset** (`Offset`): Position JSON, X Offset, Y Offset → Position JSON
- `ParentComponent` — **Parent** (`Parent`): Frame IDs → Parent JSON

**Items JSON** (8)

- `CardJsonComponent` — **Card JSON** (`CardJSON`): Title, Description, Due Date, Position JSON, Style JSON, Parent JSON → Item JSON
- `ConnectorJsonComponent` — **Connector JSON** (`ConnJSON`): Start Item ID, End Item ID, Style JSON → Connector JSON
- `FrameJsonComponent` — **Frame JSON** (`FrameJSON`): Title, Position JSON, Style JSON → Item JSON
- `ImageJsonComponent` — **Image JSON** (`ImageJSON`): Image URL, File Path, Bitmap, Position JSON, Parent JSON → Item JSON
- `ShapeJsonComponent` — **Shape JSON** (`ShapeJSON`): Shape Type, Content, Position JSON, Style JSON, Parent JSON → Item JSON
- `StickyNoteJsonComponent` — **Sticky Note JSON** (`StickyJSON`): Content, Position JSON, Style JSON, Parent JSON → Item JSON
- `TagJsonComponent` — **Tag JSON** (`TagJSON`): Title, Fill Color → Tag JSON
- `TextJsonComponent` — **Text JSON** (`TextJSON`): Content, Position JSON, Style JSON, Parent JSON → Item JSON

**Style** (6)

- `CardStyleComponent` — **Card Style** (`CardStyle`): Card Color → Style JSON
- `ConnectorStyleComponent` — **Connector Style** (`ConnStyle`): Line Style, Stroke Color, Stroke Width → Style JSON
- `FrameStyleComponent` — **Frame Style** (`FrameStyle`): Fill Color, Border Color → Style JSON
- `ShapeStyleComponent` — **Shape Style** (`ShapeStyle`): Fill Color, Stroke Color, Stroke Width, Font Size → Style JSON
- `StickyNoteStyleComponent` — **Sticky Note Style** (`StickyStyle`): Fill Color, Text Align, Font Size → Style JSON
- `TextStyleComponent` — **Text Style** (`TextStyle`): Fill Color, Font Family, Font Size, Text Align → Style JSON

**Presets** (5)

- `CardColorPreset` — **Card Color** (`CC`): — → value list
- `FontFamilyPreset` — **Font Family** (`FF`): — → value list
- `ShapeTypePreset` — **Shape Type** (`ST`): — → value list
- `StickyNoteColorPreset` — **Sticky Note Color** (`SNC`): — → value list
- `StrokeStylePreset` — **Stroke Style** (`SS`): — → value list

`Utilities/`: `MiroClient.cs` (static `HttpClient`, Bearer header, JSON and multipart requests), `ButtonComponent.cs` (button-on-canvas base class for writes).
<!-- pr:end structure -->

<!-- pr:begin builders -->
## JSON builders (`JSON Builders/`, namespace `MiroConnect.JsonBuilders`)

- `SharedBuilders.cs` — `BuildGeometry` (internal `{x,y,width,height,rotation}` blob), `BuildParent`, `ApplyGeometry` (center `position` + full `geometry`, rotation only when non-zero), `ApplyGeometryWidthOnly`, `ApplyParent`. Used by every item builder.
- `StickyNoteBuilders.cs` — `BuildStickyNoteStyle`, `BuildStickyNoteBody` (width-only geometry). Sticky Note Style / JSON.
- `ShapeBuilders.cs` — `BuildShapeStyle` (stroke → `borderColor` / `borderWidth`), `BuildShapeBody`. Shape Style / JSON.
- `TextBuilders.cs` — `BuildTextStyle`, `BuildTextBody` (width-only). Text Style / JSON.
- `CardBuilders.cs` — `BuildCardStyle` (`cardTheme`), `BuildCardBody`. Card Style / JSON.
- `FrameBuilders.cs` — `BuildFrameStyle`, `BuildFrameBody` (`freeform` / `custom`). Frame Style / JSON.
- `ImageBuilders.cs` — `BuildImageBody` (URL), `BuildImageUploadSentinel` (`_imageUpload` + base64 `_data`; Create Item turns it into a multipart POST). Image JSON.
- `ConnectorBuilders.cs` — `BuildConnectorStyle` (dash values → `strokeStyle`, routing → `lineStyle`), `BuildConnectorBody` (moves `lineStyle` to the top-level `shape`). Connector Style / JSON.
- `TagBuilders.cs` — `BuildTagBody`. Tag JSON.

Builders are pure (Newtonsoft + BCL only) so `MiroConnect.Tests` compiles them in directly. Numbers sent as strings use `CultureInfo.InvariantCulture`.
<!-- pr:end builders -->

<!-- pr:begin decisions -->
## Key design decisions

1. **JSON and HTTP are always separate** — JSON components build payloads; HTTP components send them. Builder methods are called by JSON components only, never by HTTP components.
2. **All write operations are button-triggered** — create, delete, attach and detach components extend `ButtonComponent`.
3. **No separate update component** — create handles the full push (Miro uses POST for create); each click adds new items.
4. **GET operations live in the HTTP subcategory** alongside create/delete for their resource.
5. **Auth subcategory contains only `MiroAuthComponent`** — token input, connection status output.
6. **One builder file per resource family**; shared helpers go into `SharedBuilders.cs`.
7. **Bulk by default** — Items JSON builders and write components take tree inputs, flatten them and match by longest list (shorter inputs repeat their last value), so one click runs once whatever the upstream structure and returns IDs aligned with the input (`null` for a failed entry).
8. **One item type per Create Item click** — `Item Type` picks the endpoint segment (`sticky_notes`, `shapes`, `texts`, `cards`, `frames`, `images`).
9. **Images** — URL images post JSON; file-path and Bitmap images go through an internal upload sentinel and a multipart `resource` + `metadata` POST.
10. **GUIDs and parameter order are frozen from 1.0.0** — new params are appended, new components get new GUIDs.
<!-- pr:end decisions -->

<!-- pr:begin limitations -->
## Known limitations

- Rhino 8 Mac untested; Table Reader's `.xlsx` path (EPPlus 4.5.3.3) and Bitmap / button drawing depend on `System.Drawing`, supplied by Rhino on Mac.
- Request bodies were checked against Miro's reference but not live-tested for 1.0.0 (the Miro MCP test server was down). Fields sent that are not in Miro's schema: sticky note `style.fontSize`, frame `style.borderColor`. Frame / shape `fillColor` should come from Miro's documented palette.
- Get All Items reads one page only (no cursor pagination).
- Connector Style `Line Style` carries either routing or a dash pattern, not both.
<!-- pr:end limitations -->
