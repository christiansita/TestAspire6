# Quickstart Validation: AI Content Brain Roadmap

## Prerequisites

- .NET 10 SDK and Visual Studio 2026
- Aspire tooling and an OCI container runtime
- Azure CLI authenticated to the target tenant for Foundry and production PostgreSQL validation
- A Foundry project endpoint and an accessible model deployment configured through environment variables

## Azure CLI authentication

Run `az login` locally and select the subscription/tenant that contains the Foundry project. The application uses `DefaultAzureCredential`, so the local process obtains Foundry tokens from the Azure CLI login. In Azure Container Apps, the API uses its assigned managed identity instead.

## Local workflow

1. Start the Aspire AppHost from the repository root with `aspire start --non-interactive`.
2. Wait for the Web, API, and PostgreSQL resources to report healthy in the Aspire dashboard.
3. Open the Blazor Web App from the dashboard.
4. Verify the single canonical shared Workspace is available, then create a project named `Fitness Growth Test`.
5. Submit a brief with niche `fitness`, audience `busy beginners`, platform `YouTube`, goal `growth`, and success metric `average watch time`.
6. Optionally provide historical context such as recent average watch time and click-through rate; when omitted, verify the request records historical context as unavailable (`historicalContext.availability = "unavailable"`) and shows the creator-context-only notice.
7. Verify that the idea list contains at least 3 persisted ideas, each with a title, hook, description, niche, audience, goal, success metric, canonical platform, historical-context availability, and completed generation status.
8. Verify style cardinality behavior: zero samples use generic fallback, 1–2 samples show a limited-profile warning, 3–10 samples produce a full profile, and an 11th sample is rejected.
9. Upload valid TXT, Markdown, DOCX, and PDF samples and verify that each reaches a clean/processed state.
10. Generate a YouTube long-form script and verify that it has a hook, structure, body, and call to action.
11. Generate short-form TikTok, Instagram Reels, YouTube Shorts, and Podcast outputs and verify canonical platform plus format-specific formatting; Podcast output must include an episode title, introduction, segment outline, speaker-ready content, and closing call to action.
12. Run the style review for the current script version and verify all rubric results are visible.
13. Verify that `pending` or `failed` review blocks `ready`, while a `passed` review for the current version permits `ready`; edit the script and verify the new version requires a new review.
14. Copy the script to the clipboard, download Markdown, and download DOCX; verify that each export opens and contains the current version.

## Failure checks

- Submit an incomplete brief and verify a clear validation prompt.
- Submit a brief without a success metric and verify validation blocks generation; omit historical context from an otherwise complete brief and verify the explicit unavailable-context notice.
- Upload an unsupported, oversized, malformed, or malware-detected file and verify rejection without changing the project.
- Cancel an in-progress generation and verify that no incomplete script is saved as final.
- Simulate a Foundry timeout and verify an actionable error, request status `failed`, and preserved existing assets.
- Simulate a PostgreSQL interruption and verify that the UI reports failure without losing already persisted content.

## MVP performance and usability validation

- Execute at least 20 complete idea-generation requests and record duration; at least 95% must complete within 3 minutes.
- Observe at least 5 representative users or facilitators completing the project → ideas → script → export flow; at least 80% must complete without facilitator assistance, while errors and abandonment are recorded.
- Blindly review at least 10 generated scripts against the style rubric; at least 80% must pass with only light revision.

## Acceptance evidence

Capture the request ID, final status, generated asset ID/version, and export result for each scenario. The MVP is ready for user validation when the complete project → brief → ideas → style samples → script → edit/status → export flow succeeds for the supported formats and failure checks do not cause data loss.

## Azure Container Apps deployment validation

1. Publish the Aspire application to Azure Container Apps using the repository's deployment workflow.
2. Confirm the Web and API containers are healthy and reachable through the generated endpoints.
3. Confirm the API managed identity has the required Microsoft Foundry project and Azure PostgreSQL permissions.
4. Confirm the API connects to Azure PostgreSQL without a database password.
5. Repeat the end-to-end smoke flow and inspect Aspire/Azure logs for correlation IDs and failed dependency calls.

## Phase 2 Growth validation

1. Create two personas with different pain points and generate variants from one core idea for YouTube, TikTok, Instagram, Podcast, blog, and newsletter.
2. Verify every variant retains the same core idea lineage while using the correct platform template and persona context.
3. Import the same analytics batch twice and verify that normalized metrics are not duplicated.
4. Add a project member, comment on a script, record an approval decision, and verify that unauthorized workspace access is rejected.
5. Schedule content on the calendar by date, platform, and status without triggering external publishing.

## Phase 3 Pro / Enterprise validation

1. Create a tenant brand policy with a banned phrase and required claim, then verify deterministic compliance checks block a violating ready/publish transition.
2. Run research for a topic and verify each finding contains source URL, retrieval time, evidence, and confidence metadata.
3. Configure an automation connection and simulate a timeout/retry; verify idempotency prevents duplicate external jobs.
4. Create an API client with limited permissions and verify tenant isolation, rate limits, revocation, and audit events.
5. Verify role changes, policy changes, integration changes, and publish decisions produce immutable audit records.
