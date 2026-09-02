#!/usr/bin/env bash
# Run all MuPDF.NET.Examples projects (Linux/macOS).
set -euo pipefail
ROOT="$(cd "$(dirname "$0")" && pwd)"
cd "$ROOT"

UPDATE_EXPECTED=0
CONFIGURATION=Release

for arg in "$@"; do
  case "$arg" in
    --update-expected|-UpdateExpected) UPDATE_EXPECTED=1 ;;
    --configuration=*) CONFIGURATION="${arg#*=}" ;;
  esac
done

PSARGS=(-Configuration "$CONFIGURATION")
if [[ "$UPDATE_EXPECTED" -eq 1 ]]; then
  PSARGS+=(-UpdateExpected)
fi

if command -v pwsh >/dev/null 2>&1; then
  exec pwsh -NoProfile -File "$ROOT/run-all.ps1" "${PSARGS[@]}"
elif command -v powershell >/dev/null 2>&1; then
  exec powershell -NoProfile -File "$ROOT/run-all.ps1" "${PSARGS[@]}"
fi

echo "PowerShell (pwsh) is required. Install: https://aka.ms/powershell"
echo "Or run projects individually with: dotnet run --project MuPDF.NET/01-OpenSave"
exit 1
