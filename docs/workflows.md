# Workflows

Component chains for the things people usually build. Components only — see each component's page for its inputs. `─▶` is a wire; `(click)` is a button-triggered write. Every HTTP component also takes the `Token`, and most take the `Board ID` from [Find Board](components/HTTP/Boards/FindBoardComponent.md); those wires are left out below.

The full canvas plan behind these, with port-by-port wiring and board regions, is [demo-workflows.md](demo-workflows.md).

What you can do with MiroConnect:

- Check a Miro token works, pick a board by name and see what is on it — [1](#1-connect-and-inspect)
- Put a styled sticky note on a board from Grasshopper with one click — [2](#2-hello-board)
- Turn a spreadsheet into a grid of sticky notes in one push — [3](#3-spreadsheet-to-sticky-grid)
- Draw a framed flow diagram of shapes joined by connectors — [4](#4-framed-flow-diagram)
- Create tags, attach them to items and read them back — [5](#5-tag-and-verify)
- Build a Kanban wall of cards with column headers from a spreadsheet — [6](#6-kanban-card-wall)
- Place images from URLs, local files or Grasshopper bitmaps inside a frame — [7](#7-images-into-a-frame)
- Clear items and connectors off a board, or reset it entirely, between runs — [8](#8-teardown)

---

## 1. Connect and inspect

Read-only; runs on every recompute.

```
Miro Auth (Status = Connected)

Find Board ──Board ID─▶ Get Board
           └─Board ID─▶ Get All Items ──Item IDs─▶ List Item ─▶ Get Item
```

---

## 2. Hello board

```
Position ─────────────┐
Sticky Note Color ─▶ Sticky Note Style ─┐
Panel "Hello" ────────┴─▶ Sticky Note JSON ◀┘ ──Item JSON─▶ Create Item (Type "sticky_note", click)
```

---

## 3. Spreadsheet to sticky grid

```
Table Reader ──Row Count─▶ Grid ──Position JSON─────────┐
             └─Data─▶ Tree Branch {0} ──Content─────────┼─▶ Sticky Note JSON ─▶ Create Item (click)
Sticky Note Style ──Style JSON──────────────────────────┘
```

One Style JSON feeds every note: builders repeat the last value of shorter inputs.

---

## 4. Framed flow diagram

Three clicks, in order — each stage needs the IDs the previous one returned.

```
Position ─▶ Frame JSON ─▶ Create Item (Type "frame", click) ──Item IDs─▶ Parent

Grid ─┐  Shape Type ─┐  Parent ─┐
      └──────────────┴──────────┴─▶ Shape JSON ─▶ Create Item (Type "shape", click) ──Item IDs─┐
                                                                                             │
Sub List 0..N-2 ──Start Item ID─┐                                                            │
Sub List 1..N-1 ──End Item ID───┴─▶ Connector JSON ◀── Connector Style    ◀──────────────────┘
                                    └─▶ Create Connector (click) ─▶ Get Connector (verify)
```

---

## 5. Tag and verify

```
Tag JSON (Title, Fill Color "red") ─▶ Create Tag (click) ──Tag IDs─▶ List Item ─┐
Create Item.Item IDs ──────────────────────────────────────────────────────────┴─▶ Attach Tag (click)
                                                                     Get Item Tags (verify) / Detach Tag (undo)
```

Tag colors are names (`red`, `blue`, …), not hex.

---

## 6. Kanban card wall

```
Position ─▶ Offset ×3 ─▶ Merge ─▶ Text JSON ◀─ Text Style ◀─ Font Family
                                  └─▶ Create Item (Type "text", click)

Table Reader ──Data {0} / {1} / {2}─▶ Card JSON (Title / Description / Due Date)
Grid ─▶ Card JSON ◀─ Card Style ◀─ Card Color
        └─▶ Create Item (Type "card", click)
```

Due dates are ISO 8601 (`2026-10-01T00:00:00Z`).

---

## 7. Images into a frame

```
Frame JSON ─▶ Create Item (Type "frame", click) ──Item IDs─▶ Parent ─┐
Image URL / File Path / Bitmap ─▶ Image JSON ◀─ Grid ◀───────────────┘
                                  └─▶ Create Item (Type "image", click)
```

File paths and bitmaps are uploaded as multipart binary; URLs are posted as JSON. The wiring is the same.

---

## 8. Teardown

```
Get All Items (Type Filter optional) ──Item IDs─▶ Delete Item (click)
Create Connector.Connector IDs ───────────────▶ Delete Connector (click)   ← run this one first

full reset:  Create Board (click) … Delete Board (click, Board ID from its own Panel)
```
