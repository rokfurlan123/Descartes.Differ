# Descartes.Differ

A small ASP.NET Core service that accepts base64-encoded binary inputs and returns diff metadata.

## Requirements

- .NET 10 SDK (adjust the target framework if needed).
- From WSL, you can invoke Windows .NET using `dotnet.exe`.

## Run

```bash
dotnet run --project src/Descartes.Api
```

Swagger UI is available at `/swagger`.

## Usage

- `PUT /v1/diff/{id}/left` with `{ "data": "<base64>" }`
- `PUT /v1/diff/{id}/right` with `{ "data": "<base64>" }`
- `GET /v1/diff/{id}`

### Sample

```bash
curl -X PUT http://localhost:5000/v1/diff/1/left \
  -H "Content-Type: application/json" \
  -d '{"data":"AAAAAA=="}'

curl -X PUT http://localhost:5000/v1/diff/1/right \
  -H "Content-Type: application/json" \
  -d '{"data":"AAAAAA=="}'

curl http://localhost:5000/v1/diff/1
```

## Diff Semantics

- `404 Not Found` until both left and right payloads exist.
- `Equals`: identical byte arrays.
- `SizeDoNotMatch`: different lengths.
- `ContentDoNotMatch`: same length but differing bytes; `diffs` contains contiguous ranges.

## Tests

```bash
dotnet test src/Descartes.Test
```

## Assumptions and Choices

- In-memory repository only; no persistence or eviction.
- `id` is treated as an opaque string.
- `data` must be valid base64; `null` or invalid base64 returns `400`.
- An empty base64 string is allowed and represents an empty payload.
- Diff ranges are grouped by contiguous differing bytes (not a minimal diff algorithm).
