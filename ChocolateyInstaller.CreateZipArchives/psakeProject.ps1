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

Task default -Depends build
Task build {}

Task clean -Description "Removes all built products" {
}
