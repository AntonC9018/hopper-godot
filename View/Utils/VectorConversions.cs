using HopperIntVector2 = Hopper.Utils.Vector.IntVector2;
using GodotVector2 = Godot.Vector2;

namespace Hopper.View.Utils
{
    public static class VectorConversions
    {
        public static GodotVector2 Convert(this HopperIntVector2 vector) 
            => new GodotVector2(vector.x, vector.y);
    }
}