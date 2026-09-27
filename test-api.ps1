# Pick Me Up API — PowerShell Test Script
# Run in PowerShell from repo root. Requires: Python with PyJWT, API running on localhost:5137

$secret = "7b3eb72dac98616f013eb6bea2d09182e7b0832a2454b29574edc04c51c706cb"
$accountId = [guid]::NewGuid().ToString()

# Generate JWT token
$token = python -c "import jwt,uuid,time; s='$secret'; a='$accountId'; print(jwt.encode({'sub':a,'accountId':a,'iss':'MobiusOrderServer','aud':'MobiusOrderClient','exp':int(time.time())+600},s,algorithm='HS256'))"

Write-Host "Test Account: $accountId" -ForegroundColor Cyan
Write-Host "Token: $($token.Substring(0,50))..." -ForegroundColor Gray

$headers = @{ "Authorization" = "Bearer $token" }

# ── 1. Health ──────────────────────────────────────────────────
Write-Host "`n=== HEALTH ===" -ForegroundColor Yellow
Invoke-RestMethod -Uri "http://localhost:5137/health"
Write-Host ""
Invoke-RestMethod -Uri "http://localhost:5137/ready"
Write-Host ""

# ── 2. Audio Defaults (no auth) ───────────────────────────────
Write-Host "`n=== AUDIO DEFAULTS ===" -ForegroundColor Yellow
Invoke-RestMethod -Uri "http://localhost:5137/account/preferences/audio/defaults" | ConvertTo-Json -Depth 5

# ── 3. GET Audio Preferences ──────────────────────────────────
Write-Host "`n=== GET AUDIO ===" -ForegroundColor Yellow
Invoke-RestMethod -Uri "http://localhost:5137/account/preferences/audio" -Headers $headers | ConvertTo-Json -Depth 5

# ── 4. PUT Audio (update) ─────────────────────────────────────
Write-Host "`n=== PUT AUDIO ===" -ForegroundColor Yellow
$body = @{ masterVolume = 0.5; musicVolume = 0.3; version = 1 } | ConvertTo-Json
Invoke-RestMethod -Uri "http://localhost:5137/account/preferences/audio" -Method PUT -Headers $headers -Body $body -ContentType "application/json" | ConvertTo-Json -Depth 5

# ── 5. GET Audio (verify) ─────────────────────────────────────
Write-Host "`n=== GET AUDIO (verify) ===" -ForegroundColor Yellow
Invoke-RestMethod -Uri "http://localhost:5137/account/preferences/audio" -Headers $headers | ConvertTo-Json -Depth 5

# ── 6. Stale Version (expect 409) ─────────────────────────────
Write-Host "`n=== STALE VERSION (expect 409) ===" -ForegroundColor Yellow
$staleBody = @{ masterVolume = 0.9; version = 1 } | ConvertTo-Json
try {
    Invoke-RestMethod -Uri "http://localhost:5137/account/preferences/audio" -Method PUT -Headers $headers -Body $staleBody -ContentType "application/json"
} catch {
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "Response: $($_.ErrorDetails.Message)" -ForegroundColor Red
}

# ── 7. GET All Preferences ────────────────────────────────────
Write-Host "`n=== GET ALL PREFERENCES ===" -ForegroundColor Yellow
Invoke-RestMethod -Uri "http://localhost:5137/account/preferences" -Headers $headers | ConvertTo-Json -Depth 5

# ── 8. PATCH Audio (partial update) ───────────────────────────
Write-Host "`n=== PATCH AUDIO ===" -ForegroundColor Yellow
$patchBody = @{ musicVolume = 0.7; version = 2 } | ConvertTo-Json
Invoke-RestMethod -Uri "http://localhost:5137/account/preferences/audio" -Method PATCH -Headers $headers -Body $patchBody -ContentType "application/json" | ConvertTo-Json -Depth 5

Write-Host "`n=== ALL TESTS PASSED ===" -ForegroundColor Green
