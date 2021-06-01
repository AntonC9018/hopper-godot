using HopperIntVector2 = Hopper.Utils.Vector.IntVector2;
using GodotVector2 = Godot.Vector2;
using Hopper.Utils;

namespace Hopper.View.Utils
{
    public static class VectorConversions
    {
        // scale also get regulated by the tilemap size 
        public const int TileSize = 32;
        public const int TileSizeInv = 1 / TileSize;

        public static GodotVector2 ToSceneVector(this HopperIntVector2 vector) 
            => new GodotVector2(vector.x, vector.y) * TileSize;
        public static HopperIntVector2 ToHopperWorldVector(this GodotVector2 vector)
            => new HopperIntVector2(Maths.Round(vector.x * TileSizeInv), Maths.Round(vector.y * TileSizeInv));
    }
}