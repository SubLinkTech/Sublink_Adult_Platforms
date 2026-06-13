$version = "3.5.1";

$currentDir = (Get-Item $MyInvocation.MyCommand.Path).Directory.FullName;

. dotnet.exe restore $currentDir
. dotnet.exe publish -c Release -r win-x64 $currentDir/SubLink.References/SubLink.References.csproj /p:Version=$version /p:SkipInvalidConfigurations=true;
. dotnet.exe publish -c Release -r win-x64 $currentDir/SubLink.Fansly/SubLink.Fansly.csproj /p:Version=$version /p:SkipInvalidConfigurations=true
. dotnet.exe publish -c Release -r win-x64 $currentDir/SubLink.OpenShock/SubLink.OpenShock.csproj /p:Version=$version /p:SkipInvalidConfigurations=true
. dotnet.exe publish -c Release -r win-x64 $currentDir/SubLink.Joystick/SubLink.Joystick.csproj /p:Version=$version /p:SkipInvalidConfigurations=true

New-Item build-$version -ItemType directory;
Copy-Item -Path "SubLink_Adult\SubLink_Adult.cs" -Destination "build-$($version)";
Copy-Item -Path "SubLink_Adult\Platforms\" -Destination "build-$($version)" -Recurse;
Copy-Item -Path "SubLink.Fansly\bin\Release\net8.0\win-x64\publish\SubLink.Fansly.dll" -Destination "build-$($version)\Platforms";
Copy-Item -Path "SubLink.OpenShock\bin\Release\net8.0\win-x64\publish\SubLink.OpenShock.dll" -Destination "build-$($version)\Platforms";
Copy-Item -Path "SubLink.Joystick\bin\Release\net8.0\win-x64\publish\SubLink.Joystick.dll" -Destination "build-$($version)\Platforms";

if (-not (Test-Path -Path "builds")) {
    New-Item -Path "builds" -ItemType Directory;
}

if (Test-Path builds\SubLink_Adult-$version-win-x64.zip) {
    Remove-Item builds\SubLink_Adult-$version-win-x64.zip;
}

Compress-Archive -Path "build-$version\*" -destinationpath "builds\SubLink_Adult-$version-win-x64.zip" -compressionlevel optimal;
Remove-Item build-$version -Recurse;
