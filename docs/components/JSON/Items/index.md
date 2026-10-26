# Items JSON

One request-body builder per Miro item type, plus connectors and tags. Each emits a list of JSON strings for the matching create component in the HTTP tabs. No network calls.

| Component | Description |
|---|---|
| [Sticky Note JSON](StickyNoteJsonComponent.md) | Sticky note bodies from content, positions, style and parent. |
| [Shape JSON](ShapeJsonComponent.md) | Shape bodies from shape type, content, positions, style and parent. |
| [Text JSON](TextJsonComponent.md) | Text item bodies from content, positions, style and parent. |
| [Card JSON](CardJsonComponent.md) | Card bodies from title, description, due date, positions, style and parent. |
| [Frame JSON](FrameJsonComponent.md) | Frame bodies from title, positions and style. |
| [Image JSON](ImageJsonComponent.md) | Image bodies from URLs, local files or Bitmaps. |
| [Connector JSON](ConnectorJsonComponent.md) | Connector bodies from start and end item IDs and style. |
| [Tag JSON](TagJsonComponent.md) | Tag bodies from titles and fill colors. |

## List matching

Every builder flattens each input tree into one list and makes as many JSONs as its longest driving input (listed on each page). Shorter inputs repeat their last value, so a single Style JSON or Parent JSON applies to the whole batch with no grafting.
