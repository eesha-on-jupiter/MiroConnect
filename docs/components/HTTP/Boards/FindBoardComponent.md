# Find Board

Lists every Miro board the token can access, and optionally returns the ID of the board with a given name.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board Name | Text | No | - | Name of the board to find. When set, `Board ID` returns the matched ID. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Board IDs | Text (list) | IDs of all accessible boards. |
| Board Names | Text (list) | Names of all accessible boards, in the same order. |
| Board ID | Text | ID of the first board whose name matches `Board Name`. |

## API endpoint

`GET /v2/boards?limit=50&offset={offset}`

## Notes

- Pages through every board 50 at a time until the reported `total` is reached.
- Name matching is exact but case-insensitive; the first match wins.
- No match gives a warning and leaves `Board ID` empty.
- Read-only, runs on every recompute.

## Related

- [Get Board](GetBoardComponent.md): details for the matched board.
- [Miro Auth](../../Auth/MiroAuthComponent.md): check the token first.
