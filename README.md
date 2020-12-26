# Getting Started
1. Install Godot.
2. Clone this repo somewhere
```
git clone http://github.com/AntonC9018/hopper-godot hopper-godot
cd hopper-godot
```
3. Run the following commands to get the model code and remove all .csproj files from that. We need to remove them, since Godot has to:
    
    a. have a root .csproj and if we have nested .csproj in this case, they generate errors because of assembly redefinitions; 
    
    b. compile with dotnet 4.7.2, while in the model the version 4.5 is specified;  
```
git clone http://github.com/AntonC9018/hopper.cs Hopper
rm Hopper/*/*.csproj
```


To get the latest model code, do:
```
cd Hopper
git pull origin master
rm */*.csproj
```