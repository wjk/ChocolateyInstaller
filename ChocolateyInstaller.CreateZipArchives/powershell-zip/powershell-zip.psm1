@{
    Author = "Juliano Sales"
    ModuleVersion = "0.0.0"
    HelpInfoUri = "https://github.com/julianosaless/powershellzip"
    FunctionsToExport = @('Zip','Unzip')
}


function Zip {
    [CmdletBinding()]
    Param(
		[string]$ZipFile,
		[string[]]$Files
	)
    
    & 7z.exe a -tzip "$ZipFile" $Files
}

function Unzip {
  [CmdletBinding()]
    Param(
		[string]$source,
		[string]$destination
	)

  [Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem");
  [System.IO.Compression.ZipFile]::ExtractToDirectory($source, $destination)
}
