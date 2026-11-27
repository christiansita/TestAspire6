# Tasks: AI Content Brain Roadmap

**Input**: Design documents from `/specs/001-ai-content-brain/`

**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, and `quickstart.md`

**Scope**: Product Phase 1 MVP, followed by planned Product Phase 2 Growth and Product Phase 3 Pro / Enterprise increments. Implementation phase numbers are prefixed with `MVP` or `Product` to avoid confusing foundation work with roadmap phases.

**Format**: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel with other tasks in the same phase when files do not overlap.
- **[Story]**: Maps the task to a user story for traceability.

Task identifiers are immutable traceability keys and are not required to be contiguous or ordered; newly discovered work may use a later identifier such as T###.

## MVP Phase 1: Setup

**Purpose**: Create the .NET 10 Aspire solution and project boundaries.

- [x] T001 Create the solution and project directories under `src/` and `tests/` according to `plan.md`.
- [x] T002 Add the .NET 10 Blazor Web App project in `src/ContentBrain.Web/`.
- [x] T003 [P] Add the ASP.NET Core API project in `src/ContentBrain.Api/`.
- [x] T004 [P] Add the dependency-free shared contract project `ContentBrain.Contracts` under `src/ContentBrain.Contracts/`.
- [x] T005 [P] Add the class library projects `ContentBrain.Domain`, `ContentBrain.Application`, and `ContentBrain.Infrastructure` under `src/`.
- [x] T006 [P] Add `ContentBrain.AppHost` and `ContentBrain.ServiceDefaults` Aspire projects under `src/`.
- [x] T007 [P] Add unit, API, and integration test projects under `tests/` and reference the required production projects.
- [x] T008 Add project references so API and Web reference `ContentBrain.Contracts`, API references Application, Application depends on Domain, and Infrastructure implements Application ports; Domain and Application must not reference Contracts.
- [x] T009 Add required NuGet packages through `dotnet add package`, including EF Core/Npgsql, Azure.Identity, Microsoft Agent Framework Foundry integration, and Microsoft Agent Framework Workflows using the prerelease flag where required by the selected package versions.
- [x] T010 Configure the AppHost to compose Web, API, and a PostgreSQL container and expose service references through Aspire configuration.

## MVP Phase 2: Foundational Infrastructure

**Purpose**: Blocking infrastructure required by all MVP user stories.

- [ ] T011 Create shared platform, content-format, status, and generation-operation enums in `src/ContentBrain.Domain/`, using YouTube, TikTok, Instagram, Podcast, Blog, and Newsletter as canonical platforms and Reels/Shorts as format variants.
 - [ ] T012 [P] Create Workspace, Project, Idea, Script, StyleProfile, StyleSample, GenerationRequest, and version-specific StyleReview domain entities in `src/ContentBrain.Domain/`. Include SuccessMetric and HistoricalContext value objects as part of the Idea and GenerationRequest domain model.
 - [ ] T013 [P] Implement domain validation and allowed status transitions in `src/ContentBrain.Domain/`. Validate SuccessMetric plausibility and HistoricalContext availability invariants; ensure StyleReview gating rules block transitions to `ready` when appropriate.
- [ ] T014 Create `ContentBrainDbContext` and EF Core mappings in `src/ContentBrain.Infrastructure/Persistence/`.
- [ ] T015 Create the initial PostgreSQL migration, seed exactly one canonical Workspace record, and seed any development content under that Workspace in `src/ContentBrain.Infrastructure/Persistence/Migrations/`.
- [ ] T016 Configure local PostgreSQL connection references and production Azure PostgreSQL configuration in `src/ContentBrain.Api/` and `src/ContentBrain.AppHost/`.
- [ ] T017 Configure `DefaultAzureCredential` for local Azure CLI authentication and managed identity authentication in Azure Container Apps for Foundry and PostgreSQL dependencies.
- [ ] T018 Add API Problem Details, validation, correlation IDs, cancellation-token propagation, and safe exception mapping in `src/ContentBrain.Api/`.
- [ ] T019 Add OpenTelemetry logging, traces, health checks, and dependency diagnostics through `src/ContentBrain.ServiceDefaults/` and the API/Web startup paths.
- [ ] T020 Create application ports for repositories, generation orchestration, file extraction/scanning, and exports in `src/ContentBrain.Application/`.
- [ ] T021 [P] Create shared HTTP request/response DTOs and validation models in `src/ContentBrain.Contracts/`.
 - [ ] T088 [P] Add `ContentBrain.Contracts` serialization, validation, enum-wire-value, and Problem Details compatibility tests in `tests/ContentBrain.ApiTests/Contracts/ContractSerializationTests.cs`.
- [ ] T089 [P] Add an architecture test that fails when `ContentBrain.Domain` or `ContentBrain.Application` references `ContentBrain.Contracts` in `tests/ContentBrain.UnitTests/ArchitectureTests.cs`.
- [ ] T087 [P] Add the platform template registry for YouTube, TikTok, Instagram, Podcast, blog, and newsletter outputs, including Reels and Shorts variants, in `src/ContentBrain.Application/Content/`.

**Checkpoint**: Solution builds, the AppHost starts, PostgreSQL is healthy, and all user stories can use the shared domain and application infrastructure.

## MVP Phase 3: User Story 1 - Generate Campaign-Ready Ideas (Priority: P1)

**Goal**: Turn niche, audience, platform, and goal inputs into concrete ideas with titles, hooks, and descriptions.

**Independent Test**: Submit a complete creator brief and receive persisted platform-aware ideas through the API and dashboard.

### Tests

- [ ] T022 [P] [US1] Add domain tests for creator brief validation, required success metrics, optional historical-context availability, and supported platform/goal values in `tests/ContentBrain.UnitTests/IdeaGenerationTests.cs`.
- [ ] T023 [P] [US1] Add API contract tests for `POST /projects/{projectId}/ideas/generate`, success metric and historical-context fields, and generation-request status in `tests/ContentBrain.ApiTests/IdeaGenerationContractTests.cs`.
- [ ] T024 [US1] Add integration coverage proving generated ideas are persisted with project, brief, success metric, historical-context availability, platform, and status in `tests/ContentBrain.IntegrationTests/IdeaGenerationTests.cs`.


### Implementation

- [ ] T025 [US1] Implement internal idea-generation commands, results, and application ports in `src/ContentBrain.Application/Ideas/`; keep HTTP request/response DTOs in `src/ContentBrain.Contracts/` and explicitly map them in API handlers.
- [ ] T026 [US1] Implement the ideation service using the Foundry generation port and platform template registry in `src/ContentBrain.Application/Ideas/IdeaGenerationService.cs`.
- [ ] T027 [US1] Implement the Foundry ideation adapter with structured output validation, cancellation, retry policy, and safe provider errors in `src/ContentBrain.Infrastructure/Foundry/`.
- [ ] T028 [US1] Implement project and idea persistence handlers in `src/ContentBrain.Application/Projects/` and `src/ContentBrain.Infrastructure/Persistence/`.
- [ ] T029 [US1] Implement project and idea endpoints from `contracts/api.md` in `src/ContentBrain.Api/Endpoints/`.
- [ ] T030 [US1] Implement the project list, brief form, generated idea list, empty state, loading state, and error state in `src/ContentBrain.Web/Components/Pages/`.
- [ ] T031 [US1] Add structured logs and generation-request progress updates for idea generation in `src/ContentBrain.Application/Ideas/` and `src/ContentBrain.Api/`.

**Checkpoint**: A user can create a project, submit a brief, receive usable ideas, and see them in the shared dashboard.

## MVP Phase 4: User Story 2 - Draft Styled Platform Scripts (Priority: P1)

**Goal**: Learn tone, pacing, vocabulary, and structure from samples and generate long-form or short-form scripts using platform-specific formats.

**Independent Test**: Upload valid samples, generate a script for a selected format, and verify the draft reflects the style profile and platform template.

### Tests

- [ ] T032 [P] [US2] Add upload validation tests for TXT, Markdown, DOCX, PDF, unsupported extensions, malformed content, size limits, zero-sample fallback, 1–2 sample warnings, 3–10 sample learning, and rejection above 10 samples in `tests/ContentBrain.UnitTests/StyleSampleValidationTests.cs`.
- [ ] T033 [P] [US2] Add API contract tests for style-profile upload and script generation endpoints in `tests/ContentBrain.ApiTests/ScriptGenerationContractTests.cs`.
- [ ] T034 [US2] Add integration coverage for sample processing, style-profile persistence, Podcast output structure, script versioning, version-specific review persistence, and failed-provider recovery in `tests/ContentBrain.IntegrationTests/ScriptGenerationTests.cs`.

### Implementation

- [ ] T035 [US2] Implement safe style-sample validation, generated storage names, quarantine status, and extraction ports in `src/ContentBrain.Application/Style/`.
- [ ] T036 [P] [US2] Implement TXT and Markdown extraction in `src/ContentBrain.Infrastructure/Style/`.
- [ ] T037 [P] [US2] Implement DOCX and PDF extraction adapters in `src/ContentBrain.Infrastructure/Style/`.
- [ ] T038 [US2] Implement malware-scan adapter boundary and clean/rejected/failed transitions in `src/ContentBrain.Infrastructure/Uploads/`.
- [ ] T039 [US2] Implement style-profile summarization using Foundry structured output and the extracted sample text in `src/ContentBrain.Application/Style/` and `src/ContentBrain.Infrastructure/Foundry/`.
- [ ] T040 [US2] Implement internal script-generation commands, results, and the sequential ideation/style/platform/script workflow in `src/ContentBrain.Application/Generation/`; keep HTTP DTOs in `src/ContentBrain.Contracts/`.
- [ ] T041 [US2] Implement script persistence with append-only versions and status transition rules in `src/ContentBrain.Infrastructure/Persistence/`.
- [ ] T042 [US2] Implement style-profile upload and script-generation endpoints from `contracts/api.md` in `src/ContentBrain.Api/Endpoints/`.
- [ ] T043 [US2] Implement the style upload panel, processing state, script editor, generation progress, cancellation, and provider-error UI in `src/ContentBrain.Web/Components/Pages/`.
- [ ] T044 [US2] Add platform-aware formatting for YouTube long-form and TikTok/Reels/Shorts short-form scripts, plus Instagram/blog/newsletter templates, in `src/ContentBrain.Application/Content/`.
- [ ] T045 [US2] Implement structured script review evaluation and expose results for audience fit, platform format, hook clarity, tone, pacing, vocabulary, structure, and production readiness in `src/ContentBrain.Application/Review/`.
- [ ] T046 [US2] Add the rule that a script cannot transition to `ready` until a r6eview result is available, with user-visible limited-style and review warnings in `src/ContentBrain.Application/` and `src/ContentBrain.Web/`.
- [ ] T090 [US2] Add domain and API tests proving `pending` and `failed` reviews cannot authorize `ready`, while only a `passed` review for the current script version can authorize the transition; verify edits require a new review in `tests/ContentBrain.UnitTests/` and `tests/ContentBrain.ApiTests/`.

**Checkpoint**: A user can upload samples, generate a styled platform-specific script, edit it, and preserve previous versions.

## MVP Phase 5: User Story 3 - Manage Projects, Status, and Exports (Priority: P1)

**Goal**: Provide a dashboard workflow for projects, ideas, scripts, statuses, and production-ready exports.

**Independent Test**: Create a project, manage its content through draft/in-review/ready, and export the current script as clipboard content, Markdown, and DOCX.

### Tests

- [ ] T047 [P] [US3] Add domain tests for project/item status transitions and append-only script versions in `tests/ContentBrain.UnitTests/ProjectWorkflowTests.cs`.
- [ ] T048 [P] [US3] Add API contract tests for project CRUD, status updates, script edits, and export content types in `tests/ContentBrain.ApiTests/ProjectAndExportContractTests.cs`.
- [ ] T049 [US3] Add integration coverage for project aggregation, version preservation, export generation, and export failure recovery in `tests/ContentBrain.IntegrationTests/ProjectWorkflowTests.cs`.

### Implementation

- [ ] T050 [US3] Implement project detail and dashboard query services in `src/ContentBrain.Application/Projects/`.
- [ ] T051 [US3] Implement script edit commands that create a new version and retain prior versions in `src/ContentBrain.Application/Scripts/`.
- [ ] T052 [US3] Implement Markdown and DOCX export adapters behind the export port in `src/ContentBrain.Infrastructure/Exports/`.
- [ ] T053 [US3] Implement project, status, script-edit, review, and export endpoints in `src/ContentBrain.Api/Endpoints/`.
- [ ] T054 [US3] Implement dashboard project navigation, idea/script views, status controls, version history, clipboard copy, Markdown download, DOCX download, and review results in `src/ContentBrain.Web/Components/Pages/`.
- [ ] T055 [US3] Add safe export and persistence error handling that keeps existing content available after failures in `src/ContentBrain.Api/` and `src/ContentBrain.Web/`.

**Checkpoint**: The complete MVP workflow is independently usable from project creation through export.

## MVP Phase 6: Polish, Validation, and Deployment

- [ ] T056 [P] Add accessibility labels, keyboard navigation, and clear validation/error messaging to the Blazor workflows in `src/ContentBrain.Web/`.
- [ ] T057 [P] Add configuration documentation and local `az login`/`DefaultAzureCredential` setup instructions to `README.md`.
- [ ] T058 Add Azure Container Apps deployment configuration through the Aspire deployment workflow, including API managed identity assignment and Foundry/PostgreSQL role bindings.
- [ ] T059 Add production upload quarantine and malware-scanning configuration and verify no executable permissions are granted to upload storage.
- [ ] T060 Run the complete unit, API, integration, and UI smoke test suites using the scenarios in `specs/001-ai-content-brain/quickstart.md`.
- [ ] T061 [P] Run at least 20 complete idea-generation requests, record duration, and verify at least 95% complete within 3 minutes in `tests/ContentBrain.IntegrationTests/PerformanceValidationTests.cs`.
- [ ] T062 [P] Run a facilitated usability validation with at least 5 representative users or facilitators and record completion, assistance, errors, and abandonment as evidence in `specs/001-ai-content-brain/validation/usability-validation.md`.
- [ ] T063 [P] Blindly review at least 10 generated scripts against the style rubric and record the pass rate as evidence in `specs/001-ai-content-brain/validation/style-review-validation.md`.
- [ ] T064 Run `dotnet build` for the complete solution; fix all compilation errors and rebuild until zero errors remain.
- [ ] T065 Validate the Aspire deployment manifest and Azure Container Apps deployment, then verify health, logs, managed identity, Foundry, and PostgreSQL connectivity.

## Product Phase 2: Growth

**Goal**: Turn the MVP into a multi-channel daily operating tool.

- [ ] T066 Create Persona, ContentVariant, AnalyticsMetric, ProjectMember, Comment, Approval, and CalendarEntry entities and migrations in `src/ContentBrain.Domain/` and `src/ContentBrain.Infrastructure/`.
- [ ] T067 Add workspace membership and optimistic-concurrency policies for shared projects in `src/ContentBrain.Application/` and `src/ContentBrain.Infrastructure/`.
- [ ] T068 [P] [US4] Add multi-channel variant contracts and generation workflow in `src/ContentBrain.Application/Variants/`.
- [ ] T069 [P] [US4] Add persona CRUD, persona-aware prompt context, and generation validation in `src/ContentBrain.Application/Personas/`.
- [ ] T070 [P] [US4] Add analytics provider adapter interfaces and idempotent import processing in `src/ContentBrain.Infrastructure/Analytics/`.
- [ ] T071 [P] [US5] Add project membership, comments, approval commands, and conflict responses in `src/ContentBrain.Application/Collaboration/`.
- [ ] T072 [P] [US5] Add calendar query and scheduling services in `src/ContentBrain.Application/Calendar/`.
- [ ] T073 Add Phase 2 API endpoints from `contracts/api.md` and workspace authorization checks in `src/ContentBrain.Api/Endpoints/`.
- [ ] T074 Add dashboard views for variants, personas, analytics insights, collaboration, approvals, and calendar planning in `src/ContentBrain.Web/Components/Pages/`.
- [ ] T075 Add Phase 2 unit, contract, integration, and UI tests for lineage, idempotent imports, persona targeting, collaboration conflicts, and calendar behavior under `tests/`.
- [ ] T076 Deploy Phase 2 to Azure Container Apps and validate analytics secrets, workspace authorization, concurrency, logs, and smoke scenarios.

**Checkpoint**: One core idea produces governed multi-channel variants, metrics inform recommendations, teams can review content, and the calendar supports daily planning.

## Product Phase 3: Pro / Enterprise

**Goal**: Establish a governed, automatable, tenant-aware content operating system.

- [ ] T077 Create Tenant, Brand, ResearchSource, ResearchFinding, AutomationConnection, AutomationJob, Role, Permission, ApiClient, and AuditEvent entities and migrations.
- [ ] T078 Implement tenant isolation, role-based authorization, scoped API clients, revocation, quotas, and rate limiting in `src/ContentBrain.Application/Authorization/` and `src/ContentBrain.Api/`.
- [ ] T079 [P] Implement source-attributed research workflows for trends, competitor topics, and FAQs in `src/ContentBrain.Application/Research/` and `src/ContentBrain.Infrastructure/Research/`.
- [ ] T080 [P] Implement versioned brand policies, deterministic banned-phrase/required-claim checks, advisory AI review, and publish gating in `src/ContentBrain.Application/Governance/`.
- [ ] T081 [P] Implement external upload/scheduling adapters with idempotency keys, retries, webhooks, and human approval gates in `src/ContentBrain.Infrastructure/Automation/`.
- [ ] T082 Publish versioned external API contracts, API-key rotation, tenant-scoped webhooks, and Problem Details behavior in `specs/001-ai-content-brain/contracts/` and `src/ContentBrain.Api/`.
- [ ] T083 Implement immutable audit event capture and exportable audit reports for content, policy, integration, and administration changes.
- [ ] T084 Add enterprise retention, security alerting, disaster-recovery, and observability configuration for Azure Container Apps and PostgreSQL.
- [ ] T085 Add Phase 3 security, tenant-isolation, policy, automation, API, and audit integration tests under `tests/ContentBrain.IntegrationTests/`.
- [ ] T086 Deploy Phase 3 capabilities to a controlled Azure Container Apps environment and validate least privilege, API revocation, audit completeness, and recovery scenarios.

**Checkpoint**: Enterprise tenants can research, govern, automate, integrate, authorize, and audit content operations safely.

## Dependencies and Execution Order

### Phase Dependencies

- MVP Setup has no dependencies.
- MVP Foundation depends on MVP Setup and blocks all MVP user stories.
- MVP user stories depend on MVP Foundation.
- User Story 2 uses the project and idea context from User Story 1, but its service and API tests can run with seeded project/idea data.
- User Story 3 uses persisted projects and scripts from User Stories 1 and 2 for the full flow, but CRUD/status/export behavior is independently testable with fixtures.
- MVP Polish depends on all MVP stories.
- Product Phase 2 depends on MVP validation and stable MVP contracts.
- Product Phase 3 depends on Product Phase 2's stable contracts and tenant-boundary review.

### Parallel Execution Examples

- After T001: T003–T006 can run in parallel across independent project files.
- After T011: T012, T013, T021, and T087 can run in parallel where file ownership does not overlap.
- Within US1: T022 and T023 can run in parallel; T025 and T028 can proceed in parallel after foundational contracts exist.
- Within US2: T032 and T033 can run in parallel; T036 and T037 can run in parallel because they are separate extraction adapters.
- Within US3: T047 and T048 can run in parallel; T052 can run independently of dashboard work.
- In MVP Phase 6: T056, T057, T059, T061, T062, and T063 can run in parallel before deployment validation.
- In Product Phase 2: T068–T072 can run in parallel after T066/T067; T073–T076 follow the capability implementations.
- In Product Phase 3: T079–T081 can run in parallel after T077/T078; T082–T086 follow the stable enterprise contracts.

## Implementation Strategy

1. Complete Setup and Foundational phases.
2. Deliver US1 first as the smallest validation slice: project → brief → generated ideas.
3. Add US2 to validate the primary scripting and style-learning value.
4. Add US3 to make the workflow persistent, reviewable, and exportable.
5. Run the complete MVP validation quickstart before treating the MVP as ready.
6. Implement Product Phase 2 only after MVP usage validates the core workflow.
7. Implement Product Phase 3 only after Product Phase 2 contracts and tenant boundaries are stable.
