#!/bin/bash

VERSION="3.5.1"
CURRENT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

dotnet restore "$CURRENT_DIR"

dotnet publish -c Release -r linux-x64 "$CURRENT_DIR/SubLink.References/SubLink.References.csproj" /p:Version="$VERSION" /p:SkipInvalidConfigurations=true
dotnet publish -c Release -r linux-x64 "$CURRENT_DIR/SubLink.Fansly/SubLink.Fansly.csproj" /p:Version="$VERSION" /p:SkipInvalidConfigurations=true
dotnet publish -c Release -r linux-x64 "$CURRENT_DIR/SubLink.OpenShock/SubLink.OpenShock.csproj" /p:Version="$VERSION" /p:SkipInvalidConfigurations=true
dotnet publish -c Release -r linux-x64 "$CURRENT_DIR/SubLink.Joystick/SubLink.Joystick.csproj" /p:Version="$VERSION" /p:SkipInvalidConfigurations=true

BUILD_DIR="build-$VERSION"
mkdir -p "$BUILD_DIR"

cp "SubLink_Adult/SubLink_Adult.cs" "$BUILD_DIR/"
cp -r "SubLink_Adult/Platforms/" "$BUILD_DIR/"

cp "SubLink.Fansly/bin/Release/net8.0/linux-x64/publish/SubLink.Fansly.dll" "$BUILD_DIR/Platforms/"
cp "SubLink.OpenShock/bin/Release/net8.0/linux-x64/publish/SubLink.OpenShock.dll" "$BUILD_DIR/Platforms/"
cp "SubLink.Joystick/bin/Release/net8.0/linux-x64/publish/SubLink.Joystick.dll" "$BUILD_DIR/Platforms/"

mkdir -p builds

if [ -f "builds/SubLink_Adult-$VERSION-linux-x64.zip" ]; then
    rm "builds/SubLink_Adult-$VERSION-linux-x64.zip"
fi

zip -r -9 "builds/SubLink_Adult-$VERSION-linux-x64.zip" "$BUILD_DIR/"

rm -rf "$BUILD_DIR"
