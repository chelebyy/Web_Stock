# Servisleri yönetmek için PowerShell script
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('start', 'stop', 'restart')]
    [string]$Action
)

function Stop-Services {
    Write-Host "Servisleri durdurma..."
    
    # Backend servisini durdur
    $stockApi = Get-Process -Name "Stock.API" -ErrorAction SilentlyContinue
    if ($stockApi) {
        Stop-Process -Name "Stock.API" -Force
        Write-Host "Backend servisi durduruldu."
    }
    
    # Frontend servisini durdur
    $node = Get-Process -Name "node" -ErrorAction SilentlyContinue
    if ($node) {
        Stop-Process -Name "node" -Force
        Write-Host "Frontend servisi durduruldu."
    }
    
    # Portları kontrol et
    $ports = @(5037, 4202)
    foreach ($port in $ports) {
        $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
        if ($connection) {
            Stop-Process -Id $connection.OwningProcess -Force
            Write-Host "Port $port serbest bırakıldı."
        }
    }
}

function Start-Services {
    Write-Host "Servisleri başlatma..."
    
    # Backend servisini başlat
    Set-Location -Path "Stock.API"
    Start-Process -FilePath "dotnet" -ArgumentList "run" -NoNewWindow
    Write-Host "Backend servisi başlatıldı."
    
    # Frontend servisini başlat
    Set-Location -Path "..\frontend"
    Start-Process -FilePath "npm" -ArgumentList "start -- --port 4202" -NoNewWindow
    Write-Host "Frontend servisi başlatıldı."
    
    Set-Location -Path ".."
}

switch ($Action) {
    'start' {
        Start-Services
    }
    'stop' {
        Stop-Services
    }
    'restart' {
        Stop-Services
        Start-Sleep -Seconds 2
        Start-Services
    }
} 