# API Contract: AI Content Brain Roadmap

All endpoints return JSON unless an export endpoint is requested. IDs are UUIDs. Generation endpoints return a request resource so the UI can show progress and poll or stream status.

## Projects

- `GET /projects` → `200 ProjectSummary[]`
- `POST /projects` body `{ name, description? }` → `201 Project`; the server assigns the canonical MVP Workspace
- `GET /projects/{projectId}` → `200 ProjectDetail` or `404`
- `PATCH /projects/{projectId}` body `{ name?, description?, status? }` → `200 Project`

## Ideas

- `GET /projects/{projectId}/ideas` → `200 Idea[]`
- `POST /projects/{projectId}/ideas/generate` body `{ niche, audience, platform, goal, successMetric, historicalContext?, count? }` → `202 GenerationRequest`; workspace ownership is derived from the project
- `POST /projects/{projectId}/ideas/generate` body `{ niche, audience, platform, goal, successMetric, historicalContext?, count? }` → `202 GenerationRequest`; workspace ownership is derived from the project
- `POST /projects/{projectId}/ideas` body `{ title, hook, description, platform, niche, audience, goal, successMetric, historicalContext? }` → `201 Idea`
- `PATCH /ideas/{ideaId}` body `{ title?, hook?, description?, status? }` → `200 Idea`

## Scripts

- `GET /ideas/{ideaId}/scripts` → `200 ScriptSummary[]`
- `POST /ideas/{ideaId}/scripts/generate` body `{ platform, format, formatVariant?, styleProfileId? }` → `202 GenerationRequest`
- `GET /generation-requests/{requestId}` → `200 GenerationRequest` including the persisted `formatVariant` when applicable
- `POST /generation-requests/{requestId}/cancel` → `202 GenerationRequest`
- `GET /scripts/{scriptId}` → `200 Script`
- `PATCH /scripts/{scriptId}` body `{ content, status? }` → `200 Script` with a new version

## Style profiles

- `GET /style-profiles` → `200 StyleProfile[]`
- `POST /style-profiles` multipart form with 0–10 validated files and `name` → `202 StyleProfileProcessing`
- `GET /style-profiles/{styleProfileId}` → `200 StyleProfileDetail`

## Exports

- `GET /scripts/{scriptId}/export?format=markdown` → `200 text/markdown`
- `GET /scripts/{scriptId}/export?format=docx` → `200 application/vnd.openxmlformats-officedocument.wordprocessingml.document`
- Clipboard copy is a browser interaction using the current script content and does not require a server endpoint.

Supported MVP platform values are `YouTube`, `TikTok`, `Instagram`, `Podcast`, `Blog`, and `Newsletter`. `Reels` and `Shorts` are represented by `formatVariant` values such as `instagram_reel` and `youtube_short`, not by separate platform values.

## Review evaluation

- `GET /scripts/{scriptId}/versions/{version}/review` → `200 StyleReview`
- `POST /scripts/{scriptId}/versions/{version}/review` → `200 StyleReview`; the server evaluates the immutable requested version

`StyleReview` contains pass/fail or scored results for audience fit, platform format, hook clarity, tone, pacing, vocabulary, structure, and production readiness, plus the reviewed script version and evaluation timestamp. `pending` and `failed` reviews cannot authorize `ready`; only a `passed` review for the requested current version can do so. Editing the script creates a new version that requires a new review.

## SuccessMetric and HistoricalContext shapes

The API contracts use the following JSON shapes for briefs and generation requests:

- `successMetric`: object `{ metricType: string, targetValue: number, unit?: string, timeWindow?: string }` where `metricType` is a canonical short name like `average_watch_time`, `click_through_rate`, or `sales`.
- `historicalContext` (optional): object `{ availability: "provided" | "unavailable", summary?: string, metrics?: [{ metricType: string, value: number, observedAt: string }] }`.

The server validates `successMetric` presence for idea-generation requests and returns HTTP 400 Problem Details when missing or invalid. When `historicalContext.availability` is omitted, the server records `"unavailable"` and proceeds.

## PATCH /scripts/{scriptId} readiness validation

`PATCH /scripts/{scriptId}` accepts `{ content?, status? }`. When a client requests `status: "ready"`, the API validates that a `StyleReview` with `status: "passed"` exists for the script's current version. If no such review exists, the API returns `409 Conflict` with a Problem Details body explaining that the ready transition is blocked until the current version has a passed review. The error `type` should be `https://example.com/probs/review-gate` and include a `detail` pointing to the required review endpoint: `/scripts/{scriptId}/versions/{version}/review`.

## Error contract

Errors use Problem Details with `type`, `title`, `status`, `detail`, and optional `traceId`. Validation errors identify fields. Provider, upload, and database errors expose safe user-facing messages and retain a correlation identifier for logs; secrets, raw prompts, and uploaded source contents are not returned in errors.

## UI contract

The Blazor dashboard must provide project navigation, brief entry, generated idea list, script editor, style upload status, request progress/cancellation, status transitions, and export actions. Loading, empty, validation, provider failure, and upload rejection states are required for each corresponding workflow.

The MVP API uses the dependency-free `ContentBrain.Contracts` project as the shared HTTP contract boundary. API and Web reference this project; Domain and Application do not reference it. Application models are mapped to contract DTOs at the API boundary.

## Phase 2 Growth endpoints

- `GET/POST/PATCH /personas` → persona CRUD scoped to the workspace
- `POST /ideas/{ideaId}/variants/generate` body `{ platforms[], personaId? }` → `202 GenerationRequest`
- `GET /ideas/{ideaId}/variants` → `200 ContentVariant[]`
- `POST /analytics/imports` → `202 ImportBatch`; repeated source records are idempotent
- `GET /analytics/insights?projectId=...` → `200 AnalyticsInsight[]`
- `POST /projects/{projectId}/members` and `DELETE /projects/{projectId}/members/{memberId}` → membership management
- `POST /content/{contentId}/comments` and `POST /approvals` → comments and approval decisions
- `GET/POST/PATCH /calendar-entries` → calendar planning by platform, date, and status

Phase 2 endpoints require workspace membership and optimistic-concurrency responses for conflicting updates.

## Phase 3 Pro / Enterprise endpoints

- `GET/POST/PATCH /brands/{brandId}/policies` → versioned style and compliance policy management
- `POST /research/search` → `202 ResearchRequest` with source-attributed findings
- `GET/POST /integrations` and `POST /automation/jobs` → scoped, idempotent external workflow integration
- `GET/POST /api-clients` and `POST /api-keys/rotate` → revocable tenant-scoped API access
- `GET/POST /roles` and `GET /audit-events` → RBAC and immutable audit access
- `POST /content/{contentId}/compliance-checks` → deterministic and advisory governance results

Phase 3 APIs require tenant authorization, rate limiting, audit events, API versioning, and Problem Details responses that do not expose secrets or private source data.
