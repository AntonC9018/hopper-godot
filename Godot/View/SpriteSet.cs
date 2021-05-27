using Godot;

namespace Hopper_Godot.View
{
    public class SpriteSet
    {
        public readonly Texture IdleTexture;
        public readonly Texture TelegraphTexture;
        public readonly Texture SlashTexture;

        public SpriteSet(Texture idleTexture, Texture telegraphTexture, Texture slashTexture)
        {
            IdleTexture = idleTexture;
            TelegraphTexture = telegraphTexture;
            SlashTexture = slashTexture;
        }

        public SpriteSet()
        {
            IdleTexture = (Texture)GD.Load("res://Images/Enemies/zombie.png");
            TelegraphTexture = (Texture)GD.Load("res://Images/Enemies/zombie_a.png");
            SlashTexture = (Texture)GD.Load("res://Images/Enemies/zombie_s.png");
        }
    }
}