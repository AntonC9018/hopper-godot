@echo off

if "%~1" == ""      goto view
if "%~1" == "all"   goto all
if "%~1" == "fresh" goto fresh

:fresh
echo Restoring afresh
cd Hopper
call meta all
cd ..
cd View
call dotnet build
cd ..
goto view

:view
echo Restoring just the view
cd Hopper/Meta
dotnet run Hopper.Core Hopper.TestContent Hopper.View;../../View/Hopper.View.csproj
cd ..
cd ..
goto the_end

:all
echo Restoring both the model and the view
cd Hopper/Meta
dotnet run Hopper.Core;../../Hopper/Core/Hopper.Core.csproj Hopper.TestContent;../../Hopper/TestContent/Hopper.TestContent.csproj Hopper.View;../../View/Hopper.View.csproj
cd ..
cd ..

goto the_end

:the_end