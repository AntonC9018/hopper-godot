using Hopper.Utils.Vector;

namespace Hopper.View
{
    public class RegularRotationSceneEnt : SceneEnt
    {
        public override void ChangeOrientation(IntVector2 orientation)
        {
            double angle = orientation.AngleTo(IntVector2.UnitX);
            m_node.Rotation = (float)angle;
        }
    }
}