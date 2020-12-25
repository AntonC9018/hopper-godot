using Hopper.Core;
using Hopper.Core.Generation;
using Hopper.Core.History;
using Hopper.Core.Items;
using Hopper.Core.Mods;
using Hopper.Core.Stats.Basic;
using Hopper.Test_Content;
using Hopper.Test_Content.Boss;
using Hopper.Test_Content.Explosion;
using Hopper.Test_Content.Floor;
using Hopper.Test_Content.SimpleMobs;
using Hopper.Test_Content.Status;
using Hopper.Test_Content.Trap;
using Hopper.Utils.Vector;

using Hopper.Controller;
using Hopper.Test_Content.Bind;
using Hopper.Test_Content.Status.Freezing;
using Godot.Collections;

namespace Hopper
{
    public class Demo : Godot.Node
    {
        // public GameObject playerPrefab;
        // public GameObject enemyPrefab;
        // public GameObject wallPrefab;
        // public GameObject tilePrefab;
        // public GameObject chestPrefab;
        // public GameObject droppedItemPrefab;
        // public GameObject bombPrefab;
        // public GameObject explosionPrefab;
        // public GameObject waterPrefab;
        // public GameObject icePrefab;
        // public GameObject bounceTrapPrefab;
        // public GameObject barrierPrefab;
        // public GameObject knipperPrefab;
        // public GameObject testBossPrefab;
        // public GameObject whelpPrefab;
        // public GameObject laserBeamHeadPrefab;
        // public GameObject laserBeamBodyPrefab;

        // public GameObject defaultPrefab;

        private World m_world;
        private ViewController m_controller;
        private InputManager m_inputManager;

        public override void _Input(Godot.InputEvent _event)
        {
            if (m_inputManager.TrySetAction(m_world, _event))
            {
                m_world.Loop();
            }
        }

        public override void _Ready()
        {
            m_inputManager = new InputManager();

            var modLoader = new ModLoader();
            modLoader.Add<TestMod>();
            modLoader.Add<DemoMod>();
            var result = modLoader.RegisterAll();
            var demoMod = result.mods.Get<DemoMod>();

            // Generates the map
            var generator = CreateRunGenerator();

            result.patchArea.DefaultPools = new Pools(result.registry);
            result.patchArea.DefaultPools.UsePools(
                itemPool: CreateItemPool(demoMod),
                entityPool: new ThrowawayPool()
            );
            m_world = new World(generator.grid.GetLength(1), generator.grid.GetLength(0), result.patchArea);
            m_world.InitializeWorldEvents();

            // Create view_model and hook it up to watch the world events
            SetupViewModel(demoMod, m_world);

            for (int y = 0; y < generator.grid.GetLength(1); y++)
            {
                for (int x = 0; x < generator.grid.GetLength(0); x++)
                {
                    if (generator.grid[x, y] != Generator.Mark.EMPTY)
                    {
                        // TileStuff.CreatedEventPath.Fire(m_world, new IntVector2(x, y));

                        if (generator.grid[x, y] == Generator.Mark.WALL)
                        {
                            // m_world.SpawnEntity(m_factories.wallFactory, new IntVector2(x, y));
                        }
                    }
                }
            }

            var center = generator.rooms[0].Center.Round();

            var player = m_world.SpawnPlayer(demoMod.PlayerFactory, center);

            /* Spider binds you in place */
            m_world.SpawnEntity(Spider.Factory, center + IntVector2.Right, new IntVector2(1, 1));

            /* Bounce trap and a wall. */
            // m_world.SpawnEntity(BounceTrap.Factory, center + IntVector2.Right, IntVector2.Right);
            // m_world.SpawnEntity(demoMod.WallFactory, center + IntVector2.Right * 2);

            /* Two bounce traps in a row. */
            // m_world.SpawnEntity(BounceTrap.Factory, center + IntVector2.Right, IntVector2.Right);
            // m_world.SpawnEntity(BounceTrap.Factory, center + IntVector2.Right * 2, IntVector2.Left);

            /* Uncomment to disable bouncing for player. */
            // player.Stats.GetRaw(Push.Source.Resistance.Path)[Bounce.Source.Id] = 3;

            /* A blocking trap. When you step on it, it closes you in. */
            // m_world.SpawnEntity(BlockingTrap.Factory, player.Pos + IntVector2.Right);

            /* A dummy you can attack but it wouldn't take damage */
            // m_world.SpawnEntity(Dummy.Factory, player.Pos + IntVector2.Right);

            /* Knife and Shovel basic equipment. */
            player.Inventory.Equip(demoMod.KnifeItem);
            // player.Inventory.Equip(demoMod.ShovelItem);
            // player.Inventory.Equip(demoMod.SpearItem);

            /* Bow. X toggle charge, vector input to shoot */
            // player.Inventory.Equip(testMod.Item.DefaultBow);

            /* 10000 bombs. `Space` to use. */
            // player.Inventory.Equip(new PackedItem(new ItemMetadata("bombs"), Bomb.Item, 10000));

            /* Knippers (explody boys). */
            // m_world.SpawnEntity(Knipper.Factory, new IntVector2(center.x + 4, center.y));
            // m_world.SpawnEntity(Knipper.Factory, new IntVector2(center.x + 3, center.y));

            /* A test robot boss that spawns whelps behind itself and shoots lasers. */
            // m_world.SpawnEntity(TestBoss.Factory, new IntVector2(center.x + 4, center.y));

            /* A barrier block blocks movement only through one side. The second coordinate is the 
               orientation, which for such blocks defines what side of the cell it is at.
            */
            // m_world.SpawnEntity(Barrier.Factory,
            //    new IntVector2(center.x + 2, center.y), new IntVector2(-1, 0));

            /* A chest contains something depending on the factory. It may spawn preset entities,
               items or draw them from specified in the config pools and gold.
               See how the factory is defined.
            */
            // m_world.SpawnEntity(demoMod.ChestFactory, new IntVector2(center.x + 1, center.y + 1));

            /* Water blocks stop one movement, attack or dig. */
            // m_world.SpawnEntity(Water.Factory, new IntVector2(center.x + 1, center.y + 1));
            // m_world.SpawnEntity(Water.Factory, new IntVector2(center.x, center.y + 1));

            /* Ice makes you slide, skipping movement, attack or dig. */
            // m_world.SpawnEntity(IceFloor.Factory, new IntVector2(center.x - 1, center.y - 1));
            // m_world.SpawnEntity(IceFloor.Factory, new IntVector2(center.x, center.y - 1));
            // m_world.SpawnEntity(IceFloor.Factory, new IntVector2(center.x + 1, center.y - 1));

            /* Apply freezing on player. */
            // Freeze.Status.TryApply(player, new FreezeData(), Freeze.Path.defaultFile);
        }

        private ISuperPool CreateItemPool(DemoMod mod)
        {
            PoolItem[] items = new[]
            {
                new PoolItem(mod.KnifeItem.Id, 1),
                new PoolItem(mod.ShovelItem.Id, 1)
                // new PoolItem(Bombing.item.Id, 1),
                // new PoolItem(Bombing.item_x3.Id, 20)
            };

            var pool = Pool.CreateNormal();

            pool.AddItemToSubpool("zone1/weapons", items[0]);
            pool.AddItemToSubpool("zone1/shovels", items[1]);
            // pool.AddItemToSubpool("zone1/stuff", items[2]);
            // pool.Add("zone1/stuff", items[3]);

            return pool;
            // return pool.Copy();
        }

        private void SetupViewModel(DemoMod demoMod, World world)
        {
            // var destroyOnDeathSieve = new SimpleSieve(AnimationCode.Destroy, UpdateCode.dead);
            // var playerJumpSieve = new SimpleSieve(AnimationCode.Jump, UpdateCode.move_do);

            // View.Timer timer = gameObject.AddComponent<View.Timer>();
            // var animator = new ViewAnimator(new SceneEnt(Camera.main.gameObject), timer);

            // m_controller = new ViewController(animator);
            // m_controller.SetDefaultModel(new Model<SceneEnt>(defaultPrefab, destroyOnDeathSieve));

            // m_controller.SetModelForFactory(demoMod.PlayerFactory.Id,
            //     new Model<SceneEnt>(playerPrefab, destroyOnDeathSieve, playerJumpSieve));
            // m_controller.SetModelForFactory(demoMod.WallFactory.Id,
            //     new Model<SceneEnt>(wallPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(demoMod.ChestFactory.Id,
            //     new Model<SceneEnt>(chestPrefab, destroyOnDeathSieve));

            // m_controller.SetModelForFactory(Skeleton.Factory.Id,
            //     new Model<SceneEnt>(enemyPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(BombEntity.Factory.Id,
            //     new Model<SceneEnt>(bombPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(DroppedItem.Factory.Id,
            //     new Model<RegularRotationSceneEnt>(droppedItemPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(Water.Factory.Id,
            //     new Model<SceneEnt>(waterPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(IceFloor.Factory.Id,
            //     new Model<SceneEnt>(icePrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(BounceTrap.Factory.Id,
            //     new Model<SceneEnt>(bounceTrapPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(Barrier.Factory.Id,
            //     new Model<RegularRotationSceneEnt>(barrierPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(RealBarrier.Factory.Id,
            //     new Model<RegularRotationSceneEnt>(barrierPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(Knipper.Factory.Id,
            //     new Model<SceneEnt>(knipperPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(TestBoss.Factory.Id,
            //     new Model<SceneEnt>(testBossPrefab, destroyOnDeathSieve));
            // m_controller.SetModelForFactory(TestBoss.Whelp.Factory.Id,
            //     new Model<SceneEnt>(whelpPrefab, destroyOnDeathSieve));

            // var explosionWatcher = new ExplosionWatcher(explosionPrefab);
            // var laserBeamWatcher = new LaserBeamWatcher(laserBeamHeadPrefab, laserBeamBodyPrefab);
            // var tileWatcher = new TileWatcher(new Model<SceneEnt>(tilePrefab));

            // Reference.Width = playerPrefab.GetComponent<SpriteRenderer>().size.x;

            // m_controller.WatchWorld(world, explosionWatcher, tileWatcher, laserBeamWatcher);
        }

        private Generator CreateRunGenerator()
        {
            Generator generator = new Generator(11, 11, new Generator.Options
            {
                min_hallway_length = 2,
                max_hallway_length = 5
            });

            generator.AddRoom(new IntVector2(10, 10));
            // generator.AddRoom(new IntVector2(5, 5));
            // generator.AddRoom(new IntVector2(5, 5));
            // generator.AddRoom(new IntVector2(5, 5));
            // generator.AddRoom(new IntVector2(5, 5));
            generator.Generate();
            return generator;
        }
    }
}

