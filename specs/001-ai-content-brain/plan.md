# Implementation Plan: AI Content Brain

**Branch**: `001-ai-content-brain` | **Date**: 2026-09-03 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `/specs/001-ai-content-brain/spec.md`

**Note**: This template is filled in by the `/speckit-plan` command; its definition describes the execution workflow.

## Summary

Deliver AI Content Brain in three deliberate phases. Phase 1 validates the core shared-workspace workflow for ideas, styled scripts, projects, and exports. Phase 2 turns the product into a daily multi-channel operating tool with personas, analytics, collaboration, and a content calendar. Phase 3 adds enterprise research, brand governance, workflow automation, public API access, role-based permissions, and auditability. All phases use the same .NET 10, Blazor, ASP.NET Core, Agent Framework, PostgreSQL, Aspire, and Azure Container Apps foundation.

## Technical Context

**Language/Version**: C# / .NET 10

**Primary Dependencies**: Blazor Web App, ASP.NET Core Minimal APIs, EF Core with Npgsql, Microsoft Agent Framework, Microsoft Foundry `AIProjectClient`, .NET Aspire

**Storage**: PostgreSQL; local Aspire PostgreSQL container; Azure Database for PostgreSQL Flexible Server in production. Store extracted style samples and profiles as application data with a quarantine/validation boundary for uploads.

**Testing**: .NET unit tests, API contract tests, integration tests with PostgreSQL, and end-to-end smoke validation through the Blazor UI

**Target Platform**: ASP.NET Core in Azure Container Apps; local Windows development with Visual Studio and Aspire

**Project Type**: Distributed web application

**Performance Goals**: Idea generation begins returning a useful result within 3 minutes; script generation provides visible progress or streaming status; ordinary CRUD interactions feel immediate under MVP load.

**Constraints**: Shared workspace with no MVP user isolation; AI calls must use structured outputs and cancellation; uploads must validate extension, content, size, and malware status before style processing; secrets must not be committed; production database and Foundry access must use managed identity where supported; local Foundry access must use Azure CLI authentication through `DefaultAzureCredential`.

**Scale/Scope**: Phase 1 supports a small validation cohort and one persisted Workspace record. Phase 2 supports multiple personas, channels, collaborators, calendar entries, and imported metrics. Phase 3 supports agency/enterprise tenants, governed brands, external integrations, API consumers, role policies, and auditable operations.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Passes with these gates:
- Creator briefs, generated ideas, and scripts MUST identify audience, goal, platform, and production format.
- Platform-specific templates MUST be explicit; generic output is not acceptable as the final result.
- AI responses MUST be validated as structured application data before persistence.
- Style processing MUST preserve the creator's recognizable voice without copying source content verbatim.
- Uploads and exports MUST fail safely without data loss, and generation MUST expose actionable errors.
- The MVP MUST provide observable progress and structured logs for AI and persistence failures.
- Every generated output MUST be reviewable against the style and production-readiness rubric before it is treated as ready.

## Project Structure

### Documentation (this feature)

```text
specs/001-ai-content-brain/
├── plan.md              # Architecture and implementation plan
├── research.md          # Architecture research and decisions
├── data-model.md        # Canonical entities and invariants
├── quickstart.md        # Validation and smoke-run checklist
├── contracts/           # HTTP API contracts
└── tasks.md             # Dependency-ordered implementation task list
```

### Source Code (repository root)

```text
src/
├── ContentBrain.AppHost/
├── ContentBrain.ServiceDefaults/
├── ContentBrain.Web/
├── ContentBrain.Api/
├── ContentBrain.Contracts/
├── ContentBrain.Application/
├── ContentBrain.Domain/
└── ContentBrain.Infrastructure/

tests/
├── ContentBrain.UnitTests/
├── ContentBrain.ApiTests/
└── ContentBrain.IntegrationTests/
```

**Structure Decision**: Use a layered Aspire distributed application. The Web project owns Blazor UI and export interactions; the API project exposes project, generation, upload, review, and asset endpoints; Contracts contains dependency-free HTTP DTOs and validation models; Application contains use cases and orchestration ports; Domain contains entities and state rules; Infrastructure contains EF Core, PostgreSQL, Foundry, file extraction, and export adapters. The AppHost composes Web, API, and PostgreSQL locally and publishes the composition to Azure Container Apps. Production PostgreSQL and Foundry access use managed identity; local Foundry access uses `DefaultAzureCredential` backed by the developer's Azure CLI login.

**Contract Boundary Decision**: Add `ContentBrain.Contracts` as a dependency-free shared project for versioned request/response DTOs, validation models, enums used by the HTTP surface, and Problem Details extensions. `ContentBrain.Api` references it to expose endpoints, and `ContentBrain.Web` references it only through a typed HTTP client implementation. Domain and Application remain independent of the HTTP contract project; application commands map to and from contract DTOs at the API boundary. Phase 3 may publish these contracts as a versioned external package.

**MVP Research-First Interpretation**: The constitution's research-first model is satisfied in MVP by requiring a structured creator brief with a success metric, treating uploaded style samples as historical creator evidence, and accepting optional historical performance context. When performance context is unavailable, the system records that absence and displays a creator-context-only notice. The Phase 3 public-web research agent remains additive rather than a prerequisite for MVP generation.

## Implementation Sequence

1. Bootstrap the .NET 10 solution and Aspire AppHost with Web, API, Contracts, Application, Domain, Infrastructure, ServiceDefaults, and test projects.
2. Add validated configuration and secret boundaries for Foundry endpoint/deployment, PostgreSQL, local Aspire resources, and export/upload limits.
3. Implement the canonical Workspace entity, domain entities, status transitions, EF Core mappings, migrations, and seed data.
4. Implement style sample ingestion with TXT, Markdown, DOCX, and PDF extraction, server-side validation, quarantine/scanning status, and style profile summarization.
5. Implement Foundry integration through `AIProjectClient`, typed generation contracts, cancellation, retries, safe error mapping, and focused ideation/style/platform/script steps.
6. Implement API endpoints from `contracts/api.md`, including CRUD, generation request status, cancellation, uploads, reviews, and exports; map `ContentBrain.Contracts` DTOs at the HTTP boundary.
7. Implement the Blazor dashboard flows for projects, briefs, ideas, style profiles, script editing/versioning, status transitions, progress, exports, and style-rubric review.
8. Add unit, API contract, PostgreSQL integration, UI smoke tests, performance validation, and facilitated usability validation covering the acceptance and failure scenarios in `quickstart.md`.
9. Run `dotnet build` for the complete solution and fix all errors; run the relevant test suites and resolve failures before completion.
10. Validate the Aspire deployment manifest and deploy the composed application to Azure Container Apps, then verify resource health, managed-identity permissions, Foundry connectivity, PostgreSQL connectivity, logs, and end-to-end smoke scenarios.

## Product Phase 2 – Growth Implementation Plan

Phase 2 begins only after the Phase 1 workflow is stable and user validation demonstrates repeat use. It extends the existing content model rather than creating a parallel system.

1. Add persona, channel variant, analytics metric, collaboration member/comment/approval, and calendar-entry entities with tenant/workspace ownership boundaries.
2. Add multi-channel generation workflows that derive YouTube, TikTok, Instagram, Podcast, blog, and newsletter variants from one core idea while preserving a shared narrative identifier.
3. Add persona-aware prompt context and platform/persona validation to idea and script generation.
4. Add analytics import adapters with idempotent source records for views, watch time, CTR, and platform identifiers; keep provider-specific credentials outside application data.
5. Add shared-project membership, comments, approval transitions, and optimistic concurrency handling.
6. Add calendar queries and scheduling metadata without implementing external publishing in this phase.
7. Add dashboard views for content plans, persona selection, analytics insights, collaboration, approvals, and calendar filtering.
8. Add integration tests for repeated imports, concurrent edits, multi-channel consistency, persona targeting, and calendar status changes.

## Product Phase 3 – Pro / Enterprise Implementation Plan

Phase 3 begins after Growth workflows have stable contracts and clear tenant boundaries.

1. Add tenant, brand, membership, role, policy, audit-event, integration-connection, and API-client entities.
2. Implement the research agent as a bounded, source-attributed workflow for public trends, competitor topics, and FAQs; store provenance and retrieval timestamps.
3. Implement brand governance with versioned style guides, banned phrases, required claims, compliance rules, and pre-publish validation results.
4. Implement workflow automation adapters for upload/scheduling providers with retries, idempotency keys, webhook processing, and human approval gates.
5. Publish versioned API contracts, API key/OAuth client management, quotas, rate limits, and tenant-scoped webhooks.
6. Add role-based authorization and immutable audit logs for content, policy, integration, and administrative operations.
7. Add enterprise observability, retention policies, security alerts, exportable audit reports, and disaster-recovery validation.
8. Add end-to-end tests for tenant isolation, policy enforcement, API consumers, automation retries, role boundaries, and audit completeness.

## Post-Design Constitution Check

- Creator-first ideation: satisfied by mandatory brief context and production-ready API/UI outputs.
- Platform-centered execution: satisfied by platform and format enums, templates, and contract fields.
- Data-aware decision making: satisfied by style samples and an extension point for later analytics; analytics is not part of MVP.
- Consistency and brand clarity: satisfied by StyleProfile and typed style application before drafting.
- Production-ready output: satisfied by script editing, versioning, explicit statuses, and Markdown/DOCX/clipboard exports.
- Governance: satisfied by safe upload handling, structured logs/correlation IDs, no secrets in errors, and failure tests.
- Deployment: satisfied by Azure Container Apps as the standardized cloud target, managed identity for production dependencies, and Aspire health/log validation.
- Phase 2 readiness: the MVP model retains stable project/idea/script identifiers and extensible platform/context fields for variants, personas, analytics, collaboration, and calendar data.
- Phase 3 readiness: authorization, audit, provenance, and integration boundaries are explicit extension points rather than assumptions hidden in MVP handlers.
- Validation: measurable latency, usability completion, and style-rubric outcomes are defined separately for MVP and post-MVP phases.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | The layered structure is required to keep UI, API, domain rules, AI orchestration, and infrastructure replaceable while supporting Aspire composition. | A single project would couple platform UI, persistence, and preview AI SDKs, increasing MVP change risk. |
