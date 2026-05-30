using System.Text.Json;
using RobotFleetExceptionBoard.Api;

var app = RobotFleetExceptionBoardApplication.BuildApp(args);

if (args.Contains("--prerender"))
{
    await SiteBuilder.WriteAsync();
    return;
}

if (args.Contains("--demo"))
{
    Console.WriteLine(JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine(JsonSerializer.Serialize(SampleData.FleetLanes, new JsonSerializerOptions { WriteIndented = true }));
    return;
}

app.Run();

public partial class Program;

public static class RobotFleetExceptionBoardApplication
{
    public static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => Results.Content(RenderService.Overview(), "text/html"));
        app.MapGet("/fleet-lane", () => Results.Content(RenderService.FleetLane(), "text/html"));
        app.MapGet("/mission-failures", () => Results.Content(RenderService.MissionFailures(), "text/html"));
        app.MapGet("/override-posture", () => Results.Content(RenderService.OverridePosture(), "text/html"));
        app.MapGet("/verification", () => Results.Content(RenderService.Verification(), "text/html"));
        app.MapGet("/docs", () => Results.Content(RenderService.Docs(), "text/html"));

        app.MapGet("/api/dashboard/summary", () => Results.Json(AnalysisService.Summary()));
        app.MapGet("/api/fleet-lane", () => Results.Json(SampleData.FleetLanes));
        app.MapGet("/api/mission-failures", () => Results.Json(SampleData.Payload.Failures));
        app.MapGet("/api/override-posture", () => Results.Json(SampleData.Payload.Overrides));
        app.MapGet("/api/verification", () => Results.Json(new[]
        {
            "Synthetic robot-fleet mission, override, and failure evidence only; no live autonomy stack, facility, or industrial control data is published.",
            "Fleet Reliability, Autonomy Platform, Safety Governance, Incident Command, and Field Operations are modeled as operator surfaces.",
            "This repo demonstrates robotics and fleet-operations workflow depth, not safety or regulatory certification."
        }));
        app.MapGet("/api/sample", () => Results.Text(RenderService.Sample(), "application/json"));

        return app;
    }
}

public static class SiteBuilder
{
    public static async Task WriteAsync()
    {
        var root = FindRepoRoot();
        var siteDir = Path.Combine(root, "site");
        Directory.CreateDirectory(siteDir);

        var pages = new Dictionary<string, string>
        {
            ["index.html"] = RenderService.Overview(),
            [Path.Combine("fleet-lane", "index.html")] = RenderService.FleetLane(),
            [Path.Combine("mission-failures", "index.html")] = RenderService.MissionFailures(),
            [Path.Combine("override-posture", "index.html")] = RenderService.OverridePosture(),
            [Path.Combine("verification", "index.html")] = RenderService.Verification(),
            [Path.Combine("docs", "index.html")] = RenderService.Docs()
        };

        foreach (var (relative, html) in pages)
        {
            var target = Path.Combine(siteDir, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await File.WriteAllTextAsync(target, html);
        }

        var apiDir = Path.Combine(siteDir, "api");
        Directory.CreateDirectory(Path.Combine(apiDir, "dashboard"));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "dashboard", "summary.json"), JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "fleet-lane.json"), JsonSerializer.Serialize(SampleData.FleetLanes, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "mission-failures.json"), JsonSerializer.Serialize(SampleData.Payload.Failures, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "override-posture.json"), JsonSerializer.Serialize(SampleData.Payload.Overrides, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "verification.json"), JsonSerializer.Serialize(new[]
        {
            "Synthetic robot-fleet mission, override, and failure evidence only; no live autonomy stack, facility, or industrial control data is published.",
            "Fleet Reliability, Autonomy Platform, Safety Governance, Incident Command, and Field Operations are modeled as operator surfaces.",
            "This repo demonstrates robotics and fleet-operations workflow depth, not safety or regulatory certification."
        }, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "sample.json"), RenderService.Sample());

        const string domain = "robots.kineticgain.com";
        await File.WriteAllTextAsync(Path.Combine(siteDir, "robots.txt"), $"User-agent: *{Environment.NewLine}Allow: /{Environment.NewLine}Sitemap: https://{domain}/sitemap.xml{Environment.NewLine}");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "sitemap.xml"), """
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>https://robots.kineticgain.com/</loc></url>
  <url><loc>https://robots.kineticgain.com/fleet-lane/</loc></url>
  <url><loc>https://robots.kineticgain.com/mission-failures/</loc></url>
  <url><loc>https://robots.kineticgain.com/override-posture/</loc></url>
  <url><loc>https://robots.kineticgain.com/verification/</loc></url>
  <url><loc>https://robots.kineticgain.com/docs/</loc></url>
</urlset>
""");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "CNAME"), domain + Environment.NewLine);
    }

    private static string FindRepoRoot()
    {
        var current = AppContext.BaseDirectory;
        for (var i = 0; i < 8; i++)
        {
            if (File.Exists(Path.Combine(current, "robot-fleet-exception-board.sln")))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName
                ?? throw new DirectoryNotFoundException("Unable to resolve repo root.");
        }

        throw new DirectoryNotFoundException("Unable to resolve repo root.");
    }
}
