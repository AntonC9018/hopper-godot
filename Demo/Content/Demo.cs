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
using Hopper.Utils.Vector;

using Hopper.Controller;
using Hopper.Test_Content.Bind;
using Hopper.Test_Content.Status.Freezing;
using Godot.Collections;
using Hopper.View;

namespace Hopper
{
    public class Demo : Godot.Node
    {
        private Nodes m_nodes;
        private World m_world;
        private ViewController m_controller;
        private InputManager m_inputManager;

        public override void _Input(Godot.InputEvent _event)
        {
            if (_event is Godot.InputEventKey
                && m_inputManager.TrySetActionForAllPlayers(m_world, (Godot.InputEventKey)_event))
            {
                m_world.Loop();
            }
        }

        public override void _Ready()
        {
            // The mod loader automatically loads the base mod, which adds most basic stuff
            var modLoader = new ModLoader();
            // The test mod defines some additional content: some status effects, some mobs, bows etc.
            modLoader.Add<TestMod>();
            // The demo mod is the mod used for this demo. It currently sets up basic item and the player.
            modLoader.Add<DemoMod>();
            // This gives out id's to content and stores references in the registry.
            var result = modLoader.RegisterAll();
            // For this demo mod, factories were defined as instance fields.
            // Since it is more convenient to use static fields, the test mod actually defines content statically. 
            var demoMod = result.mods.Get<DemoMod>();

            // Generates the map
            var generator = CreateRunGenerator();

            // The pools are currently used only for chest content.
            // If you're not testing chest content, you may omit this.
            result.patchArea.DefaultPools = new Pools(result.registry);
            result.patchArea.DefaultPools.UsePools(
                itemPool: CreateItemPool(demoMod), // the item pool is used for chest content
                entityPool: new ThrowawayPool()
            );

            m_world = new World(generator.grid.GetLength(1), generator.grid.GetLength(0), result.patchArea);
            m_world.InitializeWorldEvents();

            // Create view_model and hook it up to watch the world events.
            // Once a turn (game loop) completes, it automatically updates the view.
            SetupViewModel(demoMod, m_world);

            for (int y = 0; y < generator.grid.GetLength(1); y++)
            {
                for (int x = 0; x < generator.grid.GetLength(0); x++)
                {
                    if (generator.grid[x, y] != Generator.Mark.EMPTY)
                    {
                        // This is a custom world event, which has nothing to do with logic.
                        // It is used for creating objects for tiles.
                        // I suppose, with a little thought, this could be done more efficiently.
                        TileStuff.CreatedEventPath.Fire(m_world, new IntVector2(x, y));

                        // The walls, however, are relevant to the world, therefore they are 
                        // handled by the normal workflow: Logic Entity -> Scene Entity
                        if (generator.grid[x, y] == Generator.Mark.WALL)
                        {
                            m_world.SpawnEntity(demoMod.WallFactory, new IntVector2(x, y));
                        }
                    }
                }
            }

            // Rooms[0] is the starting room, which all other rooms branch out from.
            IntVector2 center = generator.rooms[0].Center.Round();

            // Players are handled differently. If you are creating a player, not just any entity, 
            // you must call `world.SpawnPlayer()`
            var player = m_world.SpawnPlayer(demoMod.PlayerFactory, center);

            // Some demos follow. 
            // Uncomment and change stuff around as you will to see what it does! 

            /* Spider binds you in place */
            // m_world.SpawnEntity(Spider.Factory, center + IntVector2.Right, new IntVector2(1, 1));

            /* Bounce trap and a wall. */
            // m_world.SpawnEntity(BounceTrap.Factory, center + IntVector2.Right, IntVector2.Right);
            // m_world.SpawnEntity(demoMod.WallFactory, center + IntVector2.Right * 2);

            /* Two bounce traps in a row. */
            // m_world.SpawnEntity(BounceTrap.Factory, center + IntVector2.Right, IntVector2.Right);
            // m_world.SpawnEntity(BounceTrap.Factory, center + IntVector2.Right * 2, IntVector2.Left);

            /* Uncomment to disable bouncing for player. */
            // player.Stats.GetRawLazy(Push.Source.Resistance.Path)[Bounce.Source.Id] = 3;

            /* A blocking trap. When you step on it, it closes you in. */
            // m_world.SpawnEntity(BlockingTrap.Factory, player.Pos + IntVector2.Right);

            /* A dummy you can attack but it wouldn't take damage */
            // m_world.SpawnEntity(Dummy.Factory, player.Pos + IntVector2.Right);

            /* Knife and Shovel basic equipment. */
            player.Inventory.Equip(demoMod.KnifeItem);
            // player.Inventory.Equip(demoMod.ShovelItem);
            // player.Inventory.Equip(demoMod.SpearItem);

            /* Bow. X toggle charge, vector input to shoot */
            // player.Inventory.Equip(Bow.DefaultBow);

            /* Gives the player 10000 bombs. `Space` to use. */
            // player.Inventory.Equip(new PackedItem(new ItemMetadata("bombs"), Bomb.Item, 10000));

            /* Knippers (explody boys). */
            // m_world.SpawnEntity(Knipper.Factory, new IntVector2(center.x + 4, center.y));
            // m_world.SpawnEntity(Knipper.Factory, new IntVector2(center.x + 3, center.y));

            /* A scuffed test robot boss that spawns whelps behind itself and shoots lasers. */
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

            /* Spawn a simple enemy */
            m_world.SpawnEntity(Skeleton.Factory, new IntVector2(center.x + 1, center.y - 1));
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

        private void SetupViewModel(DemoMod demoMod, World world)
        {
            // Nodes is a godot-specific concept. They represent scene entity models in this case. 
            m_nodes = new Nodes(this);
            // Converts user input to action and sets it, for each player.
            m_inputManager = new InputManager();

            var destroyOnDeathSieve = new SimpleSieve(AnimationCode.Destroy, UpdateCode.dead);
            var playerJumpSieve = new SimpleSieve(AnimationCode.Jump, UpdateCode.move_do);

            var timer = GetNode<View.Timer>(new Godot.NodePath("Timer"));
            var camera = GetNode<Godot.Node2D>(new Godot.NodePath("Camera_Container"));
            var animator = new ViewAnimator(new SceneEnt(camera), timer);

            m_controller = new ViewController(animator);
            m_controller.SetDefaultModel(new Model<SceneEnt>(m_nodes._default, destroyOnDeathSieve));

            m_controller.SetModelForFactory(demoMod.PlayerFactory.Id,
                new Model<SceneEnt>(m_nodes.player, destroyOnDeathSieve, playerJumpSieve));
            m_controller.SetModelForFactory(demoMod.WallFactory.Id,
                new Model<SceneEnt>(m_nodes.wall, destroyOnDeathSieve));
            m_controller.SetModelForFactory(demoMod.ChestFactory.Id,
                new Model<SceneEnt>(m_nodes.chest, destroyOnDeathSieve));

            m_controller.SetModelForFactory(Skeleton.Factory.Id,
                new Model<SceneEnt>(m_nodes.enemy, destroyOnDeathSieve));
            m_controller.SetModelForFactory(BombEntity.Factory.Id,
                new Model<SceneEnt>(m_nodes.bomb, destroyOnDeathSieve));
            m_controller.SetModelForFactory(DroppedItem.Factory.Id,
                new Model<RegularRotationSceneEnt>(m_nodes.droppedItem, destroyOnDeathSieve));
            m_controller.SetModelForFactory(Water.Factory.Id,
                new Model<SceneEnt>(m_nodes.water, destroyOnDeathSieve));
            m_controller.SetModelForFactory(IceFloor.Factory.Id,
                new Model<SceneEnt>(m_nodes.iceFloor, destroyOnDeathSieve));
            m_controller.SetModelForFactory(IceCube.Factory.Id,
                new Model<SceneEnt>(m_nodes.iceCube, destroyOnDeathSieve));
            m_controller.SetModelForFactory(BounceTrap.Factory.Id,
                new Model<SceneEnt>(m_nodes.bounceTrap, destroyOnDeathSieve));
            m_controller.SetModelForFactory(Barrier.Factory.Id,
                new Model<RegularRotationSceneEnt>(m_nodes.barrier, destroyOnDeathSieve));
            m_controller.SetModelForFactory(RealBarrier.Factory.Id,
                new Model<RegularRotationSceneEnt>(m_nodes.barrier, destroyOnDeathSieve));
            m_controller.SetModelForFactory(Knipper.Factory.Id,
                new Model<SceneEnt>(m_nodes.knipper, destroyOnDeathSieve));
            m_controller.SetModelForFactory(TestBoss.Factory.Id,
                new Model<SceneEnt>(m_nodes.testBoss, destroyOnDeathSieve));
            m_controller.SetModelForFactory(TestBoss.Whelp.Factory.Id,
                new Model<SceneEnt>(m_nodes.whelp, destroyOnDeathSieve));

            var explosionWatcher = new ExplosionWatcher(m_nodes.explosion);
            var laserBeamWatcher = new LaserBeamWatcher(m_nodes.laserBeamHead, m_nodes.laserBeamBody);
            var tileWatcher = new TileWatcher(new Model<SceneEnt>(m_nodes.tile));
            var predictionWatcher = new PredictionsWatcher(m_nodes.danger);

            Reference.Width = ((Godot.Sprite)m_nodes.player).Texture.GetWidth();

            m_controller.WatchWorld(world, explosionWatcher, tileWatcher, laserBeamWatcher, predictionWatcher);
        }
    }
}

