using Godot;

namespace Hopper.View
{
    public static class AssetManager
    {
        public static readonly PackedScene ZombiePrefab;
        public static readonly PackedScene RobotPrefab;
        public static readonly PackedScene RobotSummonPrefab;
        public static readonly PackedScene BomberPrefab;
        public static readonly PackedScene WallPrefab;
        public static readonly PackedScene WaterTilePrefab;
        public static readonly PackedScene PlayerPrefab;
        public static readonly PackedScene TrapPrefab;
        public static readonly PackedScene ChestPrefab;


        static AssetManager()
        {
            var defaultSlashTexture = (Texture) GD.Load("res://Images/Enemies/Zombie_s.png");

            // this kinda shouldn't be here, and entities should load up out of empty entityAnimators with the assigned spriteSet
            ZombiePrefab = ResourceLoader.Load<PackedScene>("res://View/Prefabs/Zombie.tscn");
            RobotPrefab = ResourceLoader.Load<PackedScene>("res://View/Prefabs/Robot.tscn");
            RobotSummonPrefab = ResourceLoader.Load<PackedScene>("res://View/Prefabs/RobotSummon.tscn");
            BomberPrefab = ResourceLoader.Load<PackedScene>("res://View/Prefabs/Bomber.tscn");
            WallPrefab = ResourceLoader.Load<PackedScene>("res://View/Prefabs/Wall.tscn");
        }
    }
}