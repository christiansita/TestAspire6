# Feature Specification: AI Content Brain

**Feature Branch**: `001-ai-content-brain`

**Created**: 2026-09-03

**Status**: Draft

**Input**: User description: " 
Phase 1 – MVP
Goal: Validate that creators will use AI Content Brain as their primary ideation and scripting hub.

Key features:

* Idea generation:

	- Input: niche, audience, platform, goals (growth, sales, engagement).

	- Output: list of content ideas with titles, hooks, and brief descriptions.

* Script drafting:

	- Long-form video scripts (YouTube).

	- Short-form scripts (TikTok/Reels/Shorts).

* Style learning:

	- User uploads 0–10 past pieces (scripts, captions, blog posts), with limited-profile behavior below three samples.

	- AI learns tone, pacing, vocabulary, structure.

* Platform-aware formatting:

	- Different output templates for YouTube, TikTok, Instagram, podcasts, blogs, and newsletters.

* Simple dashboard (Blazor):

	- Idea list, script editor, export (copy, download, maybe Markdown/Docx).

* Basic project structure:

	- “Projects” for campaigns or series.

	- Each project has ideas, scripts, and status (draft, in review, ready).

Phase 2 – Growth
Goal: Turn AI Content Brain into a daily operational tool.

Features:

* Multi-channel content plans:

	- One core idea → auto-generated variants for each platform.

* Audience persona builder:

	- Define personas (demographics, psychographics, pain points).

	- AI tailors scripts and hooks to each persona.

* Analytics integration (optional early):

	- Import performance data (views, watch time, CTR) from YouTube/other APIs.

	- AI suggests topics based on what works.

* Collaboration:

	- Team accounts, shared projects, comments, approvals.

* Content calendar (lightweight):

	- Plan content by date, platform, status.

Phase 3 – Pro / Enterprise
Goal: Make it a strategic content OS for teams.

Features:

* Advanced research agent:

	- Pulls in public web info (trends, competitor topics, FAQs).

* Brand governance:

	- Style guides, banned phrases, compliance rules.

* Workflow automation:

	- Integrations with upload/scheduling tools.

* API access:

	- Let agencies integrate AI Content Brain into their own systems.

* Role-based access & audit logs."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Generate campaign-ready content ideas from a brief (Priority: P1)
A creator enters their niche, audience, platform, desired goal, success metric, and any available historical performance context, then receives a ranked set of content concepts with titles, hooks, and short descriptions. The system reduces blank-page friction by turning a vague business objective into concrete content directions that fit the platform and audience.

**Why this priority**: This is the core job-to-be-done: turning a creator's intent into publishable content ideas quickly enough to support daily ideation and experimentation.

**Independent Test**: A user can provide a niche, target audience, platform, goal, and success metric, optionally provide historical context, and receive a usable content idea list without needing any additional editing or setup.

**Acceptance Scenarios**:

1. **Given** a creator wants to grow a fitness audience on YouTube, **When** they input their niche, target audience, platform, growth goal, and a target watch-time metric, **Then** the system returns multiple content ideas with titles, hooks, concise descriptions, and the brief context used.
2. **Given** a creator wants sales-focused Instagram content, **When** they define the audience and objective, **Then** ideas emphasize conversion-oriented angles, hooks, and value propositions suited to that platform.
3. **Given** the user omits historical performance context, **When** the brief is otherwise complete, **Then** the system records that context as unavailable, explains that recommendations use creator-provided context only, and continues without treating the omission as an error.
4. **Given** the user enters incomplete required information, **When** the system cannot infer a key detail, **Then** it asks for the missing input rather than silently inventing a success metric.

### Success metric and historical context

The brief requires a `successMetric` value that describes the creator's intended performance target. `historicalContext` is optional; when absent the system records `availability = "unavailable"` and proceeds. The API validates presence and plausible values for `successMetric` and returns validation Problem Details when missing.

---

### User Story 2 - Draft scripts in creator-specific formats and voice (Priority: P1)
A creator uploads past scripts, captions, or blog posts to teach the system their tone and structure. The system then drafts long-form YouTube scripts or short-form TikTok/Reels/Shorts scripts that match the creator's style and platform conventions.

**Why this priority**: Script generation is the highest-value operational workflow after idea generation; it determines whether the product becomes a daily production tool rather than just a brainstorming assistant.

**Independent Test**: A creator can upload sample work, choose a format, and generate a script that reflects their established voice and pacing.

**Acceptance Scenarios**:

1. **Given** a creator uploads 5 prior YouTube scripts, **When** they request a new long-form script, **Then** the generated script follows a similar tone, pacing, and structure without requiring manual rewriting.
2. **Given** a creator wants a short-form TikTok script, **When** they choose the platform template, **Then** the output uses a concise hook, fast pacing, and platform-appropriate length.
3. **Given** a creator requests a Podcast output, **When** they choose the Podcast template, **Then** the output includes an episode title, introduction, segment outline, speaker-ready content, and closing call to action.
4. **Given** the system has no prior samples, **When** the user requests a script, **Then** it still generates a usable draft using safe generic style defaults and instructions.

---

### User Story 3 - Manage content work in projects with review states (Priority: P1)
A creator organizes work into projects such as campaigns or content series. Each project contains ideas, scripts, and statuses such as draft, in review, and ready. The dashboard gives the user a simple workflow for moving content from concept to publication without losing context.

**Why this priority**: Project management is the minimum structure needed for repeatable content operations and supports adoption by both solo creators and small teams.

**Independent Test**: A user can create a project, add multiple ideas and scripts, and update status as content moves through the pipeline.

**Acceptance Scenarios**:

1. **Given** a creator starts a campaign project, **When** they add ideas and scripts, **Then** each item appears under the project with the correct status.
2. **Given** a script is ready for review, **When** the user updates its status to in review, **Then** the dashboard reflects that state for the project.
3. **Given** a script has no completed style review, **When** the user attempts to mark it ready, **Then** the system blocks the transition and explains that review is required.
4. **Given** a script has a completed style review, **When** the user marks it ready, **Then** the transition succeeds and the review remains associated with that script version.
5. **Given** the user exports a project asset, **When** they choose copy or download, **Then** the generated output is available in an easily shareable format.

---

### User Story 4 - Adapt a single core idea across multiple channels (Priority: P2)
A creator turns one overarching concept into several platform-specific variations. The system creates variants for YouTube, TikTok, Instagram, Podcast, blog, and newsletter while preserving the same strategic message and optimized hooks for each channel.

**Why this priority**: This is the next layer of operational value and supports daily planning across multiple channels without manual rework.

**Scope**: Post-MVP growth capability; not required for the Phase 1 MVP.

**Independent Test**: A single campaign concept can produce tailored outputs for multiple channels with preserved messaging and platform-specific framing.

**Acceptance Scenarios**:

1. **Given** a core idea about audience pain points, **When** the user requests multi-channel adaptation, **Then** each platform output reflects the proper format, depth, and CTA.
2. **Given** a creator uses one idea across blog and short-form video, **When** the system adapts it, **Then** the core message remains consistent while the presentation changes for the platform.

---

### User Story 5 - Collaborate and plan content in a lightweight calendar (Priority: P2)
A small content team shares projects, comments on drafts, and plans publication dates. The system supports simple scheduling and approval workflows while keeping the experience lightweight enough for teams that are not yet ready for full enterprise tooling.

**Why this priority**: Team coordination is a clear growth requirement once the product proves value for individual creators; it expands the product from a solo workflow to a repeatable operating system.

**Scope**: Post-MVP growth capability; not required for the Phase 1 MVP.

**Independent Test**: Users can create a shared project, assign status, and view content scheduled by date and platform.

**Acceptance Scenarios**:

1. **Given** a team shares a campaign in a project, **When** they add comments or approval notes, **Then** the items are visible to the team without leaving the workspace.
2. **Given** a planned content calendar, **When** the user adds dates and statuses, **Then** the timeline reflects content by platform and publishing date.

---

### Edge Cases

- What happens when a user uploads style samples? Zero samples use generic fallback; 1–2 samples create a limited profile with a warning; 3–10 samples enable full style learning; more than 10 samples are rejected.
- How does the system handle unsupported or mismatched formats? It must reject invalid platform selections with a clear error or offer the closest supported template.
- What happens when idea generation requests a platform and goal combination that are weakly aligned? The system must still produce ideas but flag the misalignment in the output rationale where appropriate.
- How does the system handle export failures for download or copy actions? It must show a clear error and keep the project content available without data loss.
- What happens when a project has multiple script variants or status changes? The system must preserve the project history and latest approved version without overwriting the original draft.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST accept a creator brief including niche, audience, platform, content goal, and success metric; historical performance context MAY be supplied and its absence MUST be recorded explicitly.
- **FR-002**: System MUST generate a list of content ideas with titles, hooks, and brief descriptions based on the brief.
- **FR-003**: System MUST support long-form YouTube script generation and short-form TikTok/Reels/Shorts script generation.
 - **FR-003**: System MUST support long-form YouTube script generation, short-form TikTok/Reels/Shorts script generation, and Podcast episode output generation.
- **FR-004**: System MUST allow users to upload 0–10 existing content samples to establish style learning:
	• 0 samples: generic style fallback
    • 1–2 samples: limited style profile with warning
    • 3–10 samples: full style-learning profile
    • More than 10: reject or require a separate profile
- **FR-005**: System MUST learn and apply tone, pacing, vocabulary, and structural patterns from uploaded samples when generating new content.
- **FR-006**: System MUST provide platform-aware output templates for the canonical platforms YouTube, TikTok, Instagram, Podcast, Blog, and Newsletter, with short-form variants for Reels and Shorts.
- **FR-007**: System MUST provide a simple dashboard that displays ideas, scripts, and project status.
- **FR-008**: System MUST support project-level organization for campaigns or series.
- **FR-009**: System MUST track each project item status as draft, in review, or ready.
- **FR-010**: System MUST allow users to copy or download generated content in a simple export format.
- **FR-011**: System MUST preserve the creator's strategic narrative across generated ideas and scripts while adapting output to the selected platform.
- **FR-012**: System MUST provide a shared workspace in which project content, uploaded style samples, ideas, and scripts are available to workspace users.
- **FR-013**: System MUST persist one canonical Workspace record and associate all MVP content and generation requests with its identifier.
- **FR-014**: System MUST use Microsoft Foundry/Azure OpenAI for AI generation and style-learning workflows.
- **FR-015**: System MUST accept common document uploads for style learning, including plain text, Markdown, Word, and PDF files, and MUST validate unsupported types or invalid files with a clear error.
- **FR-016**: System MUST provide copy-to-clipboard, Markdown download, and DOCX download export options.
- **FR-017**: System MUST enforce compliance with the project constitution by prioritizing creator goals, platform fit, data-backed decisions where available, brand consistency, and production-ready output.
- **FR-018**: System MUST evaluate generated scripts against a review rubric covering audience fit, platform format, hook clarity, tone, pacing, vocabulary, structure, and production readiness, and MUST expose the evaluation result for user review.
- **FR-019**: A script version MUST remain ineligible for `ready` while its review is `pending` or `failed`; only a `passed` review for the current version permits the `ready` transition.

The following requirements are deferred to post-MVP phases: multi-channel adaptation, persona management, analytics import and recommendations, collaboration comments and approvals, content calendar scheduling, advanced research, brand governance rules, workflow automation, API access, role-based access, and audit logging.

### Key Entities *(include if feature involves data)*

- **Workspace**: Represents the canonical shared MVP boundary. It owns projects, content, style profiles, and generation requests and provides the stable ownership key for future user and tenant isolation.
- **Project**: Represents a campaign or content series. It contains ideas, scripts, statuses, and publication dates.
 - **Idea**: Represents a proposed content concept. It includes title, hook, description, platform fit, associated goal, success metric, and historical context availability.
- **Script**: Represents generated content in a chosen format such as YouTube long-form or short-form social video. It includes content body, audience fit, platform, and status.
- **Podcast Output**: Represents an audio-oriented content format with a podcast-specific structure and production metadata; podcast is a canonical platform in the shared MVP platform vocabulary.
- **Style Profile**: Represents the creator's learned tone, vocabulary, pacing, and structure derived from uploaded samples.
- **Persona**: Represents a target audience segment with demographics, psychographics, pain points, and communication preferences in Phase 2.
- **Content Calendar Entry**: Represents a scheduled content item with date, platform, status, and associated project or idea in Phase 2.

## Success Criteria *(mandatory)*

### Measurable Outcomes

 - **SC-001 (Phase 1)**: In a validation run of at least 20 complete requests, at least 95% of idea-generation requests return a usable result within 3 minutes. A usable result contains at least 3 persisted ideas, each with a non-empty title, hook, description, niche, audience, goal, canonical platform, and completed request status.
- **SC-002 (Phase 1)**: In a validation run with at least 5 participants, at least 80% can create, edit, and export one output for each required MVP format without facilitator assistance or an external writing tool.
  - **SC-002 (Phase 1)**: In a validation run with at least 5 participants, at least 80% can create, edit, and export one output for each required MVP format without facilitator assistance or an external writing tool. Required MVP formats: YouTube (long-form), TikTok (short-form), Instagram Reels, YouTube Shorts, Podcast, Blog, Newsletter.
 - **SC-003 (Phase 1)**: In a blinded review of at least 10 generated scripts, at least 80% pass the complete style rubric for audience fit, platform format, hook clarity, tone, pacing, vocabulary, structure, and production readiness with only light revision.
- **SC-004 (Phase 1)**: At least 80% of MVP validation participants complete the project → ideas → script → export flow without facilitator assistance.
- **SC-005 (Phase 2)**: In at least 5 collaboration scenarios, 100% of comments, approvals, and status history remain attached to the correct content; unauthorized workspace reads are rejected; and concurrent edits return a conflict response without overwriting content.
- **SC-006 (Cross-phase)**: The product supports phased expansion from MVP to growth and enterprise without major re-architecture of the core content model.

## Assumptions

- Users have a basic content brief and can provide niche, audience, platform, and goal information without needing extensive onboarding.
- The MVP is designed for solo creators and small teams rather than large enterprise governance workflows.
- Style learning is based on uploaded sample content and not on deep proprietary behavioural data unless the product later adds analytics integration.
- Export capabilities focus on lightweight copy and download workflows for the MVP, with more advanced document features added later if demand requires it.
- Existing app infrastructure can support a Blazor dashboard and a simple project data model with a path to multi-channel planning and collaboration.
