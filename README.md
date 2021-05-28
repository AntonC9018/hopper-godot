# Hopper-Godot

For an overview of the project, see [this repo](https://github.com/AntonC9018/hopper.cs).

## Getting Started
1. Install Godot. Newest version works.
2. Either clone this repo somewhere with submodule init:
```
git clone http://github.com/AntonC9018/hopper-godot hopper-godot --recursive
```

Or clone this repo via the github app (thus non-recursively), then manually initialize the submodules

```
git submodule update --init
```

Now, you need to generate code for `Core`, `TestContent` and the `Godot` project itself.
From the root folder, run the following in the console:
```
cd Hopper/Meta
dotnet run Hopper.Core;../../Hopper/Core/Hopper.Core.csproj Hopper.TestContent;../../Hopper/TestContent/Hopper.TestContent.csproj Hopper.Godot;../../Godot/Hopper.Godot.csproj
```

It will take a few seconds (perhaps a minute) to compile and run.

You may also use `meta.bat` for this.
```
meta all
```