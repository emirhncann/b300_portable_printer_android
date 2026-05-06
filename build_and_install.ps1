# B300 Print Service - Auto Build & Install Script
# Telefonu USB ile bağlayın, USB hata ayıklama (developer mode + USB debugging) açık olsun
# Sonra bu scripti çalıştırın.

$ProjectDir = "$PSScriptRoot"
$ApkOutput = "$PSScriptRoot\B300PrintService-debug.apk"
$AdbPath = "adb" # adb PATH'de yoksa tam yol yazın: "C:\platform-tools\adb.exe"

Write-Host "=== B300 Print Service - Build & Install ===" -ForegroundColor Cyan

# 1. Build
Write-Host "`n[1/3] Building APK..." -ForegroundColor Yellow
Set-Location "$ProjectDir\B300PrintService"
.\gradlew assembleDebug
if ($LASTEXITCODE -ne 0) {
    Write-Host "BUILD FAILED!" -ForegroundColor Red
    exit 1
}

# 2. Copy APK
$BuiltApk = "$ProjectDir\B300PrintService\app\build\outputs\apk\debug\app-debug.apk"
Copy-Item -Path $BuiltApk -Destination $ApkOutput -Force
Write-Host "[2/3] APK kopyalandi: $ApkOutput" -ForegroundColor Green

# 3. Install via ADB
Write-Host "`n[3/3] ADB ile telefona yukleniyor..." -ForegroundColor Yellow
$devices = & $AdbPath devices
if ($devices -match "device$") {
    & $AdbPath install -r $ApkOutput
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`nYUKLEME BASARILI! Telefondaki eski servis otomatik guncellendi." -ForegroundColor Green
        Write-Host "Ayarlar -> Baglantilar -> Yazdirim menusunden servisi aktif edin." -ForegroundColor Cyan
    } else {
        Write-Host "ADB yukleme basarisiz. APK manuel kurulabilir: $ApkOutput" -ForegroundColor Red
    }
} else {
    Write-Host "Telefon bulunamadi veya ADB kurulu degil." -ForegroundColor Yellow
    Write-Host "APK manuel kurulabilir: $ApkOutput" -ForegroundColor White
    Write-Host "ADB kurmak icin: https://developer.android.com/tools/releases/platform-tools" -ForegroundColor Gray
}

Write-Host "`nTamamlandi!" -ForegroundColor Green
Set-Location $ProjectDir
