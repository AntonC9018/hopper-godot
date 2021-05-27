using System.Collections.Generic;
using Hopper.Core;
using Hopper.Controller;
using Hopper.Test_Content;
using Godot;

namespace Hopper.View
{
    public class LaserScentInfo
    {
        public LaserInfo laser_info;
        public List<RegularRotationSceneEnt> body;
        public RegularRotationSceneEnt head;
    }

    public class LaserBeamWatcher : IWatcher
    {
        private Model<RegularRotationSceneEnt> m_beamHeadModel;
        private Model<RegularRotationSceneEnt> m_beamBodyModel;
        private List<LaserScentInfo> m_activeBeams;

        public LaserBeamWatcher(Node2D headPrefab, Node2D bodyPrefab)
        {
            m_beamHeadModel = new Model<RegularRotationSceneEnt>(headPrefab);
            m_beamBodyModel = new Model<RegularRotationSceneEnt>(bodyPrefab);
            m_activeBeams = new List<LaserScentInfo>();
        }

        public void Watch(Hopper.Core.World world, ViewController vm)
        {
            Hopper.Test_Content.Laser.EventPath.Subscribe(world, AddBeam);
            world.State.StartOfLoopEvent += UpdateBeams;
        }

        public void AddBeam(LaserInfo laser_info)
        {
            var beam_info = new LaserScentInfo();
            beam_info.laser_info = laser_info;
            beam_info.head = m_beamHeadModel.Instantiate(laser_info.pos_start, -laser_info.direction);

            int numBodyElements = (laser_info.pos_end - laser_info.pos_start).Abs().ComponentSum();
            beam_info.body = new List<RegularRotationSceneEnt>(numBodyElements);

            for (int i = 0; i < numBodyElements; i++)
            {
                beam_info.body.Add(m_beamBodyModel.Instantiate(
                    laser_info.pos_start + laser_info.direction * (i + 1),
                    -laser_info.direction));
            }

            m_activeBeams.Add(beam_info);
        }

        private void UpdateBeams()
        {
            foreach (var info in m_activeBeams)
            {
                foreach (var bodyEl in info.body)
                {
                    bodyEl.Destroy();
                }
                info.head.Destroy();
            }
            m_activeBeams.Clear();
        }
    }
}