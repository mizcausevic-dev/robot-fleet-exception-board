# Security Notes

`robot-fleet-exception-board` publishes synthetic robotics mission, override, and failure data only.

It does **not** contain:

- live robot telemetry
- facility control secrets
- warehouse, hospital, or customer identifiers
- production autonomy stack credentials
- real safety review packets

This repo is a public operator-surface proof for robotics and fleet governance workflows. It is not a safety certification claim and should not be treated as production fleet-control software.
