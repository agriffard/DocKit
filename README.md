# DocKit

A lightweight documentation-site template built with **Blazor WebAssembly** (.NET 10). Write your docs as Markdown files, and DocKit renders them into a fast, themeable, fully client-side static site — ready to deploy to GitHub Pages.

## Features

- **Markdown-driven** — drop `.md` files in `wwwroot/docs/`; navigation and routes are generated automatically.
- **Build-time manifest** — an MSBuild task scans the docs folder and emits `manifest.json` (titles, groups, ordering) from YAML front-matter. No manual index to maintain.
- **Markdown rendering** — [Markdig](https://github.com/xoofx/markdig) with advanced extensions, YAML front-matter, and auto heading IDs. External links get `target="_blank" rel="noopener noreferrer"`; relative `.md` links are rewritten to in-app routes.
- **Responsive nav** — sidebar on desktop, slide-in drawer with hamburger trigger on mobile.
- **Light/dark theme** — toggle persisted to `localStorage`; respects `prefers-color-scheme` by default.
- **Feature flags** — `FeatureFlagService` + `<FeatureGate>` component to show/hide content at runtime, with a live demo page.
- **Syntax highlighting** — via JS interop (`highlight-interop.js`).
- **GitHub Pages deploy** — included GitHub Actions workflow.

## Project Structure

```
DocKit/
├─ Program.cs                  ← DI registration, HttpClient setup
├─ App.razor                   ← Router
├─ Layout/
│  ├─ MainLayout.razor         ← Shell: nav drawer, theme toggle
│  └─ NavMenu.razor            ← Sidebar, built from manifest
├─ Pages/
│  ├─ Home.razor               ← Landing page
│  ├─ Docs.razor               ← Renders a doc by slug (/docs/{slug})
│  ├─ Demo.razor               ← FeatureGate live demo
│  └─ NotFound.razor
├─ Components/
│  ├─ MarkdownView.razor       ← Renders Markdown → HTML
│  ├─ TableOfContents.razor    ← Per-page heading nav
│  └─ FeatureGate.razor        ← Conditional render by feature flag
├─ Services/
│  ├─ DocsManifestService.cs   ← Loads manifest.json (scoped, uses HttpClient)
│  ├─ MarkdownService.cs       ← Markdig pipeline + link rewriting
│  └─ FeatureFlagService.cs    ← Runtime feature flags
├─ Build/
│  └─ GenerateDocsManifest.targets  ← MSBuild task: docs → manifest.json
└─ wwwroot/
   ├─ docs/                    ← Your Markdown content lives here
   │  ├─ *.md
   │  └─ manifest.json         ← Generated at build (do not edit by hand)
   ├─ css/site.css
   ├─ js/highlight-interop.js
   └─ index.html
```

## How It Works

1. **Build time** — `GenerateDocsManifest.targets` runs before build. It walks `wwwroot/docs/**/*.md`, reads each file's YAML front-matter, and writes `wwwroot/docs/manifest.json`.
2. **Run time** — `DocsManifestService` fetches `manifest.json` once and caches it. `NavMenu` groups/sorts entries to build the sidebar.
3. **Page view** — `Docs.razor` matches the route slug, fetches the matching `.md` file, and `MarkdownService` renders it to HTML inside `MarkdownView`.

## Adding a Doc

Create a Markdown file under `wwwroot/docs/` with front-matter:

```markdown
---
title: My Page
group: Guides
order: 3
description: Short summary shown in listings.
---

# My Page

Content here...
```

| Field         | Purpose                                              |
|---------------|------------------------------------------------------|
| `title`       | Display name in nav (falls back to slug).            |
| `group`       | Sidebar section heading (empty = top-level).         |
| `order`       | Sort order within its group.                         |
| `description` | Summary used in listings.                            |

The slug is the file path relative to `wwwroot/docs/`, minus `.md` (e.g. `guides/usage.md` → `/docs/guides/usage`). Rebuild to regenerate the manifest.

## Running Locally

```bash
dotnet run
```

Then open the URL shown (typically `https://localhost:5001`).

## Building

```bash
dotnet build
dotnet publish -c Release
```

Published static output lands in `bin/Release/net10.0/publish/wwwroot/`.

## Deploying to GitHub Pages

The included `.github/workflows/deploy.yml` publishes the app on push. Set the `RepoName` so the base-href matches your project-pages path (`/<RepoName>/`).

## Using as a Template

DocKit ships as a `dotnet new` template (`.template.config/template.json`).

```bash
dotnet new install .
dotnet new dockit -n MyDocs
```

Template parameters:

| Parameter         | Type   | Default | Description                                      |
|-------------------|--------|---------|--------------------------------------------------|
| `RepoName`        | string | `""`    | GitHub repo name, used for base-href.            |
| `IncludeDemo`     | bool   | `true`  | Include the FeatureGate live demo page.          |
| `Theme`           | choice | `auto`  | Default theme: `auto`, `light`, or `dark`.       |
| `IncludeWorkflow` | bool   | `true`  | Include the GitHub Actions deploy workflow.      |

## Tech Stack

- .NET 10 — Blazor WebAssembly
- Markdig — Markdown processing
- MSBuild RoslynCodeTaskFactory — build-time manifest generation
