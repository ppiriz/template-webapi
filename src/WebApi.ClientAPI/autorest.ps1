[cmdletBinding()]
param()
$targetHost = 'http://localhost:63300/swagger/v1/swagger.json'
$possiblePaths = @('..\packages\autorest\0.17.3\tools\AutoRest.exe',"$env:USERPROFILE.nuget\packages\autorest\0.17.3\tools\autorest.exe")

foreach($path in $possiblePaths)
{
    Write-Verbose "Checking $path for autorest.exe"
    if(Test-Path $path)
    {
        & $path -CSharp -ModelsName Models -Namespace webApi.ClientAPI -Input $targetHost -PackageName ClientAPI -PackageVersion 0.0.2

        exit 0
    }
}
Write-Warning "Cant locate autorest.exe. Did you package restore?"