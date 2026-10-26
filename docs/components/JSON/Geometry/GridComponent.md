# Grid

Generates a list of Position JSONs arranged in a grid, filling left to right, then top to bottom.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Count | Integer | No | 1 | Total number of items in the grid. |
| Items Per Row | Integer | No | 4 | Number of items per row. |
| Width | Number | No | 200 | Width of each item in pixels. |
| Height | Number | No | 200 | Height of each item in pixels. |
| Gap X | Number | No | 20 | Horizontal gap between items in pixels. |
| Gap Y | Number | No | 20 | Vertical gap between items in pixels. |
| Origin X | Number | No | 0 | Center X of the first (top-left) item. |
| Origin Y | Number | No | 0 | Center Y of the first (top-left) item. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Position JSON | Text (list) | One Position JSON per grid cell, in fill order. |

## Output

For cell `i`: column `i % Items Per Row`, row `i / Items Per Row`, center at `Origin + (column, row) * (size + gap)`. Each cell is a Position JSON with rotation `0`. No API call.

## Notes

- `Count` and `Items Per Row` are clamped to at least 1.
- `Height` and `Gap Y` are independent of `Width` and `Gap X`; set them explicitly for non-square cells.
- Wire [Table Reader](../../Data/TableReader.md) `Row Count` into `Count` for one cell per spreadsheet row.

## Related

- [Position](GeometryComponent.md): a single position.
- [Items JSON tab](../Items/index.md): consumers of the list.
