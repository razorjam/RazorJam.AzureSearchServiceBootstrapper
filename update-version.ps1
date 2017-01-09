param ([string]$filePath = $null, [string]$version = $null)

if(!$filePath -Or !$version){
    Write-Output "ERROR: Update-Version given Invalid parameters."
    return;
}

"Updating version to '{0}' in project.json file located at '{1}'." -f $version, $filePath | Write-Output
$object = Get-Content -Raw -Path $filePath | ConvertFrom-Json
$object.version = $version
$object | ConvertTo-Json | Out-File -FilePath $filePath
