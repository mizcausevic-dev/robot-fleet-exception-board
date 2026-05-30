namespace RobotFleetExceptionBoard.Api;

public static class SampleData
{
    public static readonly RobotFleetExceptionExport Payload = new(
        new[]
        {
            new RobotUnit(
                "rb-014",
                "Warehouse AMR fleet",
                "Aisle replenishment",
                "DFW-1",
                "red",
                48,
                "Localization drift during a live replenishment mission",
                "Recalibrate lidar frame and close the supervisor override packet before the next night shift.",
                new[] { "localization drift", "manual override open", "safety review waiting" }),
            new RobotUnit(
                "rb-022",
                "Yard inspection fleet",
                "Trailer sweep",
                "ATL-3",
                "yellow",
                67,
                "Battery sag created an incomplete sweep proof set",
                "Close the mission replay and battery telemetry evidence before the next dispatch window.",
                new[] { "battery sag", "mission replay gap" }),
            new RobotUnit(
                "rb-031",
                "Fulfillment pick fleet",
                "Rush order pick",
                "PHX-2",
                "red",
                39,
                "Vision stack timeout forced an operator stop",
                "Repair the camera inference timeout and attach the human override notes before redeploying.",
                new[] { "vision timeout", "human override", "incident note missing" }),
            new RobotUnit(
                "rb-041",
                "Hospital courier fleet",
                "Specimen handoff",
                "BOS-4",
                "green",
                86,
                "No active failure",
                "Hold green posture and keep custody telemetry fresh.",
                new[] { "custody telemetry healthy" })
        },
        new[]
        {
            new MissionFailure(
                "mf-1001",
                "rb-014",
                "LocalizationDrift",
                "critical",
                "Navigation confidence dropped below the floor during a live aisle replenishment mission.",
                "Fleet Reliability",
                "Supervisor override packet is still incomplete.",
                "Re-run localization calibration and attach override reviewer signoff."),
            new MissionFailure(
                "mf-1002",
                "rb-031",
                "VisionTimeout",
                "critical",
                "Primary camera inference stalled and the robot entered a human-stop state.",
                "Autonomy Platform",
                "Root-cause note and redeploy guardrails are both missing.",
                "Patch the inference timeout and add the post-incident redeploy gate."),
            new MissionFailure(
                "mf-1003",
                "rb-022",
                "BatterySag",
                "high",
                "Battery discharge curve dipped under the expected mission envelope mid-route.",
                "Field Operations",
                "Mission replay evidence is incomplete.",
                "Collect the full replay packet and verify the charger lane before the next sweep."),
            new MissionFailure(
                "mf-1004",
                "rb-014",
                "SafetyReviewGap",
                "high",
                "Safety reviewer has not closed the most recent manual-override packet.",
                "Safety Governance",
                "Override packet is still waiting on named signoff.",
                "Get the safety review closed before allowing another mission assignment."),
            new MissionFailure(
                "mf-1005",
                "rb-031",
                "IncidentNoteMissing",
                "medium",
                "Operator stop happened but the mission handoff note still lacks a final incident summary.",
                "Incident Command",
                "Missing final note blocks clean audit replay.",
                "Attach the final incident summary and timestamp the handoff."),
            new MissionFailure(
                "mf-1006",
                "rb-022",
                "ReplayGap",
                "medium",
                "Replay export omitted a portion of the trailer sweep sensor stream.",
                "Observability",
                "Incomplete replay set weakens evidence posture.",
                "Repair the sensor export and re-issue the mission replay archive.")
        },
        new[]
        {
            new OverridePacket(
                "ovr-301",
                "rb-014",
                "2026-05-29 · night replenishment",
                58,
                "red",
                "Safety reviewer signoff and localization calibration export are still missing.",
                "Safety Governance",
                "2h"),
            new OverridePacket(
                "ovr-302",
                "rb-031",
                "2026-05-29 · rush-pick recovery",
                64,
                "red",
                "Human stop note and redeploy guardrail checklist are both incomplete.",
                "Incident Command",
                "4h"),
            new OverridePacket(
                "ovr-303",
                "rb-022",
                "2026-05-30 · trailer sweep replay",
                79,
                "yellow",
                "Battery discharge evidence is ready, but replay export validation is still waiting.",
                "Fleet Reliability",
                "6h")
        });

    public static readonly FleetLane[] FleetLanes =
    {
        new(
            "warehouse-lane",
            "Warehouse AMR lane",
            "Fleet Reliability",
            "red",
            "Navigation confidence, aisle missions, and manual overrides",
            "Close localization drift and safety signoff before the next replenishment wave.",
            "The warehouse lane stays blocked until navigation confidence and override governance both recover."),
        new(
            "yard-lane",
            "Yard inspection lane",
            "Field Operations",
            "yellow",
            "Trailer sweep proof, battery posture, and replay evidence",
            "Finish the mission replay archive and verify the charging posture before dispatch.",
            "The yard lane is recoverable, but not yet clean enough for blind trust."),
        new(
            "fulfillment-lane",
            "Fulfillment pick lane",
            "Autonomy Platform",
            "red",
            "Vision inference, operator stops, and redeploy guardrails",
            "Repair the inference timeout and attach the operator stop summary before redeploying.",
            "The fulfillment lane is the highest redeploy risk in the fleet right now."),
        new(
            "courier-lane",
            "Hospital courier lane",
            "Mobility Operations",
            "green",
            "Specimen handoff, custody telemetry, and route stability",
            "Maintain green posture and keep custody telemetry complete.",
            "The courier lane is healthy and serves as the control sample for the fleet.")
    };
}
