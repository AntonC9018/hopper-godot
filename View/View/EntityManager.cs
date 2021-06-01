using System.Collections.Generic;
using Godot;

namespace Hopper.View
{
    public class EntityManager : TileMap
    {
        private Dictionary<int, PackedScene> KeyToPrefab = new Dictionary<int, PackedScene>()
        {
            {0, AssetManager.BomberPrefab},
            {1, AssetManager.RobotPrefab},
            {2, AssetManager.RobotSummonPrefab},
            {3, AssetManager.ZombiePrefab},
            // {4, AssetManager.PlayerPrefab},
            // {5, AssetManager.ChestPrefab},
            {6, AssetManager.WallPrefab},
            // {7, AssetManager.WaterTilePrefab},
            // {8, AssetManager.TrapPrefab},
        };

        public override void _Ready()
        {
            foreach (var key in KeyToPrefab.Keys)
            {
                var positions = GetUsedCellsById(key);
                
                
            }
        }
    }
}