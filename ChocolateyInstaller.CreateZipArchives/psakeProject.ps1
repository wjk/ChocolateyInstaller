function Perform {
	param(
		[Parameter(Mandatory=$true)][string]$Description,
		[Parameter(Mandatory=$true)][scriptblock]$Task
	)

	Write-Host "$($Description)"
	$Task.Invoke()
	Write-Host "$($Description) -- " -NoNewline
	Write-Host "done" -ForegroundColor Green
}

function Get-WebFile {
	param(
		[Parameter(Mandatory=$true)][uri]$URI,
		[Parameter(Mandatory=$true)][string]$Path
	)

	$client = (New-Object System.Net.WebClient)
	$client.DownloadFile($URI, $Path)
}

Properties {
	$SolutionDir = $null
	$ProjectDir = $null
	$Configuration = $null
}

Task default -Depends build
Task build -Depends WriteRCFile

Task MkDirs -RequiredVariables ProjectDir {
	$dir = "$($ProjectDir)\generated-archive"
	if (-not [system.io.directory]::Exists($dir)) {
		New-Item -ItemType Directory $dir | Out-Null
	}
}

Task DownloadChocolateyPackage -Depends MkDirs -RequiredVariables ProjectDir {
	$output = "$($ProjectDir)\generated-archive\Chocolatey.nupkg"
	if (-not [system.io.file]::Exists($output)) {
		Get-WebFile -URI 'https://chocolatey.org/api/v2/package/chocolatey/' -Path $output
	}
}

Task BuildWizardArchive -Depends MkDirs -RequiredVariables ProjectDir, SolutionDir, Configuration {
	Import-Module "$($ProjectDir)\powershell-zip\powershell-zip.psm1"
	Zip -ZipFile "$($ProjectDir)\generated-archive\Wizard.zip" -Files "$($SolutionDir)\ChocolateyInstaller.Wizard\bin\$($Configuration)\*.exe", `
	"$($SolutionDir)\ChocolateyInstaller.Wizard\bin\$($Configuration)\*.dll", "$($SolutionDir)\ChocolateyInstaller.Wizard\bin\$($Configuration)\*.config" `
	| Out-Null
}

Task WriteRCFile -Depends DownloadChocolateyPackage, BuildWizardArchive -RequiredVariables ProjectDir {
	$value = "
1 ZIPFILE `"Chocolatey.nupkg`"
2 ZIPFILE `"Wizard.zip`"
"

	Set-Content -Path "$($ProjectDir)\generated-archive\embedded-files.rc" -Value $value
}

Task clean -Description "Removes all built products" -RequiredVariables ProjectDir {
	rm -Recurse -Force "$($ProjectDir)\generated-archive"
}
