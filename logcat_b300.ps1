# B300 Print Service - Gerçek Zamanlı Log Scripti
# Telefonu USB ile bağlayın, USB Hata Ayıklama açık olsun
# Sonra bu scripti çalıştırın - yazdırmayı deneyin ve log çıkışını buradan takip edin

$AdbPath = "adb" # ADB PATH'de yoksa tam yol yazın

Write-Host "=== B300 Print Service Logcat ===" -ForegroundColor Cyan
Write-Host "Cikis icin Ctrl+C'ye basin" -ForegroundColor Gray
Write-Host ""

# Logcat'i temizle
& $AdbPath logcat -c

# Servisimize ve Android Print Framework'e ait logları filtrele
& $AdbPath logcat -s "B300PrintService:V" "B300DiscoverySession:V" "PrintService:V" "PrintSpoolerService:W" "ActivityManager:W" AndroidRuntime:E
