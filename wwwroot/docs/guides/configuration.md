---
title: Configuration
group: Guides
order: 2
description: Configuring feature flag sources.
---

# Configuration

DocKit's `FeatureFlagService` is a simple in-memory dictionary by default, but it can be seeded from configuration, environment variables, or any async source.

## Default Setup

`FeatureFlagService` is registered as a singleton in `Program.cs`:

```csharp
builder.Services.AddSingleton<FeatureFlagService>();
```

By default no flags are defined, so all `<FeatureGate>` components render their `Disabled` fragment (or nothing).

## Seeding Flags from JSON

Add a `features.json` file to `wwwroot/`:

```json
{
  "my-feature": true,
  "beta-dashboard": false,
  "new-onboarding": true
}
```

Then load it during app startup in `Program.cs`:

```csharp
var host = builder.Build();

var http = host.Services.GetRequiredService<HttpClient>();
var flags = host.Services.GetRequiredService<FeatureFlagService>();

try
{
    var config = await http.GetFromJsonAsync<Dictionary<string, bool>>("features.json");
    if (config is not null)
        foreach (var (key, value) in config)
            flags.SetFlag(key, value);
}
catch { /* ignore missing features.json */ }

await host.RunAsync();
```

> **Note:** The try/catch ensures the app starts even if `features.json` is missing or malformed.

## Environment-Based Flags

For build-time flags, you can embed values at publish time using MSBuild:

```xml
<PropertyGroup>
  <EnableBetaDashboard Condition="'$(DOTNET_ENVIRONMENT)' == 'Production'">false</EnableBetaDashboard>
  <EnableBetaDashboard Condition="'$(DOTNET_ENVIRONMENT)' != 'Production'">true</EnableBetaDashboard>
</PropertyGroup>
```

Then generate an `appsettings.json` or inject the values via a Blazor `IConfiguration` provider.

## Theme Configuration

The default theme is controlled by the `data-theme` attribute on `<html>`. DocKit reads `localStorage` on startup:

| Value | Behaviour |
|-------|-----------|
| `"light"` | Force light mode |
| `"dark"` | Force dark mode |
| *(not set)* | Follow OS `prefers-color-scheme` |

You can force a default by setting the attribute before Blazor loads in `wwwroot/index.html`:

```html
<script>
  var t = localStorage.getItem('theme');
  if (t) document.documentElement.setAttribute('data-theme', t);
</script>
```

## Highlight.js Configuration

By default DocKit loads highlight.js from the Cloudflare CDN with the `github` theme. To use a different theme, swap the CSS link in `wwwroot/index.html`:

```html
<!-- Replace the highlight.js CSS href with any theme from cdnjs -->
<link rel="stylesheet"
  href="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/styles/atom-one-dark.min.css" />
```

### Registering Additional Languages

highlight.js auto-detects languages. To add a language not bundled in the default build:

```html
<script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/languages/fsharp.min.js"></script>
```

---

See also: [Usage](usage.md) for details on the `FeatureGate` component.

See [Introduction](../introduction.md) for a high-level overview.
