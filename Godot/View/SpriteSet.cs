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
    }
}