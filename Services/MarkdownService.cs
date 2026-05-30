using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text.RegularExpressions;

namespace DocKit.Services;

public class MarkdownService
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseYamlFrontMatter()
            .UseAutoIdentifiers()
            .Build();
    }

    public string RenderToHtml(string markdown)
    {
        // Strip YAML front-matter
        var content = StripFrontMatter(markdown);

        // Parse and render
        var document = Markdown.Parse(content, _pipeline);

        // Process links
        foreach (var link in document.Descendants<LinkInline>())
        {
            var url = link.Url ?? "";

            if (IsExternalLink(url))
            {
                // Add target="_blank" rel="noopener noreferrer" via HtmlAttributes
                var attrs = link.GetAttributes();
                attrs.AddPropertyIfNotExist("target", "_blank");
                attrs.AddPropertyIfNotExist("rel", "noopener noreferrer");
            }
            else if (url.EndsWith(".md") || url.Contains(".md#"))
            {
                // Rewrite relative .md links to in-app routes
                link.Url = RewriteMarkdownLink(url);
            }
        }

        using var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        _pipeline.Setup(renderer);
        renderer.Render(document);
        return writer.ToString();
    }

    private static string StripFrontMatter(string markdown)
    {
        if (!markdown.StartsWith("---"))
            return markdown;

        var end = markdown.IndexOf("\n---", 3);
        if (end < 0)
            return markdown;

        // Skip past the closing ---\n
        var afterFrontMatter = end + 4; // \n---
        if (afterFrontMatter < markdown.Length && markdown[afterFrontMatter] == '\n')
            afterFrontMatter++;

        return markdown[afterFrontMatter..].TrimStart('\n');
    }

    private static bool IsExternalLink(string url)
    {
        return url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("//");
    }

    private static string RewriteMarkdownLink(string url)
    {
        // Split off fragment
        var fragment = "";
        var hashIdx = url.IndexOf('#');
        if (hashIdx >= 0)
        {
            fragment = url[hashIdx..];
            url = url[..hashIdx];
        }

        // Remove .md extension
        if (url.EndsWith(".md"))
            url = url[..^3];

        // Prefix with docs/ (relative, resolves against <base href>) if not already absolute
        if (!url.StartsWith("/"))
            url = "docs/" + url;

        return url + fragment;
    }
}
