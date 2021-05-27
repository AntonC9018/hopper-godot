using System.Collections.Generic;
using Hopper.Core;
using Hopper.Utils.Vector;
using Hopper.Controller;
using Hopper.Test_Content.Explosion;
using Godot;
using World = Hopper.Core.World;

namespace Hopper.View
{
    public class ExplosionWatcher : IWatcher
    {
        private Model<ExplosionScent> m_explosionModel;
        private List<ExplosionScent> m_goingExplosions;

        public ExplosionWatcher(Node2D prefab)
        {
            m_explosionModel = new Model<ExplosionScent>(prefab);
            m_goingExplosions = new List<ExplosionScent>();
        }

        public void Watch(World world, ViewController vm)
        {
            Explosion.EventPath.Subscribe(world, AddExplosion);
            world.State.EndOfLoopEvent += UpdateGoingExplosions;
        }

        private void AddExplosion(IntVector2 pos)
        {
            var scent = m_explosionModel.Instantiate(pos, IntVector2.Right);
            m_goingExplosions.Add(scent);
        }

        private void UpdateGoingExplosions()
        {
            for (int i = m_goingExplosions.Count - 1; i >= 0; i--)
            {
                m_goingExplosions[i].Update();
                if (m_goingExplosions[i].ShouldRemove())
                {
                    m_goingExplosions[i].Destroy();
                    m_goingExplosions.RemoveAt(i);
                }
            }
        }
    }
}