from pathlib import Path
import textwrap


ROOT = Path(__file__).resolve().parents[1]
SHOT_DIR = ROOT / "screenshots"
SHOT_DIR.mkdir(exist_ok=True)


def wrap(text: str, width: int):
    return textwrap.wrap(text, width=width) or [text]


def draw_lines(lines, x, y, font_size=22, color="#e9f3ff", weight="400", family="Inter,Segoe UI,Arial"):
    parts = [f'<text x="{x}" y="{y}" fill="{color}" font-size="{font_size}" font-weight="{weight}" font-family="{family}">']
    dy = 0
    for line in lines:
        parts.append(f'<tspan x="{x}" dy="{dy}">{line}</tspan>')
        dy = int(font_size * 1.25)
    parts.append("</text>")
    return "\n".join(parts)


def stat_card(x, y, w, h, label, value, detail, accent):
    return f"""
    <rect x="{x}" y="{y}" width="{w}" height="{h}" rx="20" fill="#111827" stroke="rgba(120,255,170,.14)" />
    {draw_lines([label.upper()], x + 24, y + 34, 11, "#8fdcff", "700", "Consolas, monospace")}
    {draw_lines([value], x + 24, y + 86, 34, accent, "700", "Segoe UI, Arial")}
    {draw_lines(wrap(detail, 28), x + 24, y + 124, 15, "#b8c6db", "400")}
    """


def panel(x, y, w, h, kicker, title, body, accent="#19c7ff"):
    return f"""
    <rect x="{x}" y="{y}" width="{w}" height="{h}" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.18)" />
    {draw_lines([kicker.upper()], x + 26, y + 34, 11, accent, "700", "Consolas, monospace")}
    {draw_lines(wrap(title, 30), x + 26, y + 84, 28, "#f5f7ff", "700", "Georgia, serif")}
    {draw_lines(wrap(body, 50), x + 26, y + 136, 16, "#b8c6db", "400")}
    """


def bullet_list(items, x, y, accent="#37ff8b"):
    rows = []
    offset = 0
    for item in items:
        rows.append(f'<circle cx="{x}" cy="{y + offset - 6}" r="5" fill="{accent}" />')
        rows.append(draw_lines(wrap(item, 70), x + 16, y + offset, 16, "#e9f3ff", "400"))
        offset += 54
    return "\n".join(rows)


def shell(eyebrow, title, subtitle, inner):
    return f"""<svg xmlns="http://www.w3.org/2000/svg" width="1400" height="860" viewBox="0 0 1400 860">
    <rect width="1400" height="860" fill="#070a0f"/>
    <rect x="24" y="24" width="1352" height="812" rx="30" fill="#0a1426" stroke="rgba(120,255,170,.18)"/>
    <rect x="58" y="58" width="1284" height="152" rx="26" fill="#0b1220" stroke="rgba(120,255,170,.12)"/>
    {draw_lines([eyebrow.upper()], 94, 96, 13, "#37ff8b", "700", "Consolas, monospace")}
    {draw_lines(wrap(title, 36), 94, 146, 34, "#f5f7ff", "700", "Georgia, serif")}
    {draw_lines(wrap(subtitle, 100), 94, 194, 18, "#b8c6db", "400")}
    {inner}
    </svg>"""


overview = shell(
    "Robot Fleet Exception Board",
    "Control-plane summary for robot mission failures, override packets, and redeploy pressure.",
    "Localization drift, vision timeouts, battery sag, and replay evidence stay visible together before another autonomous mission window opens.",
    f"""
    {stat_card(58, 238, 288, 150, "robot units", "4", "Warehouse, yard, fulfillment, and courier robots modeled together.", "#19c7ff")}
    {stat_card(364, 238, 288, 150, "active failures", "6", "Localization, vision, review, note, battery, and replay failures still open.", "#ffcc66")}
    {stat_card(670, 238, 288, 150, "override risks", "3", "Manual override packets are still missing evidence or named signoff.", "#ff5c7a")}
    {stat_card(976, 238, 366, 150, "lead recommendation", "Repair drift and close override packets", "Do not reopen autonomous windows until localization and signoff posture recover.", "#37ff8b")}

    {panel(58, 420, 614, 356, "Mission failures", "The riskiest mission blockers stay visible first.", "Navigation drift, camera inference timeouts, and battery-sag failures stay tied to their owners so the fleet queue is readable before redeploy.", "#19c7ff")}
    <rect x="708" y="420" width="634" height="356" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.18)" />
    {draw_lines(["OVERRIDE POSTURE"], 734, 454, 11, "#37ff8b", "700", "Consolas, monospace")}
    {draw_lines(["What must close before", "the next mission wave"], 734, 506, 28, "#f5f7ff", "700", "Georgia, serif")}
    {bullet_list([
      "Warehouse AMR localization recalibration and safety reviewer signoff are both still missing.",
      "Fulfillment pick redeploy is blocked until the vision timeout note and human-stop summary land.",
      "Yard inspection replay evidence is incomplete, so the next dispatch is still review-bound."
    ], 744, 580)}
    """,
)

lane = shell(
    "Fleet Lane",
    "Each lane keeps owner, mission, fleet, and next action visible.",
    "Warehouse, yard, fulfillment, and courier lanes stay separated cleanly so exception routing does not collapse into one noisy queue.",
    f"""
    {panel(58, 238, 620, 250, "Warehouse lane", "AMR replenishment missions", "Localization drift, aisle missions, and override governance stay grouped under Fleet Reliability ownership.", "#19c7ff")}
    {panel(708, 238, 634, 250, "Yard lane", "Trailer sweep and inspection posture", "Battery sag, replay evidence, and dispatch readiness stay visible before another field run.", "#ffcc66")}
    {panel(58, 520, 620, 256, "Fulfillment lane", "Rush-pick mission resilience", "Vision inference failures and human stops stay tied to Autonomy Platform review pressure.", "#b88cff")}
    {panel(708, 520, 634, 256, "Courier lane", "Specimen handoff stability", "Custody telemetry, route health, and mission control posture provide the fleet's green reference lane.", "#37ff8b")}
    """,
)

posture = shell(
    "Override Posture",
    "Packet readiness, missing evidence, and signoff timing stay readable for robotics operators.",
    "The board keeps redeploy posture explicit instead of hiding it behind aggregate fleet health numbers.",
    f"""
    {panel(58, 238, 402, 244, "Packet OVR-301", "Warehouse localization override", "58 percent complete. Safety signoff and calibration export are both still missing.", "#ff5c7a")}
    {panel(498, 238, 402, 244, "Packet OVR-302", "Fulfillment human-stop recovery", "64 percent complete. Incident note and redeploy guardrail checklist are still incomplete.", "#ffcc66")}
    {panel(938, 238, 404, 244, "Packet OVR-303", "Yard replay packet", "79 percent complete. Replay validation is close, but not yet operator-safe.", "#37ff8b")}
    {panel(58, 514, 1284, 262, "Why this monetizes cleanly", "Hosted preview planned, paid template pack later, embedded by engagement.", "This is a strong robotics operator wedge because it lives where teams actually feel pressure: mission failures, manual overrides, replay evidence, and the named signoff needed before the next autonomous window opens.", "#19c7ff")}
    """,
)

(SHOT_DIR / "01-overview.svg").write_text(overview, encoding="utf-8")
(SHOT_DIR / "02-fleet-lane.svg").write_text(lane, encoding="utf-8")
(SHOT_DIR / "03-override-posture.svg").write_text(posture, encoding="utf-8")
