using Hopper.Utils.Vector;

namespace Hopper.Controller
{
    public enum AnimationCode
    {
        Destroy, None, Jump
    }

    public interface IAnimationGuide
    {
        AnimationCode AnimationCode { get; }
    }
}