using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DocKit;
using DocKit.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddSingleton<DocsManifestService>();
builder.Services.AddSingleton<MarkdownService>();
builder.Services.AddSingleton<FeatureFlagService>();

await builder.Build().RunAsync();
