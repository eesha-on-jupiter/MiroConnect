# Quickstart

Six components, one canvas: validate a token, pick a board, build one sticky note and push it live.

## 1. Get a token

- Follow [authentication.md](authentication.md) to get a Miro access token with `boards:read` and `boards:write`.
- Put it on a Panel so you can wire it into every HTTP component.

## 2. Verify the connection

- Drop **Auth > Miro Auth** ([page](components/Auth/MiroAuthComponent.md)).
- Wire the token Panel into `Token`; `Status` reads `Connected`.

## 3. Pick a board

- Drop **Boards > Find Board** ([page](components/HTTP/Boards/FindBoardComponent.md)) and wire the token in.
- `Board Names` lists every board the token can see; type one into a Panel wired to `Board Name`.
- `Board ID` is the ID every other HTTP component needs.
- No board yet? Use [Create Board](components/HTTP/Boards/CreateBoardComponent.md) with a Team ID instead.

## 4. Build the item

- **Geometry > Position** ([page](components/JSON/Geometry/GeometryComponent.md)): `X` = 0, `Y` = 0, `Width` = 300. Origin is the board center; Y grows downward.
- **Style > Sticky Note Style** ([page](components/JSON/Style/StickyNoteStyleComponent.md)) with a **Presets > Sticky Note Color** wired into `Fill Color`.
- **Items JSON > Sticky Note JSON** ([page](components/JSON/Items/StickyNoteJsonComponent.md)): a Panel `Hello from Grasshopper` into `Content`, Position's `Position JSON` into `Position JSON`, the style's `Style JSON` into `Style JSON`.

Nothing has been sent yet — JSON components never touch the network.

## 5. Push it

- Drop **Items > Create Item** ([page](components/HTTP/Items/CreateItemComponent.md)).
- Wire `Token`, `Board ID`, a Panel `sticky_note` into `Item Type`, and Sticky Note JSON's `Item JSON` into `Item JSON`.
- Click the **Create** button on the component. `Item IDs` returns the new note's ID.

Moving a slider does not resend anything: writes only fire on a click, and each click creates new items.

## 6. Scale it up

- Swap **Position** for **Grid** and feed a list into `Content` — one click creates every note. A spreadsheet works too, via [Table Reader](components/Data/TableReader.md).
- Clean up with [Get All Items](components/HTTP/Items/GetAllItemsComponent.md) → [Delete Item](components/HTTP/Items/DeleteItemComponent.md).

## Next

- [workflows.md](workflows.md) — common wirings as component chains.
- [demo-workflows.md](demo-workflows.md) — a full eight-workflow demo canvas plan.
- [index.md](index.md) — every component by tab.
