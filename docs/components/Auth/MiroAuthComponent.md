# Miro Auth

Validates a Miro access token and confirms the plugin can reach the Miro API.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro personal access token or OAuth bearer token. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `Connected` on success, otherwise `Error: <message>`. |
| Base URL | Text | The Miro API base URL in use (`https://api.miro.com/v2`). |

## API endpoint

`GET /v2/boards?limit=1`

## Notes

- Read-only: lists at most one board, so it has no side effects and runs on every recompute.
- An empty Token raises a warning and the component does nothing.
- On failure the HTTP status and Miro's error body are shown in `Status` and as a runtime error.

## Related

- [Authentication](../../authentication.md): how to get a token.
- [Find Board](../HTTP/Boards/FindBoardComponent.md): the usual next step, to get a Board ID.
