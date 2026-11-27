# Data Model: AI Content Brain Roadmap

## Workspace
- `id`: UUID primary key
- `name`: required, initialized to the shared MVP workspace name
- `created_at`, `updated_at`: UTC timestamps

Represents the shared MVP boundary. Phase 1 creates exactly one canonical Workspace record, and all projects, style profiles, samples, ideas, scripts, and generation requests reference it. Future user and tenant ownership is added through relationships to this stable identifier.

## Project
- `id`: UUID primary key
- `name`: required, 1–120 characters
- `description`: optional text
- `status`: `active` or `archived`
- `workspace_id`: required foreign key to Workspace
- `created_at`, `updated_at`: UTC timestamps

A project contains ideas and scripts and represents a campaign or series.

## Idea
- `id`: UUID primary key
- `project_id`: required foreign key to Project
- `workspace_id`: required foreign key to Workspace
- `title`: required
- `hook`: required
- `description`: required
- `niche`, `audience`, `goal`, `success_metric`: required brief context
- `historical_context`: optional creator-provided performance context; absence is recorded explicitly
- `platform`: required enum: YouTube, TikTok, Instagram, Podcast, Blog, Newsletter
- `status`: `draft`, `in_review`, `ready`
- `created_at`, `updated_at`: UTC timestamps

## Script
- `id`: UUID primary key
- `idea_id`: required foreign key to Idea
- `workspace_id`: required foreign key to Workspace
- `platform`: required supported platform
- `format`: `youtube_long_form`, `short_form`, `instagram`, `podcast`, `blog`, or `newsletter`
- `format_variant`: optional variant such as `tiktok_short`, `instagram_reel`, or `youtube_short`
- `content`: required text
- `version`: positive integer unique per idea
- `status`: `draft`, `in_review`, `ready`
- `style_profile_id`: optional foreign key to StyleProfile
- `created_at`, `updated_at`: UTC timestamps

Versions are append-only. Updating a script creates a new version rather than overwriting a prior version.

## StyleProfile
- `id`: UUID primary key
- `workspace_id`: required foreign key to Workspace
- `name`: required
- `source_count`: integer from 0 to 10
- `tone_summary`, `pacing_summary`, `vocabulary_summary`, `structure_summary`: extracted profile data
- `created_at`, `updated_at`: UTC timestamps

## StyleSample
- `id`: UUID primary key
- `style_profile_id`: required foreign key
- `workspace_id`: required foreign key to Workspace
- `original_file_name`: display-only sanitized name
- `media_type`: validated MIME type
- `file_extension`: validated extension
- `extracted_text`: required after extraction
- `scan_status`: `pending`, `clean`, `rejected`, `failed`
- `created_at`: UTC timestamp

A style profile accepts 0–10 samples. Zero samples use generic fallback; 1–2 samples mark the profile as limited; 3–10 samples enable full style learning; attempts to add an 11th sample are rejected.

## GenerationRequest
- `id`: UUID primary key
- `workspace_id`: required foreign key to Workspace
- `project_id`: required foreign key
- `idea_id`: optional foreign key
- `operation`: `generate_ideas` or `generate_script`
- `platform`, `format`, `niche`, `audience`, `goal`, `success_metric`: request context
- `historical_context`: optional creator-provided performance context and availability state
- `format_variant`: optional normalized variant such as `tiktok_short`, `instagram_reel`, or `youtube_short`
- `style_profile_id`: optional foreign key
- `status`: `queued`, `running`, `completed`, `failed`, `cancelled`
- `error_message`: optional safe diagnostic
- `created_at`, `completed_at`: UTC timestamps

## SuccessMetric

- `success_metric` (value object)
  - `metric_type`: string (e.g., "average_watch_time", "click_through_rate", "sales")
  - `target_value`: numeric target (float) or human-friendly threshold
  - `unit`: optional string (e.g., "seconds", "%", "conversions")
  - `time_window`: optional string describing the target window (e.g., "7d", "30d")

The SuccessMetric is a small value object attached to briefs, generation requests, and persisted ideas. It is required for idea-generation requests in the MVP and must be validated for presence and a plausible metric_type/target combination.

## HistoricalContext

- `historical_context` (optional object)
  - `availability`: enum — `provided` or `unavailable`
  - `summary`: optional short human-provided description
  - `metrics`: optional array of lightweight metric records (metric_type, value, observed_at)

When creators omit historical context, the API records `historical_context.availability = "unavailable"` and proceeds. When provided, the system may use the included `metrics` to influence generation. HistoricalContext content is not treated as authoritative and may be ignored by some generation strategies.

## Relationships and invariants
- Project 1-to-many Idea; Idea 1-to-many Script; StyleProfile 1-to-many StyleSample.
- Workspace 1-to-many Project, Idea, Script, StyleProfile, and GenerationRequest.
- Every persisted idea and script belongs to a Workspace directly and to a Project through its relationship chain.
- IDs are generated server-side and are never accepted as client-controlled ownership identifiers.
- Status transitions are `draft → in_review → ready`; reopening a ready item requires an explicit transition back to `draft`.
- All timestamps are UTC. Generation requests are cancellable and must not leave partially persisted final assets.
- Canonical platform values are YouTube, TikTok, Instagram, Podcast, Blog, and Newsletter. Reels and Shorts are format/channel variants, not additional platform enum values.
- Generated scripts expose a version-specific review evaluation for audience fit, platform format, hook clarity, tone, pacing, vocabulary, structure, and production readiness.

## StyleReview
- `id`: UUID primary key
- `script_id`: required foreign key to Script
- `script_version`: required version reviewed
- `status`: `pending`, `passed`, `failed`
- `audience_fit`, `platform_format`, `hook_clarity`, `tone`, `pacing`, `vocabulary`, `structure`, `production_readiness`: scored or pass/fail rubric results
- `evaluation_source`: `system`, `creator`, or `team_reviewer`
- `evaluated_at`: UTC timestamp

Each script version has its own review. A unique key on `(script_id, script_version)` prevents duplicate current-version reviews. A content edit creates a new script version and invalidates the prior version's readiness until the new version is reviewed. `pending` and `failed` reviews cannot authorize `ready`; only `passed` for the current version can do so.

## Phase 2 entities

### Persona
- `id`, `workspace_id`, `name`, `demographics`, `psychographics`, `pain_points`, `communication_preferences`
- Personas are selectable generation context and are versioned when materially changed.

### ContentVariant
- `id`, `workspace_id`, `core_idea_id`, `platform`, `format`, `format_variant`, `hook`, `content`, `status`, `created_at`
- Variants retain the core idea lineage and have independent review status.

### AnalyticsMetric
- `id`, `workspace_id`, `source`, `external_content_id`, `metric_type`, `metric_value`, `observed_at`, `import_batch_id`
- A source/external-content/metric/observed-at key prevents duplicate imports.

### ProjectMember, Comment, Approval
- Project members have a workspace role; comments target an idea, script, or variant; approvals record reviewer, decision, and timestamp.
- Concurrent edits use a version or row token and return a conflict instead of silently overwriting content.

### CalendarEntry
- `id`, `workspace_id`, `project_id`, `content_id`, `platform`, `scheduled_at`, `status`
- Calendar planning does not publish externally in Phase 2.

## Phase 3 entities

### Tenant and Brand
- Tenant owns workspaces, memberships, policies, integrations, API clients, and audit events.
- Brand contains versioned style guides, banned phrases, required claims, and compliance rules.

### ResearchSource and ResearchFinding
- Sources and findings include required `workspace_id` and optional `tenant_id`. Findings retain source URL, retrieval timestamp, query/topic, evidence excerpt, and confidence metadata.

### AutomationConnection and AutomationJob
- Connections reference an external provider without storing raw secrets; jobs use idempotency keys, retry state, webhook correlation, and approval state.

### Role, Permission, ApiClient, AuditEvent
- Roles grant scoped permissions; API clients are tenant-scoped and revocable; audit events are append-only and immutable.

## Cross-phase invariants

- Every Phase 2/3 entity is scoped to its workspace or tenant; cross-tenant reads are forbidden.
- Content lineage from core idea to variants, scripts, research, approvals, and publishing jobs is retained.
- A content item cannot become ready or be externally published when a deterministic brand policy violation or required approval is unresolved.
