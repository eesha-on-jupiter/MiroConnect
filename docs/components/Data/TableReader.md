# Table Reader

Reads a CSV or Excel (.xlsx) file and outputs its headers and cells as a column-per-branch tree.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| File Path | Text | No | "" | Full path to a .csv or .xlsx file. |
| Sheet | Integer | No | 0 | Sheet index (0-based) for Excel files. Ignored for CSV. |
| Has Headers | Boolean | No | true | When true, the first row is treated as column headers. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Headers | Text (list) | Column header strings in order. |
| Data | Text (tree) | Cell values. Branch `{i}` holds every value for column `i`. |
| Row Count | Integer | Number of data rows (excluding the header row if present). |
| Col Count | Integer | Number of columns. |

## Output

- No API call: reads the file locally.
- `.csv` is parsed per RFC 4180 (quoted fields, escaped `""`); multi-line quoted fields are not supported.
- `.xlsx` / `.xls` is read with EPPlus using each cell's displayed text.
- Short rows are padded with empty strings so every column branch has `Row Count` items.
- With `Has Headers` false, headers are generated as `Column 1`, `Column 2`, and so on.

## Notes

- Use a **Tree Branch** component with path `{0}`, `{1}`, ... to pull one column out of `Data`.
- `Row Count` wires straight into [Grid](../JSON/Geometry/GridComponent.md) `Count` to lay out one item per row.
- An empty path gives a warning; a missing file, an unsupported extension or an out-of-range sheet gives an error.

## Related

- [Grid](../JSON/Geometry/GridComponent.md): one position per row.
- [Sticky Note JSON](../JSON/Items/StickyNoteJsonComponent.md) and [Card JSON](../JSON/Items/CardJsonComponent.md): typical consumers of a column.
