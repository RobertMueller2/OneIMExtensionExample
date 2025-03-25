param(
    [Parameter(Position=0,mandatory=$true,HelpMessage="Frontend installation directory that has a NuGet Subdirectory")]
    [string]$FrontendDirectory
)

if (-not (Test-Path $FrontendDirectory)) {
    Write-Error "Directory '$FrontendDirectory' does not exist!"
    exit 1
}

$TargetDirectory = [System.IO.Path]::Combine($FrontendDirectory, "NuGet")
if (-not (Test-Path $TargetDirectory)) {
    Write-Error "Directory '$TargetDirectory' does not exist!"
    exit 1
}

$packages = Get-ChildItem -Path $TargetDirectory -Filter *.nupkg
$xml = New-Object System.Xml.XmlDocument
$xml.AppendChild($xml.CreateXmlDeclaration("1.0", "utf-8", $null)) | Out-Null
$root = $xml.CreateElement("packages")
$xml.AppendChild($root) | Out-Null

foreach ($pkg in $packages) {
    $zip = [System.IO.Compression.ZipFile]::OpenRead($pkg.FullName)
    $nuspecEntry = $zip.Entries | Where-Object { $_.Name -like "*.nuspec" } | Select-Object -First 1
    $nuspecStream = $nuspecEntry.Open()
    $nuspec = New-Object System.Xml.XmlDocument
    $nuspec.Load($nuspecStream)
    $id = $nuspec.package.metadata.id
    $version = $nuspec.package.metadata.version

    $packageNode = $xml.CreateElement("package")
    $packageNode.SetAttribute("id", $id)
    $packageNode.SetAttribute("version", $version)
    $root.AppendChild($packageNode) | Out-Null

    $zip.Dispose()
}
$xml.Save("packages.config")
Read-Host "Press any key to continue"