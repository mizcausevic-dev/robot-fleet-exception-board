namespace RobotFleetExceptionBoard.Api;

public static class AnalysisService
{
    public static RobotFleetExceptionPostureReport Analyze(RobotFleetExceptionExport payload)
    {
        var findings = new List<RobotFleetExceptionFinding>();

        foreach (var failure in payload.Failures)
        {
            findings.Add(new RobotFleetExceptionFinding(
                FailureCode(failure.Code),
                failure.Severity,
                failure.Owner,
                failure.Summary,
                failure.RecommendedAction));
        }

        foreach (var packet in payload.Overrides.Where(packet => packet.CompletenessPercent < 80))
        {
            findings.Add(new RobotFleetExceptionFinding(
                "override-packet-gap",
                packet.Status == "red" ? "high" : "medium",
                packet.Owner,
                $"{packet.UnitId} override packet is only {packet.CompletenessPercent}% complete.",
                $"Close the missing evidence: {packet.MissingEvidence}"));
        }

        var criticalFailures = payload.Failures.Count(f => f.Severity == "critical");
        var activeFailures = payload.Failures.Count;
        var overrideRisks = payload.Overrides.Count(packet => packet.Status != "green");
        var missionRisks = payload.Units.Count(unit => unit.Status != "green");

        return new RobotFleetExceptionPostureReport(
            payload.Units.Count,
            activeFailures,
            criticalFailures,
            overrideRisks,
            missionRisks,
            "Repair localization drift, vision timeouts, and override-signoff gaps before another autonomous mission window opens.",
            findings);
    }

    public static object Summary()
    {
        var report = Analyze(SampleData.Payload);
        return new
        {
            units = report.Units,
            activeFailures = report.ActiveFailures,
            criticalFailures = report.CriticalFailures,
            overrideRisks = report.OverrideRisks,
            missionRisks = report.MissionRisks,
            recommendation = report.Recommendation
        };
    }

    private static string FailureCode(string code) => code switch
    {
        "LocalizationDrift" => "localization-drift",
        "VisionTimeout" => "vision-timeout",
        "BatterySag" => "battery-sag",
        "SafetyReviewGap" => "safety-review-gap",
        "IncidentNoteMissing" => "incident-note-missing",
        "ReplayGap" => "mission-replay-gap",
        _ => "robot-fleet-gap"
    };
}
