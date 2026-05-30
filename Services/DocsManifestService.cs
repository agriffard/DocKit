using System.Net.Http.Json;

namespace DocKit.Services;

public class DocEntry
{
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string Group { get; set; } = "";
    public int Order { get; set; }
    public string Description { get; set; } = "";
}

public class DocsManifestService
{
    private readonly HttpClient _http;
    private List<DocEntry>? _cache;

    public DocsManifestService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DocEntry>> GetManifestAsync()
    {
        if (_cache is not null)
            return _cache;

        try
        {
            _cache = await _http.GetFromJsonAsync<List<DocEntry>>("docs/manifest.json")
                     ?? new List<DocEntry>();
        }
        catch
        {
            _cache = new List<DocEntry>();
        }

        return _cache;
    }
}
