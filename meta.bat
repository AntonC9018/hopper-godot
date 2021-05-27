@echo off
cd Hopper/Meta
call dotnet run ../Core/Hopper.Core.csproj ../TestContent/Hopper.TestContent.csproj ../../Godot/Hopper.Godot.csproj
cd ..
cd ..