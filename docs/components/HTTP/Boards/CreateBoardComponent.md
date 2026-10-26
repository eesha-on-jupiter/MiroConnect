# Create Board

Creates a new Miro board in a team.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board Name | Text | Yes | - | Name for the new board. |
| Team ID | Text | Yes | - | ID of the team to create the board in. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Board ID | Text | ID of the newly created board. |
| Response | Text | Raw JSON response from the Miro API. |

## API endpoint

`POST /v2/boards` with body `{"name": "<Board Name>", "teamId": "<Team ID>"}`

## Notes

- Button-triggered (**Create**): only fires on a click, never on recompute.
- Each click creates another board; there is no check for an existing board with the same name.
- Needs the `boards:write` scope.

## Related

- [Delete Board](DeleteBoardComponent.md): remove a board.
- [Find Board](FindBoardComponent.md): find an existing board instead of creating one.
