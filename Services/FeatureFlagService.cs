namespace DocKit.Services;

public class FeatureFlagService
{
    private readonly Dictionary<string, bool> _flags = new(StringComparer.OrdinalIgnoreCase);

    public event Action? OnChange;

    public bool IsEnabled(string flag)
        => _flags.TryGetValue(flag, out var val) && val;

    public void SetFlag(string flag, bool enabled)
    {
        _flags[flag] = enabled;
        OnChange?.Invoke();
    }

    public void Toggle(string flag)
        => SetFlag(flag, !IsEnabled(flag));
}
