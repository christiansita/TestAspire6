# AI Content Brain

AI Content Brain is an early-stage content planning and drafting application. Its goal is to help creators move from a content brief to platform-aware ideas and scripts, keep that work organized in projects, and review and export finished assets.

> **Project status:** This repository is a scaffold, not a feature-complete product. The product direction and MVP requirements are documented, but the Web and API entry points currently return placeholder responses, and the AppHost currently prints a service composition rather than starting the services. Treat capabilities described below as planned unless explicitly identified as present in the scaffold.

## What the product is intended to do

The MVP is intended to provide a shared workspace where a creator can:

- Turn a brief (niche, audience, platform, goal, and success metric) into content ideas with titles, hooks, and descriptions.
- Draft long-form YouTube scripts, short-form TikTok/Reels/Shorts scripts, and structured podcast episodes.
- Upload up to 10 examples of their own content to inform a style profile, with a generic fallback for no examples and a limited-profile warning for one or two.
- Organize ideas and scripts into campaign or series projects and track their draft, in-review, and ready status.
- Review generated scripts against a style and production-readiness rubric, then copy or download outputs as Markdown or DOCX.

The product is designed to preserve the creator's strategic message while adapting the structure and presentation for the chosen channel. Historical performance context is optional; when it is not supplied, the intended experience is to make that limitation explicit rather than imply recommendations are performance-data-backed.

### Intended users

The initial audience is **solo content creators and small content teams** who regularly plan and produce content for channels such as YouTube, TikTok, Instagram, podcasts, blogs, and newsletters. The product is meant to support ideation, drafting, and lightweight review—not replace creator judgment or publish content automatically.

The longer-term roadmap describes growth features such as multi-channel repurposing, audience personas, analytics, collaboration, and a content calendar, followed by advanced research, brand governance, automation, API access, role-based access, and audit logs. These are roadmap items, not claims about current functionality.

## Current implementation

The repository currently provides a .NET solution scaffold with separate projects for the web host, API, shared contracts, application/domain layers, infrastructure, and tests. The shared contracts include an idea-generation request DTO and a style-review DTO. The app host lists intended Web, API, and PostgreSQL resources, but its local builder is a lightweight placeholder and does not orchestrate them. The Web and API projects each currently expose a placeholder root response.

The detailed feature specification and validation plan are in [`specs/001-ai-content-brain/spec.md`](specs/001-ai-content-brain/spec.md), [`specs/001-ai-content-brain/plan.md`](specs/001-ai-content-brain/plan.md), and [`specs/001-ai-content-brain/quickstart.md`](specs/001-ai-content-brain/quickstart.md). They describe the target workflows and architecture; they should not be read as proof that those workflows are already implemented.

## Technology stack

### Present in the scaffold

- **C# and .NET 10** for the solution projects.
- **ASP.NET Core** web SDK projects for the current Web and API placeholders.
- **xUnit** package references in the test projects.
- **Entity Framework Core and Npgsql** package references in Infrastructure.
- **Azure.Identity** package references in the API and AppHost.

These dependencies are foundations only. For example, the presence of PostgreSQL and Azure identity packages does not mean persistence or Microsoft Foundry integration is currently wired up.

### Planned target architecture

The implementation plan targets a Blazor Web App, ASP.NET Core Minimal APIs, EF Core with PostgreSQL, Microsoft Agent Framework and Microsoft Foundry for AI workflows, .NET Aspire for local distributed-app orchestration, and Azure Container Apps for deployment. These target components are not all implemented in the current scaffold.

## Repository layout

```text
src/
├── ContentBrain.AppHost/          # Local application composition entry point (currently a stub)
├── ContentBrain.ServiceDefaults/  # Shared service defaults project
├── ContentBrain.Web/              # Web application host (currently a placeholder)
├── ContentBrain.Api/              # HTTP API host (currently a placeholder)
├── ContentBrain.Contracts/        # Shared HTTP contract DTOs
├── ContentBrain.Application/      # Application/use-case layer
├── ContentBrain.Domain/           # Domain layer
└── ContentBrain.Infrastructure/   # Persistence and external-service adapters

tests/
├── ContentBrain.UnitTests/
├── ContentBrain.ApiTests/
└── ContentBrain.IntegrationTests/

specs/001-ai-content-brain/        # Product specification, architecture, and validation notes
```

## Build and inspect the scaffold

Install the .NET 10 SDK, then from the repository root:

```sh
dotnet build TestAspire6.slnx
```

The current AppHost can be run to print the declared service composition:

```sh
dotnet run --project src/ContentBrain.AppHost/ContentBrain.AppHost.csproj
```

This command does **not** start the Web or API projects, launch PostgreSQL, or run an Aspire dashboard. The individual Web and API projects are also placeholders and are not yet usable as the AI Content Brain product.

## Roadmap at a glance

1. **MVP:** shared workspace, project-based ideation and scripts, style samples and review, platform-aware formats, and exports.
2. **Growth:** multi-channel content plans, personas, analytics, team collaboration, and a lightweight calendar.
3. **Pro / Enterprise:** sourced research, brand governance, workflow integrations, public API access, authorization, and auditability.

See the feature specification for requirements and the quickstart document for the planned acceptance and validation scenarios.