# Versione 1.0.4
# Fermo app pool per procedere al rilascio

$appPool = '#{WebAppName}#AppPool'
If (Get-IISAppPool $appPool -ErrorAction SilentlyContinue) {
    If ((Get-IISAppPool $appPool).State -eq 'Started') {
        Stop-WebAppPool $appPool
        Write-Host "Fermo app pool $appPool, aspetto 10 secondi per non trovare file bloccati.."
		start-sleep -seconds 10
		Write-Host "Proseguo"
    } Else {
        Write-Host "App pool $appPool trovato gia' fermo, ok proseguo."
    }
} Else {
    Write-Host "App pool $appPool non trovata, ok proseguo."
}

$appPool = '#{WebAPIName}#AppPool'
If (Get-IISAppPool $appPool -ErrorAction SilentlyContinue) {
    If ((Get-IISAppPool $appPool).State -eq 'Started') {
        Stop-WebAppPool $appPool
        Write-Host "Fermo app pool $appPool, aspetto 10 secondi per non trovare file bloccati.."
		start-sleep -seconds 10
		Write-Host "Proseguo"
    } Else {
        Write-Host "App pool $appPool trovato gia' fermo, ok proseguo."
    }
} Else {
    Write-Host "App pool $appPool non trovata, ok proseguo."
}