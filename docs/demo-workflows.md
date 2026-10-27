# MiroConnect Demo Workflows

Eight workflows for `MiroConnect_Demo.gh`. Each one is a separate GH group on the
canvas, stacked top to bottom in the order below. Every workflow writes into its own
region of the Miro board, so all of them can be fired in one session without overlapping.

Port names below use the **nicknames** shown on the component, e.g. `Sticky Note JSON.G`.

---

## Shared header (top of canvas, feeds every group)

```
Panel "Token"  ──> Miro Auth.T
                   Miro Auth.S  ──> Panel "Status"   (reads "Connected")
Panel "Token"  ──> [TOKEN] text param ──> the T input of every HTTP component
Find Board.ID  ──> [BOARD] text param ──> the Board input of every HTTP component
```

Two **Param > Primitive > Text** params named `TOKEN` and `BOARD` are the single source
for the whole file. Every group downstream wires from these two, so a demo only needs
the token pasted once.

> The token panel is the one thing the user edits. Leave it empty in the shipped file
> and put a scribble above it: *paste a Miro personal access token here*.

---

## W1 · Connect and inspect

**Teaches:** auth, board discovery, read-only calls. No writes, safe to run on load.

```
TOKEN ──> Miro Auth.T ──> Miro Auth.S ──> Panel   (Connected / error message)

TOKEN ──> Find Board.T
          Find Board.IDs   ──> Panel
          Find Board.Names ──> Value List (pick a board by name)
          Panel "board name" ──> Find Board.Name ──> Find Board.ID ──> BOARD

TOKEN, BOARD ──> Get Board.T / .ID
          Get Board.N / .Team / .Date ──> three Panels

TOKEN, BOARD ──> Get All Items.T / .Board
          Panel "" (empty = all types) ──> Get All Items.Type
          Get All Items.IDs ──> Panel  +  List Length ──> Panel "items on board"

Get All Items.IDs ──> List Item (i=0) ──> Get Item.ID
TOKEN, BOARD ──> Get Item.T / .Board
          Get Item.Type / .X / .Y / .W / .H ──> Panels
```

**Try it:** paste a token, read the board list, pick one, watch the item count.

---

## W2 · Hello board: one sticky note

**Teaches:** the JSON → HTTP split and the button trigger. Smallest possible write.
**Board region:** `(0, 0)`

```
Slider X (-1000..1000, =0)  ──> Position.X
Slider Y (-1000..1000, =0)  ──> Position.Y
Slider W (100..600,  =300)  ──> Position.W
Slider H (100..600,  =200)  ──> Position.H
                                Position.G ────────────┐
Sticky Note Color preset ──> Sticky Note Style.Fill    │
Panel "center"           ──> Sticky Note Style.Align   │
Slider Size (10..40, =18)──> Sticky Note Style.Size    │
                             Sticky Note Style.S ─────┐│
Panel "Hello from Grasshopper" ──> Sticky Note JSON.C  ││
                                   Sticky Note JSON.G <─┘
                                   Sticky Note JSON.S <──┘
                                   Sticky Note JSON.J ──┐
TOKEN ──> Create Item.T                                 │
BOARD ──> Create Item.Board                             │
Panel "sticky_note" ──> Create Item.Type                │
                        Create Item.J  <────────────────┘
                        Create Item.IDs ──> Panel
                        Create Item.R   ──> Panel
```

**Try it:** click the button on **Create Item**. Nothing fires until the button is
pressed, so moving the sliders is free. Move a slider, press again, a second note appears.

---

## W3 · Spreadsheet to sticky grid ★ flagship

**Teaches:** Table Reader, Grid, bulk create in one call.
**Board region:** origin `(-2000, -1200)`

```
File Path panel  ──> Table Reader.P      (ships with demo-data/tasks.csv)
Slider Sheet (=0)──> Table Reader.Sh
Toggle true      ──> Table Reader.H
                     Table Reader.Hd ──> Panel   (column headers)
                     Table Reader.D  ──> Tree Branch {0} ──> contents
                     Table Reader.RC ──> Grid.N
                     Table Reader.CC ──> Panel

Grid.R  = 5      (items per row)
Grid.W  = 220    Grid.H  = 160
Grid.GX = 40     Grid.GY = 40
Grid.X  = -2000  Grid.Y  = -1200
Grid.G ──> Sticky Note JSON.G

Tree Branch {0} of Table Reader.D ──> Sticky Note JSON.C
Sticky Note Color preset ──> Sticky Note Style.Fill ──> Sticky Note JSON.S
                                                        Sticky Note JSON.J ──> Create Item.J
TOKEN / BOARD / Panel "sticky_note" ──> Create Item.T / .Board / .Type
Create Item.IDs ──> [W3 IDS] text param      (reused by W5 and W8)
```

**Notes**

- `Table Reader.D` is a tree with one branch per column. Use **Tree Branch** with path
  `{0}` for column 0, `{1}` for column 1, and so on.
- Grid emits one Position JSON per cell, left to right, wrapping at `Items Per Row`.
- The JSON components match by longest list and repeat the last value, so a single
  style object feeds every note, with no grafting, no list repeating needed.
- **Create Item** flattens the whole JSON tree into one list and posts the items one by
  one, so a 40-row CSV is a single button press.

**Try it:** swap the CSV for any file and press once. Change `Grid.R` to reflow.

---

## W4 · Framed flow diagram with connectors

**Teaches:** parenting into frames, staged writes, connector pairing.
**Board region:** origin `(0, -1400)`

Stage 1: create the frame:

```
Position (X=0, Y=-1400, W=1400, H=900) ──> Frame JSON.G
Frame Style (.Fill #ffffff, .Border #1a1a1a) ──> Frame JSON.S
Panel "Process Flow" ──> Frame JSON.T ──> Frame JSON.J ──> Create Item.J
TOKEN / BOARD / Panel "frame" ──> Create Item.T / .Board / .Type
Create Item.IDs ──> [FRAME ID] text param
```

Stage 2: shapes inside the frame:

```
[FRAME ID] ──> Parent.F ──> Parent.P ──> Shape JSON.P
Grid (N=5, R=5, W=200, H=120, GX=80, X=-480, Y=-1400) ──> Shape JSON.G
Shape Type preset (Round Rectangle) ──> Shape JSON.T
Panel, 5 lines: Intake / Model / Analyse / Review / Issue ──> Shape JSON.C
Shape Style (.Fill #d0e8ff, .Stroke #1a1a1a, .Width 2, .Size 16) ──> Shape JSON.S
Shape JSON.J ──> Create Item.J ──> Create Item.IDs ──> [SHAPE IDS] text param
TOKEN / BOARD / Panel "shape" ──> Create Item.T / .Board / .Type
```

Stage 3: chain them with connectors:

```
[SHAPE IDS] ──> Sub List (0 .. N-2) ──> Connector JSON.Start
[SHAPE IDS] ──> Sub List (1 .. N-1) ──> Connector JSON.End
Panel "elbowed" (or Stroke Style preset for a dash) ──> Connector Style.Style
Connector Style (.Color #1a1a1a, .Width 2) ──> Connector JSON.S
Connector JSON.J ──> Create Connector.J
TOKEN / BOARD ──> Create Connector.T / .Board
Create Connector.IDs ──> [CONNECTOR IDS] text param ──> Panel

verify:
[CONNECTOR IDS] ──> List Item (0) ──> Get Connector.ID
TOKEN / BOARD ──> Get Connector.T / .Board
Get Connector.Start / .End / .R ──> Panels
```

**Notes**

- Three button presses in order: frame, then shapes, then connectors. Each stage needs
  the IDs the previous one returned, which is exactly why writes are button-gated.
- Connectors carry no position; they only reference item IDs.
- The two **Sub List** components are the chain trick: item *i* joins item *i+1*.

**Try it:** add a sixth line to the content panel, bump `Grid.N` to 6, re-press all three.

---

## W5 · Tag and verify

**Teaches:** tags are board-level objects that get attached to items afterwards.
**Board region:** reuses the W3 stickies.

```
Panel, 2 lines: Priority / Blocked   ──> Tag JSON.T
Panel, 2 lines: red / yellow        ──> Tag JSON.Fill
Tag JSON.J ──> Create Tag.J
TOKEN / BOARD ──> Create Tag.T / .Board
Create Tag.IDs ──> [TAG IDS] text param

[TAG IDS] ──> List Item (0)        ──> Attach Tag.Tag
[W3 IDS]  ──> Sub List (0 .. 2)    ──> Attach Tag.Items
TOKEN / BOARD ──> Attach Tag.T / .Board
Attach Tag.R ──> Panel

verify:
[W3 IDS] ──> List Item (0) ──> Get Item Tags.Item
TOKEN / BOARD ──> Get Item Tags.T / .Board
Get Item Tags.IDs / .Titles ──> Panels

undo:
same Items + Tag ──> Detach Tag ──> Detach Tag.R ──> Panel
```

**Try it:** attach, read the titles back, detach, read again, and the list empties.

---

## W6 · Kanban card wall

**Teaches:** Offset for column layout, cards, text headers, the remaining presets.
**Board region:** origin `(2200, -1200)`

```
Position (X=2200, Y=-1400, W=300, H=60) ──> [COL BASE] position JSON

column headers:
  [COL BASE] ──> Offset.G,  Offset.X = 0 / 400 / 800   (three Offset components)
  the three Offset.G ──> Merge ──> Text JSON.G
  Panel, 3 lines: To Do / In Progress / Done ──> Text JSON.C
  Font Family preset ──> Text Style.Font
  Text Style (.Fill transparent, .Size 24, .Align center) ──> Text JSON.S
  Text JSON.J ──> Create Item.J        (Type = "text")

cards:
  Grid (N=9, R=3, W=300, H=180, GX=100, GY=40, X=2200, Y=-1200) ──> Card JSON.G
  Table Reader.D branch {0} ──> Card JSON.T      (title)
  Table Reader.D branch {1} ──> Card JSON.D      (description)
  Table Reader.D branch {2} ──> Card JSON.Due    (ISO 8601 date strings)
  Card Color preset ──> Card Style.Color ──> Card JSON.S
  Card JSON.J ──> Create Item.J        (Type = "card")

TOKEN / BOARD ──> both Create Item components
```

**Notes**

- Grid fills left to right, so with `R=3` the three columns line up under the headers.
- Due dates must be ISO 8601 (`2026-10-01T00:00:00Z`); the demo CSV already is.
- Offset shifts an existing Position JSON, cheaper than three Position components when
  one size is shared across columns.

**Try it:** change the Card Color preset and press again. Miro replaces nothing, so a
second wall appears; use W8 to clear it.

---

## W7 · Images into a frame

**Teaches:** the three image sources and parenting.
**Board region:** origin `(2200, 600)`

```
Frame JSON "Reference" at (2200, 600, 1200, 800) ──> Create Item   (Type "frame")
Create Item.IDs ──> Parent.F ──> Parent.P ──> Image JSON.P

pick one source (priority is Bitmap > File Path > URL):
  Panel of public image URLs  ──> Image JSON.URL
  File Path params            ──> Image JSON.F     (demo-data/*.png)
  any bitmap-producing comp   ──> Image JSON.B

Grid (N=4, R=2, W=500, H=340, X=1950, Y=400) ──> Image JSON.G
Image JSON.J ──> Create Item.J        (Type = "image")
TOKEN / BOARD ──> Create Item.T / .Board
```

**Notes**

- File paths and bitmaps upload as multipart binary; URLs are posted as JSON. Create
  Item detects which and picks the right call, so the wiring is identical either way.
- Two button presses: frame first, images second.

---

## W8 · Teardown

**Teaches:** cleanup, type filters, and how to reset the demo board between showings.
Put this group at the bottom in a red GH group named **DANGER: deletes board content**.

```
TOKEN / BOARD ──> Get All Items.T / .Board
Value List: sticky_note / shape / card / text / frame / image / "" ──> Get All Items.Type
Get All Items.IDs ──> Panel + List Length
Get All Items.IDs ──> Delete Item.IDs
TOKEN / BOARD ──> Delete Item.T / .Board
Delete Item.R ──> Panel

[CONNECTOR IDS] ──> Delete Connector.IDs
TOKEN / BOARD   ──> Delete Connector.T / .Board

full reset, optional:
TOKEN ──> Create Board.T
Panel "MiroConnect Demo" ──> Create Board.Name
Panel team id ──> Create Board.Team ──> Create Board.ID ──> BOARD
TOKEN / BOARD ──> Delete Board
```

**Notes**

- Delete connectors before the items they join, or the calls error out on connectors
  whose endpoints are already gone.
- Leave **Delete Board** on the canvas but wire its `ID` from a separate panel, not from
  `BOARD`, so a stray click cannot take out the demo board.

---

## Canvas layout

```
┌── Shared header ───────── TOKEN · BOARD · Miro Auth ──────────────┐
├── W1  Connect and inspect                          read only      │
├── W2  Hello board                                  1 button       │
├── W3  Spreadsheet to sticky grid           ★       1 button       │
├── W4  Framed flow diagram                          3 buttons      │
├── W5  Tag and verify                                3 buttons     │
├── W6  Kanban card wall                             2 buttons      │
├── W7  Images into a frame                          2 buttons      │
└── W8  Teardown                              red    2-4 buttons    │
```

## Demo data to ship beside the .gh

`demo-data/tasks.csv`: 9 rows, 3 columns, feeds W3 and W6:

```csv
Task,Owner,Due
Site survey,A. Rivera,2026-10-01T00:00:00Z
Massing options,J. Park,2026-10-08T00:00:00Z
Structural grid,M. Osei,2026-10-15T00:00:00Z
```

`demo-data/*.png`: two or three small images for W7.

## Coverage

Every shipped component appears at least once:
Auth W1 · Boards W1/W8 · Items W2-W7 · Connectors W4/W8 · Tags W5 ·
Position W2/W7 · Grid W3/W4/W6/W7 · Offset W6 · Parent W4/W7 ·
all six Style components · all six Item JSON components · Tag JSON W5 ·
Connector JSON W4 · Table Reader W3/W6 · all five presets.
