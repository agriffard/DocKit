---
title: Usage
group: Guides
order: 1
description: How to wrap content with FeatureGate.
---

# Usage

The `FeatureGate` component conditionally renders content based on named boolean flags. It reacts in real time when flags change, making it ideal for live demos and progressive feature rollouts.

## Basic Usage

Wrap any content with `<FeatureGate>` and provide a `Flag` parameter:

```razor
<FeatureGate Flag="new-dashboard">
    <ChildContent>
        <NewDashboard />
    </ChildContent>
</FeatureGate>
```

When `new-dashboard` is enabled, `<NewDashboard />` is rendered. Otherwise, nothing is rendered.

## Showing a Fallback

Use the `Disabled` render fragment to show fallback content when the flag is off:

```razor
<FeatureGate Flag="new-dashboard">
    <ChildContent>
        <NewDashboard />
    </ChildContent>
    <Disabled>
        <p>The new dashboard is coming soon! 🚀</p>
    </Disabled>
</FeatureGate>
```

## Toggling Flags at Runtime

Inject `FeatureFlagService` and call `Toggle`, `SetFlag`, or check `IsEnabled`:

```csharp
@inject FeatureFlagService Flags

<button @onclick="() => Flags.Toggle("new-dashboard")">
    Toggle feature
</button>

<p>Enabled: @Flags.IsEnabled("new-dashboard")</p>
```

> **Tip:** `FeatureGate` subscribes to `FeatureFlagService.OnChange` so it re-renders automatically — no manual `StateHasChanged` needed.

## Component Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Flag` | `string` | ✅ | Name of the feature flag |
| `ChildContent` | `RenderFragment` | ✅ | Content shown when flag is enabled |
| `Disabled` | `RenderFragment?` | ❌ | Content shown when flag is disabled |

## Example: A/B Testing

You can use multiple gates in a single page to toggle between two variants:

```razor
<FeatureGate Flag="variant-b">
    <ChildContent>
        <VariantB />
    </ChildContent>
    <Disabled>
        <VariantA />
    </Disabled>
</FeatureGate>
```

## Live Demo

Visit the [Demo](/demo) page to see `FeatureGate` in action with a toggle button.

---

See also: [Configuration](configuration.md) for setting up flag sources.
