param (
    [Parameter(Mandatory=$true)]
    [string]$deployUser
)

Write-Host "building app ..."
dotnet build

Write-Host "publishing app ..."
dotnet publish -c Release -o .\publish

Write-Host "deploying app ..."
scp -r .\publish\* ${deployUser}@142.93.135.187:/var/www/clodsire.nl/api

Write-Host "Restarting api.service on the server, requires SSSH key ..."
ssh ${deployUser}@142.93.135.187 'sudo systemctl restart api.service'

Write-Host "Done"
