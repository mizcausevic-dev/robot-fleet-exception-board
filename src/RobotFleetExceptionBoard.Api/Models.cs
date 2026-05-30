namespace RobotFleetExceptionBoard.Api;

public sealed record RobotUnit(
    string Id,
    string Fleet,
    string Mission,
    string Site,
    string Status,
    int HealthScore,
    string LastFailure,
    string NextAction,
    IReadOnlyList<string> Risks);

public sealed record MissionFailure(
    string Id,
    string UnitId,
    string Code,
    string Severity,
    string Summary,
    string Owner,
    string Blocker,
    string RecommendedAction);

public sealed record OverridePacket(
    string PacketId,
    string UnitId,
    string MissionWindow,
    int CompletenessPercent,
    string Status,
    string MissingEvidence,
    string Owner,
    string SignoffEta);

public sealed record FleetLane(
    string Id,
    string Lane,
    string Owner,
    string Status,
    string Focus,
    string NextAction,
    string Note);

public sealed record RobotFleetExceptionExport(
    IReadOnlyList<RobotUnit> Units,
    IReadOnlyList<MissionFailure> Failures,
    IReadOnlyList<OverridePacket> Overrides);

public sealed record RobotFleetExceptionFinding(
    string Code,
    string Severity,
    string Owner,
    string Summary,
    string RecommendedAction);

public sealed record RobotFleetExceptionPostureReport(
    int Units,
    int ActiveFailures,
    int CriticalFailures,
    int OverrideRisks,
    int MissionRisks,
    string Recommendation,
    IReadOnlyList<RobotFleetExceptionFinding> Findings);
