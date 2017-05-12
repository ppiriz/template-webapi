[cmdletBinding()]
param()
$SourceFile = "webApi.ClientAPI.csproj"
$destFile = "nuget-$SourceFile"

Write-Verbose 'Removing unnecessary references'
Copy-Item $SourceFile $destFile
$csProj = [xml](gc $destFile)
$targetElement = $csProj.SelectSingleNode('//PackageReference[@Include="AutoRest"]')
$targetElement.ParentNode.RemoveChild($targetElement) | Out-Null

$csProj.Save($destFile)
Write-Verbose 'Creating nuget package'
&dotnet @('pack',$destFile,'/p:Version=0.0.1')

Write-Verbose 'Restoring csproj'

Remove-Item $destFile