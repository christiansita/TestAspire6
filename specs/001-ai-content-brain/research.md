# Research: AI Content Brain Roadmap

## Decisions

### .NET and Aspire composition
- **Decision**: Use a .NET 10 Aspire distributed application with separate Web, API, Application, Domain, Infrastructure, AppHost, and ServiceDefaults projects.
- **Rationale**: The requested Blazor UI, ASP.NET Core API, PostgreSQL dependency, health checks, logs, and local/cloud composition map directly to this boundary. The repository is currently a Spec Kit scaffold, so implementation must create the application structure from scratch.
- **Alternatives considered**: A single Blazor project was rejected because it would couple UI, API, persistence, and AI provider concerns. A separate agent process is deferred until scaling or deployment boundaries require it.

### Microsoft Foundry and Agent Framework
- **Decision**: Use Microsoft Agent Framework for focused generation components and explicit sequential orchestration only where multiple steps are needed. Use `AIProjectClient`/Foundry project endpoints rather than legacy persistent-agent clients or a resource-level `AzureOpenAIClient`.
- **Rationale**: The workflow has explicit stages—brief validation, ideation, style application, platform formatting, and script drafting. Strongly typed intermediate results and structured output reduce malformed persistence. Microsoft guidance recommends workflows for defined multi-step processes and agents for open-ended generation.
- **Alternatives considered**: A single unstructured prompt was rejected because it weakens validation and platform consistency. A large autonomous multi-agent graph was rejected for MVP complexity.

### Model selection
- **Decision**: Configure the model as an environment-controlled Foundry deployment name. Default the MVP recommendation to a stable GPT-4.1-class deployment for responsive interactive drafting, with the deployment validated in the target Foundry project before implementation. Use a separate embedding deployment only if semantic style retrieval is implemented.
- **Rationale**: Microsoft guidance distinguishes GPT-4.1 for low-latency, high-throughput user-facing workloads from GPT-5 for deeper reasoning. The Foundry catalog documents structured outputs and large context for GPT-4.1. The authenticated Foundry catalog could not be queried in this environment, so subscription availability remains a deployment prerequisite rather than an asserted fact.
- **Alternatives considered**: GPT-5-class models are suitable for complex research but add latency and are unnecessary for the defined MVP. Preview models are avoided for production readiness.

### PostgreSQL and identity
- **Decision**: Use EF Core/Npgsql with PostgreSQL. Use an Aspire PostgreSQL container locally. In Azure, use Azure Database for PostgreSQL Flexible Server with Microsoft Entra authentication and managed identity for the API. Keep local password-based development credentials in Aspire/.NET user secrets only.
- **Rationale**: PostgreSQL matches the requested data model and has a supported .NET driver. Microsoft documents token-based PostgreSQL authentication and recommends managed identities/passwordless access for Azure applications.
- **Alternatives considered**: Storing generated content in files was rejected because projects, versions, and statuses require relational queries. Production database passwords are rejected because they increase secret-management risk.

### Aspire deployment target
- **Decision**: Standardize the MVP cloud deployment on Azure Container Apps. Use Aspire's deployment/publishing model for the Web and API services, an Azure PostgreSQL resource for persistence, and managed identity permissions for PostgreSQL and Foundry access.
- **Rationale**: Container Apps preserves the distributed service composition used locally, supports independently deployable Web/API containers, and avoids maintaining separate App Service deployment paths for the MVP. Aspire remains the source of truth for service references, health checks, logs, and environment wiring.
- **Alternatives considered**: Azure App Service was rejected as the standardized target for this phase because the requested composition is service-oriented and already modeled around containers. Supporting both targets is deferred to avoid duplicating deployment validation.

### Local Foundry authentication
- **Decision**: Use `DefaultAzureCredential` locally with the developer's Azure CLI login (`az login`). Do not store Foundry keys in source control or require local API-key configuration for the default workflow.
- **Rationale**: This aligns local development with the production managed-identity path and reduces credential drift. The developer must have access to the Foundry project and model deployment.
- **Alternatives considered**: Environment-only keys and committed configuration are rejected. Aspire secrets remain available for non-identity local values such as database settings, but are not the default Foundry authentication mechanism.

### Style sample uploads
- **Decision**: Accept text, Markdown, DOCX, and PDF in MVP, with server-side extension/content validation, size limits, generated storage names, quarantine status, and malware scanning before extraction. Support zero to 10 samples: zero uses generic fallback, 1–2 produce a limited profile warning, 3–10 enable full learning, and more than 10 are rejected. Persist extracted text and a style profile summary; do not persist raw files in the relational database unless required by the implementation.
- **Rationale**: Microsoft upload guidance requires server-side validation, safe names, size limits, and malware scanning. Extracted text is sufficient for MVP style analysis and keeps the data model focused.
- **Alternatives considered**: Accepting arbitrary files was rejected due to security and extraction reliability. Embedding vectors are optional and deferred unless retrieval quality requires them.

### Workspace identity and API contracts
- **Decision**: Persist one canonical Workspace row in Phase 1 and include its identifier in all content and generation ownership paths. Use a dependency-free `ContentBrain.Contracts` project for shared HTTP DTOs and validation models; API and Web reference it while Domain and Application remain independent.
- **Rationale**: A stable ownership key avoids a later data migration while honoring the shared-workspace MVP. A dedicated contract boundary prevents Web/API coupling and supports future versioned external consumers.

### MVP research-first context
- **Decision**: Apply the constitution's research-first operating model in MVP through three structured inputs: the creator brief requires audience, objective, platform, and success metric; style samples provide historical creator evidence; and historical performance context is optional input when supplied by the creator. When unavailable, the system records that absence and shows an explicit creator-context-only notice.
- **Rationale**: MVP does not require public-web research, but it must not generate without structured creator context and a success metric. The Phase 3 research agent adds external source attribution later.

### MVP quality evaluation
- **Decision**: Generate a structured style review for each script covering audience fit, platform format, hook clarity, tone, pacing, vocabulary, structure, and production readiness. Store the review with the script version and display it before the ready transition.
- **Rationale**: “Light revision” is otherwise subjective and cannot validate the constitution's production-ready and platform-centered principles.

### Research source controls
- **Decision**: Phase 3 research must use bounded source policies, retain URL, retrieval timestamp, query, evidence, and confidence metadata, and mark findings as unreviewed until creator/team approval.
- **Rationale**: Public web content is mutable and potentially unreliable. Provenance and review prevent unsupported research from silently becoming production content.
- **Alternatives considered**: Unattributed web summaries were rejected because they cannot be validated or audited.

### Exports
- **Decision**: Implement clipboard copy, Markdown download, and DOCX download behind an export service.
- **Rationale**: These are the clarified MVP outputs and keep formatting logic independent from the UI.
- **Alternatives considered**: PDF export is deferred because it was not selected for MVP.

## Phase 2 Growth Decisions

### Multi-channel plans and personas
- **Decision**: Model one core idea with related platform variants and attach optional persona context to each generation request.
- **Rationale**: A shared core identifier preserves narrative consistency while allowing each platform variant to use its own template, hook, CTA, and length. Persona context belongs to the request and output, not to the global style profile.
- **Alternatives considered**: Copying independent ideas was rejected because it loses narrative lineage and makes consistency validation difficult.

### Analytics imports
- **Decision**: Use provider-specific import adapters that normalize views, watch time, CTR, publication date, and source identifiers into an append-only metric model. Imports must be idempotent.
- **Rationale**: Normalized metrics let recommendation logic remain provider-neutral and prevent duplicate imports from distorting decisions.
- **Alternatives considered**: Direct provider-shaped columns were rejected because they make additional platforms and schema evolution expensive.

### Collaboration and calendar
- **Decision**: Use workspace membership, comments, approval transitions, optimistic concurrency, and calendar entries as separate bounded capabilities.
- **Rationale**: This preserves the lightweight MVP workflow while allowing teams to add review and scheduling context without changing content entities.

## Phase 3 Pro / Enterprise Decisions

### Research and provenance
- **Decision**: Every research result must retain source URL, retrieval time, query or topic, and confidence/quality metadata. Research suggestions remain drafts until creator or team review.
- **Rationale**: Public research changes over time and must be explainable before it influences production content.

### Brand governance
- **Decision**: Store versioned brand policies and evaluate generated content before a ready/publish transition. Banned phrases and required claims are deterministic checks; AI review is advisory and produces evidence.
- **Rationale**: Deterministic policies provide predictable compliance behavior while AI can handle nuanced review.

### Enterprise access and integrations
- **Decision**: Introduce tenant-scoped RBAC, immutable audit events, versioned API contracts, scoped API clients, idempotent automation, and approval gates before external publishing.
- **Rationale**: Enterprise operations require least privilege, traceability, and safe retries across external systems.

## Sources

- Microsoft Agent Framework overview and workflows: https://learn.microsoft.com/agent-framework/overview/ and https://learn.microsoft.com/agent-framework/workflows/agents-in-workflows
- Foundry SDKs and endpoints: https://learn.microsoft.com/azure/foundry/how-to/develop/sdk-overview#agent-framework
- Azure model catalog: https://learn.microsoft.com/azure/ai-foundry/foundry-models/concepts/models-sold-directly-by-azure
- Model choice guide: https://learn.microsoft.com/azure/ai-foundry/foundry-models/how-to/model-choice-guide
- Azure PostgreSQL .NET connectivity: https://learn.microsoft.com/azure/postgresql/connectivity/connect-csharp
- PostgreSQL managed identity authentication: https://learn.microsoft.com/azure/postgresql/security/security-connect-with-managed-identity
- ASP.NET Core upload validation: https://learn.microsoft.com/aspnet/core/mvc/models/file-uploads?view=aspnetcore-10.0#validation
- Blazor downloads: https://learn.microsoft.com/aspnet/core/blazor/file-downloads?view=aspnetcore-10.0
