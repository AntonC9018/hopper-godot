using Hopper.Controller;
using Hopper.Utils.Vector;
using HopperVector2 = Hopper.Utils.Vector.Vector2;
using Godot;

namespace Hopper.View
{
    public class SceneEnt : ISceneEntity, ICamera
    {
        public Node2D m_node;
        private HopperVector2 m_prevPos;
        private bool m_ignoreUpdates;
        private HopperVector2 m_currentFinalState;

        public SceneEnt(Node2D node)
        {
            m_node = node;
        }

        public SceneEnt()
        {
        }

        public void SetInitialOrientation(IntVector2 pos)
        {
            ChangeOrientation(pos);
        }

        public virtual void ChangeOrientation(IntVector2 orientation)
        {
            if (orientation.x != 0)
            {
                m_node.Scale = new Godot.Vector2(
                    System.Math.Abs(m_node.Scale.x) * orientation.x, m_node.Scale.y);
            }
        }

        public void SetInitialPosition(HopperVector2 pos)
        {
            m_prevPos = pos;
            ChangePos(pos);
        }

        public virtual void ChangePos(HopperVector2 pos)
        {
            m_node.Position = new Godot.Vector2(pos.x * Reference.Width, pos.y * Reference.Width);
        }

        public virtual void Destroy()
        {
            m_ignoreUpdates = true;
            m_node.GetParent().RemoveChild(m_node);
        }

        public void Hide()
        {
            m_ignoreUpdates = true;
            m_node.Visible = false;
        }

        public void Show()
        {
            m_ignoreUpdates = false;
            m_node.Visible = true;
        }

        public void EnterPhase(
            Hopper.Core.History.EntityState finalState, ISieve sieve, Controller.AnimationInfo animationInfo)
        {
            if (m_ignoreUpdates)
            {
                return;
            }

            if (sieve != null)
            {
                var viewSieve = (IViewSieve)sieve;
                StartAnimation(viewSieve.AnimationCode);
            }

            m_prevPos = animationInfo.currentPhase > 0 ? m_currentFinalState : m_prevPos;
            m_currentFinalState = finalState.pos;
            ChangeOrientation(finalState.orientation);
        }

        private void StartAnimation(AnimationCode animationCode)
        {
            if (animationCode == AnimationCode.Destroy)
            {
                Destroy();
            }
            // TODO: this should be scalable, obviously
            else if (animationCode == AnimationCode.Jump)
            {
                // m_node.GetComponent<Animator>().Play("Candace_Jump");
            }
        }

        public void Update(Controller.AnimationInfo animationInfo)
        {
            if (m_ignoreUpdates)
            {
                return;
            }
            ChangePos(HopperVector2.Lerp(m_prevPos, m_currentFinalState, animationInfo.proportionIntoPhase));
        }
    }
}