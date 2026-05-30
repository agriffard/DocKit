---
title: Introduction
group: ""
order: 1
description: What is DocKit and why use it?
---

# Introduction

Welcome to **DocKit** — a minimal, responsive documentation and showcase template for [Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/), ready to deploy to GitHub Pages with zero server-side code.

## What is DocKit?

DocKit is a `dotnet new` template that scaffolds a complete documentation site in seconds. It includes:

- **Markdown-powered docs** with YAML front-matter for metadata
- **Auto-generated navigation** built from a manifest at build time
- **Syntax-highlighted code blocks** via highlight.js
- **Light/Dark theme** toggle with `localStorage` persistence
- **FeatureGate** component for conditional content
- **GitHub Pages deployment** via GitHub Actions

## Why DocKit?

Most documentation frameworks require Node.js, a separate build pipeline, or a server. DocKit runs entirely in the browser using Blazor WebAssembly.

> **No Node. No server. Just `dotnet publish` and deploy.**

### Key Benefits

| Feature | DocKit | Other Tools |
|---------|--------|-------------|
| Runtime | Blazor WASM | Node / Server |
| Template engine | `dotnet new` | npm / yarn |
| Deployment | GitHub Pages | Vercel / Netlify |
| Language | C# | JS / TS |

### Who is it for?

- .NET open-source library authors
- Teams building internal developer portals
- Anyone who wants to showcase a Blazor component

## Architecture

DocKit uses an MSBuild task to scan `wwwroot/docs/**/*.md` at **build time** and emit a `manifest.json`. The Blazor app loads this manifest on startup to build the navigation tree — no server query required.

```
Build time:   .md files → GenerateDocsManifest.targets → manifest.json
Run time:     manifest.json → DocsManifestService → NavMenu
              .md files → HttpClient → MarkdownService → MarkdownView
```

## Next Steps

Ready to get started? Head over to [Getting Started](getting-started.md) to install and configure DocKit.
