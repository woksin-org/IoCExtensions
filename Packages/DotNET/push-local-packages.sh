#!/bin/bash
version=""
version="${1:-1.0.0-local}"
output="NuGet"
rm -r ./"$output"
dotnet pack --configuration Release -o "$output" -p:Version="$version" -p:PackageVersion="$version"
dotnet nuget push ./"$output"/*.nupkg --source local
