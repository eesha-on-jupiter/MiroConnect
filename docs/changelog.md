# Changelog

## 1.0.0 — 2026-10-26

Initial release.

- **Auth tab:** Miro Auth validates a token against the Miro REST API v2.
- **Data tab:** Table Reader reads a CSV or .xlsx into headers and a column-per-branch tree.
- **Boards tab:** find boards by name (paginated), read board details, create and delete boards (button-triggered).
- **Items tab:** read one item, list item IDs with a type filter, bulk create items of one type per click (JSON or multipart image upload), bulk delete items.
- **Connectors tab:** read a connector's ends, bulk create and delete connectors.
- **Tags tab:** bulk create tags, attach or detach one tag across many items, read an item's tags.
- **Geometry tab:** Position, Grid, Offset and Parent builders for center-based layout and frame parenting.
- **Items JSON tab:** request-body builders for sticky notes, shapes, text, cards, frames, images, connectors and tags, with longest-list matching.
- **Style tab:** style builders for sticky notes, shapes, text, cards, frames and connectors.
- **Presets tab:** value lists for shape types, sticky note colors, card colors, font families and connector dash patterns.
- **Packages:** Rhino 7 (Windows), Rhino 8 Windows and Rhino 8 Mac, as `.yak` packages and Components-folder zips.

### Known limitations

- **Rhino 8 Mac is untested.** The Mac package builds, but no component has been run in Rhino 8 for Mac. Table Reader's `.xlsx` path uses EPPlus 4.5, which relies on `System.Drawing`; on Mac that is Rhino's own implementation, so reading Excel files there may fail — CSV does not depend on it. Image JSON's Bitmap input and the button rendering also go through `System.Drawing`.
- **Request bodies are checked against Miro's reference, not the live API, for this release.** A few style fields the components send are not in Miro's published schema and may be ignored or rejected:
  - Sticky Note Style `Font Size` (Miro's sticky note style has no `fontSize`) — leave it at 0 to omit it.
  - Frame Style `Border Color` (frames have no `borderColor`) — leave it empty to omit it.
  - Frame Style `Fill Color` and Shape Style `Fill Color`: Miro documents a fixed palette of hex values; other hex values may be rejected.
- Get All Items reads only the first page of results; it does not follow Miro's pagination cursor.
- Create Item takes one item type per click; mixed types need one Create Item each.
- Each create click adds new items; nothing is updated in place.
- Connector Style's `Line Style` takes either a routing value or a dash pattern, not both on the same connector.
