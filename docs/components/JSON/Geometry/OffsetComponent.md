# Offset

Shifts a Position JSON by an X and Y offset, keeping its width, height and rotation.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Position JSON | Text | Yes | - | Position JSON from Position, Grid or another Offset. |
| X Offset | Number | No | 0 | Amount to shift along X (positive is right). |
| Y Offset | Number | No | 0 | Amount to shift along Y (positive is down). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Position JSON | Text | The shifted Position JSON. |

## Output

Returns a new Position JSON with `x + X Offset` and `y + Y Offset`. No API call. Missing fields in the input fall back to `x`/`y` 0, `width`/`height` 200, `rotation` 0.

## Notes

- Cheaper than several Position components when one size is shared, e.g. column headers on a Kanban wall.
- Item access: feed a list and it runs once per item.

## Related

- [Position](GeometryComponent.md)
- [Grid](GridComponent.md)
