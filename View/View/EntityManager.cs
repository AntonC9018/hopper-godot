using System;
using System.Collections.Generic;
using Godot;
using Hopper.View.Animations;

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

        public void ReplaceTiles()
        {
            foreach (int id in TileSet.GetTilesIds())
            {
                var path = TileSet.TileGetTexture(id).ResourcePath;

                foreach (Vector2 pos in GetUsedCellsById(id))
                {
                    // instantiate things at given positions
                }
            }
        }
    }
}