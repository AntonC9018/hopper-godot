# Hopper-Godot

For an overview of the project, see [this repo](https://github.com/AntonC9018/hopper.cs).

## Getting Started

1. Install Godot 3.2.3.
2. Either clone this repo somewhere with submodule init:
```
git clone http://github.com/AntonC9018/hopper-godot hopper-godot --recursive
```

Or clone this repo via the github app (thus non-recursively), then manually initialize the submodules
```
git submodule update --init
```

Now, you need to generate code for `Core`, `TestContent` and the `Godot` project itself.
In the root folder, run:
```
meta fresh
```

It will take a few seconds (perhaps a minute) to compile and run.
Note that it may show a lot of errors, which is expected.

When you need to regenerate the code for the `View` subproject, use `meta` in the console.
```
meta
```

If you're also willing to regenerate code for the nested repository with the model, run `meta all`.
```
meta all
```
