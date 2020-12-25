using Hopper.Core;

namespace Hopper.View
{
    public static class Guard
    {
        public static System.Action<T, World> SameWorld<T>(World control, System.Action<T> func)
        {
            return (meta, check) =>
            {
                if (ReferenceEquals(check, control))
                {
                    func(meta);
                }
            };
        }
    }
}