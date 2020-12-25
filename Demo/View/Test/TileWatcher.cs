using System.Collections.Generic;
using Hopper.Core;
using Hopper.Controller;
using Hopper.Utils.Vector;

namespace Hopper.View
{
    // TODO: move this static class to wherever the generator output is realized
    public static class TileStuff
    {
        public static readonly WorldEventPath<IntVector2> CreatedEventPath = new WorldEventPath<IntVector2>();
    }

    public class TileWatcher : IWatcher
    {
        private Model<SceneEnt> m_tileModel;
        private List<ISceneEntity> m_scents;

        public TileWatcher(Model<SceneEnt> model)
        {
            m_tileModel = model;
            m_scents = new List<ISceneEntity>();
        }

        public void Watch(World world, ViewController vm)
        {
            TileStuff.CreatedEventPath.Subscribe(world, AddTile);
        }

        private void AddTile(IntVector2 pos)
        {
            var scent = m_tileModel.Instantiate(pos, IntVector2.Right);
            m_scents.Add(scent);
        }
    }
}