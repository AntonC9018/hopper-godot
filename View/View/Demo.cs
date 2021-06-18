using System;
using System.Linq;
using Godot;
using Hopper.Core;
using Hopper.Core.Components.Basic;
using Hopper.Core.Mods;
using Hopper.Core.WorldNS;
using Hopper.TestContent.SimpleMobs;
using Hopper.Utils.Vector;
using Hopper.View.Animations;
using World = Hopper.Core.WorldNS.World;

namespace Hopper.View
{
    public class Demo : Node
    {
        [Godot.Export] public NodePath PrefabNodePath { get; set; }
        [Godot.Export] public NodePath TileMapPath { get; set; }

        public Node PrefabNode;
        public TileMap TileMap;

        public override void _Ready()
        {
            PrefabNode = GetNode(PrefabNodePath);
            TileMap = (TileMap) GetNode(TileMapPath);
            
            // Setup all mods
            var loader = new ModLoader();
            loader.Add(Hopper.Core.Main.Init);
            loader.Add(Hopper.TestContent.Main.Init);
            loader.Add(Hopper.View.Main.Init);
            loader.InitMods();

            // Now, the registry contains all of the types defined by the mods.
            // Now we can patch any particular types to include the animator component
            // to provide linkage between the Entity type and the Godot world.
            // This logic may also be put in the BeforeInit() of Hopper.View.Main, 
            // but it would currently not work if the View defined any logic entity types.

            foreach (Node node in PrefabNode.GetChildren())
            {
                // The node names must be in snake_case
                if (!Registry.Global.EntityFactory.TryGetByName(node.Name, out var factory))
                {
                    Console.WriteLine($"{node.Name} does not match an associated entity type in the registry!");
                    continue;
                }

                Animations.EntityAnimator.AddTo(factory, node);

                var subject = factory.subject;

                if (subject.HasAttacking() && node.HasNode(EntityAnimator.SpritePath.Slash))
                    AttackAnim.AddTo(subject);

                if (subject.HasAttackable() && node.HasNode(EntityAnimator.SpritePath.Entity))
                    // last check is kinda redundant as each entity must have an entity sprite
                    GetHitAnim.AddTo(subject);

                if (subject.TryGetTransform(out var transform) && !transform.layer.HasFlag(Layers.WALL)
                                                               && node.HasNode(EntityAnimator.SpritePath.Entity))
                    // again, redundant last check
                    MovementAnim.AddTo(subject);
            }

            // Now that we have set up the registry we can create an empty world.
            // This just creates an empty width * height world, it does not spawn anything.
            // var world = new World(width: 10, height: 10);
            var world = TileMap.CreateWorldOfSameSize();

            // We must set it globally, since all the logic in the game assumes it's global.
            // The entities do not store a reference to the world in the current code, since that's too annoying.
            World.Global = world;

            // If we wanted to load the tilemap, call the tilemap function
            // Make sure, though, that the world is of correct dimensions, 
            // since that function fills up the already existing world!
            TileMap.InstantiateEntities_ForTilesRepresentingEntityTypes();

            // Now, if we spawn an entity, we should get a sprite on the screen, assuming the animator is coded correctly.
            // world.SpawnEntity(Zombie.Factory, IntVector2.Zero, IntVector2.Right);

            // To do an iteration in the logic, do Loop()
            world.Loop();
        }

        public void Finalize()
        {
            // At last, when another scene needs to be loaded, the runtime entities need to be cleared and the world wiped.
            // var world = new World(width: 10, height: 10);
            // World.Global = world;

            // Either clear the runtime entities
            // Registry.Global.RuntimeEntities.Clear();

            // Or reinitialize the registry and reload all mods.
            // This is not required, since we know the only thing that changes are the runtime entities.
            // Registry.Global = new Registry(); Registry.Global.Init(); // then loader stuff again.


            // TODO: Making the GC run here would be a good idea to avoid stutterings while the game is going.
        }
    }
}