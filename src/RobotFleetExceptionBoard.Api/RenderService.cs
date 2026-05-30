using System.Text.Json;

namespace RobotFleetExceptionBoard.Api;

public static class RenderService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string Overview() => Layout(
        "Robot Fleet Exception Board",
        "/",
        $$"""
        <section class="section">
          <div class="sh"><h2>Operator Snapshot</h2><div class="note">robotics / fleet operations / mission governance</div></div>
          <div class="kpis">
            {{Metric("4", "robot units", "Warehouse, yard, fulfillment, and courier robots modeled in one control plane.", "cyan")}}
            {{Metric("6", "active failures", "Localization, vision, battery, review, note, and replay failures still open.", "red")}}
            {{Metric("2", "critical failures", "Autonomous mission blockers that should stop the next run window.", "red")}}
            {{Metric("3", "override risks", "Human-override packets still need evidence or named signoff.", "amber")}}
            {{Metric("3", "mission risks", "Three lanes are still too weak for operator-blind trust.", "plum")}}
            {{Metric("58%", "lowest packet", "The weakest override packet is still far from redeploy-safe.", "yellow")}}
          </div>
        </section>
        <section class="section">
          <div class="sh"><h2>Why this lane matters</h2><div class="note">c# / dotnet / robotics ops</div></div>
          <div class="stack">
            <div class="src"><div class="src-name">mission safety</div><div class="src-tit">Keep failures, override posture, and redeploy risk on one board</div><p>Fleet operators need to see mission failures, manual override evidence, and handoff posture together before the next autonomous window opens.</p></div>
            <div class="src"><div class="src-name">operator depth</div><div class="src-tit">Real robotics workflow pressure, not generic uptime theater</div><p>This surface stays grounded in localization drift, battery sag, vision stack faults, review gaps, and replay evidence that determine whether a robot can go back out.</p></div>
            <div class="src"><div class="src-name">monetization path</div><div class="src-tit">Hosted preview planned · Embedded by engagement</div><p>The public repo proves the operator model. The commercial path is an embedded fleet exception and override review surface.</p></div>
          </div>
        </section>
        """
    );

    public static string FleetLane() => Layout(
        "Robot Fleet Exception Board — Fleet Lane",
        "/fleet-lane",
        $$"""
        <section class="section">
          <div class="sh"><h2>Fleet Lane</h2><div class="note">owner · focus · next action</div></div>
          <table class="ttbl">
            <thead><tr><th>Lane</th><th>Owner</th><th>Status</th><th>Focus</th><th>Next action</th></tr></thead>
            <tbody>
              {{string.Join("", SampleData.FleetLanes.Select(lane => $$"""
                <tr>
                  <td><b>{{lane.Lane}}</b><br />{{lane.Note}}</td>
                  <td>{{lane.Owner}}</td>
                  <td><span class="st {{SeverityClass(lane.Status)}}">{{lane.Status}}</span></td>
                  <td>{{lane.Focus}}</td>
                  <td>{{lane.NextAction}}</td>
                </tr>
              """))}}
            </tbody>
          </table>
        </section>
        """
    );

    public static string MissionFailures() => Layout(
        "Robot Fleet Exception Board — Mission Failures",
        "/mission-failures",
        $$"""
        <section class="section">
          <div class="sh"><h2>Mission Failures</h2><div class="note">severity · owner · blocker</div></div>
          <table class="ttbl">
            <thead><tr><th>Failure</th><th>Owner</th><th>Unit</th><th>Observed state</th></tr></thead>
            <tbody>
              {{string.Join("", SampleData.Payload.Failures.Select(failure => $$"""
                <tr>
                  <td><span class="st {{SeverityClass(failure.Severity)}}">{{failure.Severity}}</span><br /><b>{{failure.Code}}</b></td>
                  <td>{{failure.Owner}}</td>
                  <td>{{failure.UnitId}}</td>
                  <td>{{failure.Summary}}<br /><code>{{failure.Blocker}}</code></td>
                </tr>
              """))}}
            </tbody>
          </table>
        </section>
        """
    );

    public static string OverridePosture() => Layout(
        "Robot Fleet Exception Board — Override Posture",
        "/override-posture",
        $$"""
        <section class="section">
          <div class="sh"><h2>Override Posture</h2><div class="note">packet readiness · blocker · timing</div></div>
          <div class="board">
            {{string.Join("", SampleData.Payload.Overrides.Select(packet => $$"""
              <article class="pcard">
                <div class="ptop">
                  <div class="pnum">{{packet.CompletenessPercent}}%</div>
                  <div class="ppri">{{packet.Owner}}</div>
                </div>
                <h3>{{packet.UnitId}}</h3>
                <p class="pdesc">{{packet.MissionWindow}}</p>
                <ul class="check">
                  <li>{{packet.MissingEvidence}}</li>
                  <li>Review ETA: {{packet.SignoffEta}}</li>
                  <li>Status: <span class="st {{SeverityClass(packet.Status)}}">{{packet.Status}}</span></li>
                </ul>
                <div class="pfoot"><code>{{packet.PacketId}}</code></div>
              </article>
            """))}}
          </div>
        </section>
        """
    );

    public static string Verification() => Layout(
        "Robot Fleet Exception Board — Verification",
        "/verification",
        $$"""
        <section class="section">
          <div class="sh"><h2>Verification</h2><div class="note">operator-safe claims only</div></div>
          <div class="stack">
            {{VerificationCard("Synthetic robot mission, override, and failure evidence only; no live autonomy stack, facility, or industrial control data is published.")}}
            {{VerificationCard("The control plane is rooted in fleet reliability, autonomy platform operations, safety governance, and incident handoff pressure.")}}
            {{VerificationCard("This is a robotics operator surface, not a safety-certification or regulatory-compliance claim.")}}
            {{VerificationCard("Hosted preview is planned; embedded fleet review delivery is available by engagement.")}}
          </div>
        </section>
        """
    );

    public static string Docs() => Layout(
        "Robot Fleet Exception Board — Docs",
        "/docs",
        $$"""
        <section class="section">
          <div class="sh"><h2>Docs</h2><div class="note">routes · runbook · api</div></div>
          <div class="stack">
            <div class="src"><div class="src-name">routes</div><div class="src-tit">Public proof surface</div><p><code>/</code>, <code>/fleet-lane</code>, <code>/mission-failures</code>, <code>/override-posture</code>, <code>/verification</code>, <code>/docs</code></p></div>
            <div class="src"><div class="src-name">api</div><div class="src-tit">Structured payloads</div><p><code>/api/dashboard/summary</code>, <code>/api/fleet-lane</code>, <code>/api/mission-failures</code>, <code>/api/override-posture</code>, <code>/api/verification</code>, <code>/api/sample</code></p></div>
            <div class="src"><div class="src-name">runbook</div><div class="src-tit">Local execution</div><p><code>dotnet run --project src/RobotFleetExceptionBoard.Api -- --demo</code> prints the same robotics posture used by the public proof surface.</p></div>
          </div>
        </section>
        """
    );

    public static string Sample() => JsonSerializer.Serialize(new
    {
        summary = AnalysisService.Summary(),
        fleetLane = SampleData.FleetLanes,
        missionFailures = SampleData.Payload.Failures,
        overridePosture = SampleData.Payload.Overrides,
        sample = SampleData.Payload
    }, JsonOptions);

    private static string VerificationCard(string title) =>
        $$"""<div class="src"><div class="src-name">verification</div><div class="src-tit">{{title}}</div><p>This lane keeps mission pressure, override evidence, and commercial framing honest.</p></div>""";

    private static string Metric(string value, string label, string help, string tone) =>
        $$"""<div class="kpi {{tone}}"><div class="v">{{value}}</div><div class="lbl">{{label}}</div><div class="h">{{help}}</div></div>""";

    private static string SeverityClass(string value) => value switch
    {
        "critical" or "high" or "red" => "red",
        "medium" or "yellow" => "yellow",
        "green" or "low" => "green",
        _ => "info"
    };

    public static string Layout(string title, string active, string body)
    {
        var nav = new[]
        {
            Nav("/", "Overview"),
            Nav("/fleet-lane", "Fleet Lane"),
            Nav("/mission-failures", "Mission Failures"),
            Nav("/override-posture", "Override Posture"),
            Nav("/verification", "Verification"),
            Nav("/docs", "Docs")
        };

        var navHtml = string.Join("", nav.Select(item =>
            item.Href == active
                ? $"""<a class="navchip active" href="{item.Href}">{item.Label}</a>"""
                : $"""<a class="navchip" href="{item.Href}">{item.Label}</a>"""));

        return $$$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>{{{title}}}</title>
          <style>
            :root{--bg:#070a0f;--panel:#0b1220;--line:rgba(120,255,170,.18);--line2:rgba(120,255,170,.10);--text:#e9f3ff;--muted:rgba(233,243,255,.72);--muted2:rgba(233,243,255,.55);--bert:#37ff8b;--bert2:#19c7ff;--warn:#ffcc66;--bad:#ff5c7a;--shadow:0 18px 60px rgba(0,0,0,.55);--mono:ui-monospace,SFMono-Regular,Menlo,Monaco,Consolas,"Liberation Mono","Courier New",monospace;--sans:ui-sans-serif,system-ui,-apple-system,Segoe UI,Roboto,Helvetica,Arial}
            *{box-sizing:border-box} body{margin:0;font-family:var(--sans);color:var(--text);background:radial-gradient(1200px 600px at 20% -10%, rgba(55,255,139,.18), transparent 60%),radial-gradient(900px 520px at 90% 0%, rgba(25,199,255,.16), transparent 55%),linear-gradient(180deg,#05070c 0%,#070a0f 35%,#05070c 100%)}
            .wrap{max-width:1280px;margin:0 auto;padding:24px 22px 80px}.topbar{display:flex;justify-content:space-between;gap:14px;border-bottom:1px solid var(--line2);padding-bottom:14px;margin-bottom:22px;font-family:var(--mono);font-size:11px;letter-spacing:.16em;color:var(--muted);text-transform:uppercase}.topbar .left{color:var(--bert)}
            .herorow{display:grid;grid-template-columns:1.5fr .9fr;gap:18px}@media (max-width:1000px){.herorow{grid-template-columns:1fr}}
            .hero,.src,.pcard,.kpi,.bluf,.corr{background:linear-gradient(180deg, rgba(11,18,32,.95), rgba(8,14,26,.92));border:1px solid var(--line);box-shadow:var(--shadow)}
            .hero{border-radius:22px;padding:28px 28px 24px;border-top:2px solid var(--bert2)} .hero h1{font-size:60px;line-height:.95;margin:0 0 18px;font-weight:800}@media (max-width:700px){.hero h1{font-size:42px}} .hero p{color:var(--muted);font-size:15px;line-height:1.55;max-width:680px;margin:0 0 18px}
            .chiprow,.navrow{display:flex;flex-wrap:wrap;gap:8px}.navrow{margin-top:18px}.meta-chip,.navchip,.ppri,.st,code{font-family:var(--mono)} .meta-chip,.navchip{font-size:11px;color:var(--muted);padding:7px 12px;border-radius:999px;border:1px solid var(--line);background:rgba(6,10,18,.4);text-decoration:none}.navchip.active{color:#071017;background:linear-gradient(135deg,var(--bert),var(--bert2));font-weight:700}
            .side{display:flex;flex-direction:column;gap:14px}.bluf,.corr{border-radius:14px;padding:16px 18px}.bluf{border-left:4px solid var(--warn)}.corr{border-left:4px solid var(--bert)}.lbl{font-family:var(--mono);font-size:10px;letter-spacing:.18em;text-transform:uppercase}.bluf .lbl{color:var(--warn)} .corr .lbl{color:var(--bert)} .bluf p,.corr p,.src p,.pcard .pdesc,.kpi .h{color:var(--muted);line-height:1.55}
            .section{margin-top:34px}.sh{display:flex;justify-content:space-between;gap:14px;padding-bottom:10px;border-bottom:1px solid var(--line2);margin-bottom:14px}.sh h2{margin:0;font-size:24px;font-weight:600}.sh .note{font-family:var(--mono);font-size:11px;color:var(--muted2);letter-spacing:.16em;text-transform:uppercase}
            .kpis{display:grid;grid-template-columns:repeat(6,1fr);gap:12px}@media (max-width:1100px){.kpis{grid-template-columns:repeat(3,1fr)}}@media (max-width:640px){.kpis{grid-template-columns:repeat(2,1fr)}} .kpi{border-radius:14px;padding:14px 14px 12px}.kpi .v{font-size:26px;font-weight:600}.kpi .lbl{font-size:10px;letter-spacing:.18em;text-transform:uppercase;color:var(--muted);margin-top:6px}.cyan .v{color:var(--bert2)} .green .v{color:var(--bert)} .plum .v{color:#b88cff} .amber .v,.yellow{color:var(--warn)} .red .v,.red{color:var(--bad)}
            .stack{display:grid;grid-template-columns:repeat(3,1fr);gap:12px}@media (max-width:1100px){.stack{grid-template-columns:repeat(2,1fr)}}@media (max-width:640px){.stack{grid-template-columns:1fr}} .src{border-radius:16px;padding:16px}.src-name{font-family:var(--mono);font-size:11px;color:var(--bert);letter-spacing:.2em;text-transform:uppercase}.src-tit{margin:8px 0 6px;font-size:17px;font-weight:600}
            .ttbl{width:100%;border-collapse:separate;border-spacing:0;border:1px solid var(--line);border-radius:14px;overflow:hidden}.ttbl th,.ttbl td{padding:13px 14px;text-align:left;font-size:13.5px;vertical-align:top}.ttbl thead th{font-family:var(--mono);font-size:11px;letter-spacing:.16em;text-transform:uppercase;color:var(--muted2);border-bottom:1px solid var(--line);background:rgba(11,18,32,.5)}.ttbl td,.ttbl td *{color:var(--muted)}.ttbl b{color:var(--text)}
            .board{display:grid;grid-template-columns:repeat(3,1fr);gap:14px}@media (max-width:1000px){.board{grid-template-columns:1fr}} .pcard{border-radius:16px;padding:18px 20px;display:flex;flex-direction:column}.ptop{display:flex;justify-content:space-between;align-items:center;margin-bottom:8px}.pnum{font-family:var(--mono);font-size:22px;font-weight:600;color:var(--bert)}.ppri{font-size:10px;padding:5px 10px;border-radius:999px;border:1px solid var(--line);color:var(--bert);letter-spacing:.14em;background:rgba(55,255,139,.06)}.pcard h3{margin:6px 0 8px;font-size:19px}.check{list-style:none;padding:0;margin:0 0 14px}.check li{display:grid;grid-template-columns:18px 1fr;gap:10px;padding:6px 0;font-size:13.5px;color:var(--muted)}.check li:before{content:"";width:14px;height:14px;border:1px solid var(--line);border-radius:3px;background:rgba(6,10,18,.4);margin-top:3px}
            .st{font-size:10px;padding:4px 9px;border-radius:6px;letter-spacing:.1em;text-transform:uppercase;border:1px solid currentColor;display:inline-block}.st.green{color:var(--bert)}.st.yellow{color:var(--warn)}.st.red{color:var(--bad)}.st.info{color:var(--bert2)}
            .footer{margin-top:30px;padding-top:14px;border-top:1px dashed var(--line2);display:flex;justify-content:space-between;gap:10px;flex-wrap:wrap;font-family:var(--mono);font-size:11px;color:var(--muted2);letter-spacing:.08em} code{font-size:12px;color:var(--bert2);background:rgba(25,199,255,.08);padding:1px 6px;border-radius:5px;border:1px solid rgba(25,199,255,.18)}
          </style>
        </head>
        <body>
          <div class="wrap">
            <div class="topbar">
              <div class="left">Kinetic Gain · Robot Fleet Exception Board</div>
              <div>synthetic mission failures and override packets · no live robot, facility, or industrial control secrets</div>
            </div>
            <div class="herorow">
              <section class="hero">
                <div class="chiprow">
                  <span class="meta-chip">Robotics / fleet exception lane</span>
                  <span class="meta-chip">C# · mission governance and override proof</span>
                  <span class="meta-chip">Hosted preview planned · Embedded by engagement</span>
                </div>
                <h1>Mission failures, override packets, and redeploy posture that stay operator-readable.</h1>
                <p>This control plane turns synthetic fleet exports into one review surface: localization drift, vision timeouts, battery sag, safety signoff gaps, replay evidence, and human override posture before another autonomous mission window opens.</p>
                <div class="navrow">{{{navHtml}}}</div>
              </section>
              <aside class="side">
                <div class="bluf"><div class="lbl">Commercial front door</div><p><strong>Fleet exception routing, override evidence posture, and redeploy-safe mission review for robotics teams.</strong><br />The free surface is buyer-readable proof; the commercial path is an embedded exception and override module for fleet workflows.</p></div>
                <div class="corr"><div class="lbl">Proof layer</div><p><strong>.NET API + static operator shell.</strong><br />The repo models autonomy platform pressure, fleet reliability, safety governance, and operator handoff without pretending to be a live robot integration.</p></div>
                <div class="corr"><div class="lbl">Why it matters</div><p>Robotics teams need mission failure pressure and override evidence in one lane, not scattered device logs, operator notes, and stale redeploy packets.</p></div>
              </aside>
            </div>
            {{{body}}}
            <div class="footer">
              <div>robot-fleet-exception-board · synthetic sample data only</div>
              <div>routes: / · /fleet-lane · /mission-failures · /override-posture · /verification · /docs</div>
            </div>
          </div>
        </body>
        </html>
        """;
    }

    private static (string Href, string Label) Nav(string href, string label) => (href, label);
}
