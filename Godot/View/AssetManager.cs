using System.Net.Mime;
using Godot;

namespace Hopper_Godot.View
{
    public static class AssetManager
    {
        public static readonly SpriteSet ZombieSpriteSet;
        
        static AssetManager()
        {
            ZombieSpriteSet = new SpriteSet(
                (Texture)GD.Load("res://Images/Enemies/zombie.png"),
                (Texture)GD.Load("res://Images/Enemies/zombie_a.png"),
                (Texture)GD.Load("res://Images/Enemies/zombie_s.png")
            );
            // ZombieIdle = (Texture)GD.Load("res://Images/Enemies/zombie.png");
            // ZombieTelegraph = (Texture)GD.Load("res://Images/Enemies/zombie_a.png");
        }
    }
}