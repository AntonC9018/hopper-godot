using System.Collections.Generic;
using Hopper.Core;
using Hopper.Utils.Vector;
using Hopper.Controller;
using Hopper.Test_Content.Explosion;
using Godot;
using World = Hopper.Core.World;
using Hopper.Core.Predictions;
using System;

namespace Hopper.View
{
    public class PredictionsWatcher : IWatcher
    {
        private Model<SceneEnt> m_dangerModel;
        private List<SceneEnt> m_dangers;
        private Predictor m_predictor;

        public PredictionsWatcher(Node2D prefab)
        {
            m_dangerModel = new Model<SceneEnt>(prefab);
            m_dangers = new List<SceneEnt>();
        }

        public void Watch(World world, ViewController vm)
        {
            world.State.EndOfLoopEvent += UpdatePredictions;
            m_predictor = new Predictor(world, Faction.Player);
        }

        private void UpdatePredictions()
        {
            int i = 0;
            foreach (var pos in m_predictor.GetBadPositions())
            {
                if (m_dangers.Count <= i)
                {
                    m_dangers.Add(m_dangerModel.Instantiate(pos, IntVector2.Zero));
                }
                else
                {
                    m_dangers[i].ChangePos(pos);
                    m_dangers[i].Show();
                }
                i++;
            }
            for (; i < m_dangers.Count; i++)
            {
                m_dangers[i].Hide();
            }
        }
    }
}