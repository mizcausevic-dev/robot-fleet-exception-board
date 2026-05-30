using Microsoft.AspNetCore.Mvc.Testing;
using RobotFleetExceptionBoard.Api;

namespace RobotFleetExceptionBoard.Tests;

public sealed class RobotFleetExceptionBoardTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RobotFleetExceptionBoardTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Overview_route_renders_robotics_shell()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Robot Fleet Exception Board", html);
        Assert.Contains("mission failures", html);
    }

    [Fact]
    public async Task Api_summary_returns_expected_counts()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/dashboard/summary");
        var json = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("\"units\":4", json);
        Assert.Contains("\"criticalFailures\":2", json);
    }

    [Fact]
    public void Analysis_flags_robot_fleet_gaps()
    {
        var report = AnalysisService.Analyze(SampleData.Payload);

        Assert.Equal(4, report.Units);
        Assert.Equal(6, report.ActiveFailures);
        Assert.Contains(report.Findings, finding => finding.Code == "localization-drift");
        Assert.Contains(report.Findings, finding => finding.Code == "override-packet-gap");
    }
}
