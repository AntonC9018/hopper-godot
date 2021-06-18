using HopperIntVector2 = Hopper.Utils.Vector.IntVector2;
using GodotVector2 = Godot.Vector2;
using Hopper.Utils;

namespace Hopper.View.Utils
{
    public static class VectorConversions
    {
        // scale also get regulated by the tilemap size 
        public const int TileSize = 32;
        public const float TileSizeInv = 1.0f / TileSize;

        public static GodotVector2 ToSceneVector(this HopperIntVector2 vector) 
            => new GodotVector2(vector.x + 0.5f, vector.y + 0.5f) * TileSize;

        /// <summary>
        /// Converts the given Godot vector, representing the position of an entity in the scene,
        /// to a Hopper vector, representing the position in the logic world.
        /// The vector is rescaled accordingly.
        ///
        /// Adding/subtracting half a tile is required, as (0, 0) stands right between 4 tiles,
        /// thus an offset of (0.5, 0.5) is required to get it all correctly lined up in the scene
        /// </summary>
        public static HopperIntVector2 ToHopperWorldVector(this GodotVector2 vector)
            => new HopperIntVector2(Maths.Round(vector.x * TileSizeInv - 0.5f), Maths.Round(vector.y * TileSizeInv - 0.5f));

        public static HopperIntVector2 Convert(this GodotVector2 vector)
            => new HopperIntVector2(Maths.Round(vector.x), Maths.Round(vector.y));
    }
}