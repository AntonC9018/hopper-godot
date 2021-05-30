@echo off

cd View
call dotnet restore
cd ..

cd Hopper/Meta

if %1==all (
    call dotnet run Hopper.Core;../../Hopper/Core/Hopper.Core.csproj Hopper.TestContent;../../Hopper/TestContent/Hopper.TestContent.csproj Hopper.View;../../View/Hopper.View.csproj
) else (
    call dotnet run Hopper.Core Hopper.TestContent Hopper.View;../../View/Hopper.View.csproj
)

cd ..
cd ..