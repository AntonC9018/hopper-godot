@echo off
cd Hopper/Meta

if %1==all (
    call dotnet run Hopper.Core;../../Hopper/Core/Hopper.Core.csproj Hopper.TestContent;../../Hopper/TestContent/Hopper.TestContent.csproj Hopper.Godot;../../Godot/Hopper.Godot.csproj
) else (
    call dotnet run Hopper.Core Hopper.TestContent Hopper.Godot;../../Godot/Hopper.Godot.csproj
)

cd ..
cd ..