---
title: Getting Started
group: ""
order: 2
description: Install and configure DocKit in minutes.
---

# Getting Started

This guide walks you through installing the DocKit template and creating your first documentation site.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Git (for GitHub Pages deployment)
- A GitHub repository (optional, for deployment)

## Installation

Install the DocKit template from NuGet:

```bash
dotnet new install DocKit.Template
```

## Creating a New Project

Scaffold a new DocKit site:

```bash
dotnet new dockit -n MyDocs
cd MyDocs
```

### Available Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `--RepoName` | string | `""` | GitHub repo name for base-href |
| `--IncludeDemo` | bool | `true` | Include FeatureGate demo page |
| `--Theme` | choice | `auto` | Default theme: `auto`, `light`, `dark` |
| `--IncludeWorkflow` | bool | `true` | Include GitHub Actions workflow |

Example with options:

```bash
dotnet new dockit -n MyDocs --RepoName my-repo --Theme auto
```

## Running Locally

```bash
dotnet run
```

Open your browser at `https://localhost:5001`. You should see the DocKit home page.

## Adding Documentation

Create a new Markdown file in `wwwroot/docs/`:

```markdown
---
title: My New Page
group: Guides
order: 3
description: A brief description for nav tooltips.
---

# My New Page

Content goes here.
```

> **Tip:** After adding a file, run `dotnet build` to regenerate `manifest.json` — the new page will appear in the navigation automatically.

## Project Structure

```
MyDocs/
├─ wwwroot/docs/       ← Your Markdown files live here
├─ Pages/              ← Blazor pages (Home, Docs, Demo)
├─ Components/         ← MarkdownView, TableOfContents, FeatureGate
├─ Services/           ← DocsManifestService, MarkdownService
└─ Build/              ← MSBuild manifest generator
```

## Next Steps

- See [Usage](guides/usage.md) to learn how to use the `FeatureGate` component.
- See [Configuration](guides/configuration.md) to configure feature flags and theming.
