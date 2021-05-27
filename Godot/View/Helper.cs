using Godot;

namespace Hopper.Godot.View
{
    public static class Helper
    {
        public static Vector2 LinearInterpolation(Vector2 startPos, Vector2 destPos, float duration, float x)
        {
            return startPos + (destPos - startPos) * (x / duration);
        }

        public static float SquareInterpolation(float peak, float duration, float x)
        {
            // gets value of x from parabola defined by these 3 points:
            // (0, 0)
            // (duration/2, peak)
            // (duration, 0)
            // simplified to the given formula:

            var result = (4 * peak / duration) * (-x * x / duration + x);
            
            return result;
        }
        
        // TODO: try cubic curves
    }
}