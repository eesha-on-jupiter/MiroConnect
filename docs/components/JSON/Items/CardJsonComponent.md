# Card JSON

Builds Miro card request bodies from lists of titles, descriptions, due dates, positions, style and parent.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Title | Text (tree) | Yes | - | Card titles. |
| Description | Text (tree) | No | - | Card descriptions. |
| Due Date | Text (tree) | No | - | Due dates in ISO 8601, e.g. `2026-10-01T00:00:00Z`. |
| Position JSON | Text (tree) | Yes | - | Position JSONs from Position or Grid. |
| Style JSON | Text (tree) | No | - | Style JSON from [Card Style](../Style/CardStyleComponent.md). |
| Parent JSON | Text (tree) | No | - | Parent JSONs from [Parent](../Geometry/ParentComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item JSON | Text (list) | Card request bodies for Create Item. |

## Output

Builds `{"data":{"title":"...","description":"...","dueDate":"..."},"position":{"x":0,"y":0,"origin":"center"},"geometry":{"width":200,"height":200},"style":{"cardTheme":"#2d9bf0"},"parent":{"id":"..."}}`. No API call. Count is the longer of `Title` and `Position JSON`.

## Notes

- Empty fields are left out of `data`.
- Full geometry is sent: width, height, and rotation when it is non-zero.
- Send with [Create Item](../../HTTP/Items/CreateItemComponent.md) and `Item Type` = `card`.

## Related

- [Card Style](../Style/CardStyleComponent.md) and [Card Color](../../Presets/CardColorPreset.md)
- [Table Reader](../../Data/TableReader.md): titles, descriptions and dates from a spreadsheet.
