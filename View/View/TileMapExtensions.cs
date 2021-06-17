using System;
using Hopper.Core;
using Hopper.Core.WorldNS;
using Hopper.View.Animations;
using Hopper.View.Utils;
using HopperIntVector2 = Hopper.Utils.Vector.IntVector2;

namespace Hopper.View
{
	public static class TileMapExtensions
	{
        public static World CreateWorldOfSameSize(this Godot.TileMap tileMap)
        {
            var size = tileMap.CellSize.Convert();
            return new World(size.x, size.y);
        }

		/// <summary>
		///
		/// Instantiates an entity for each tiles in the tilemap which represents a logical entity.
		/// Removes all such tiles, effectively replacing them with fresh entities.
		/// The entities are spawned in the <c>World.Global</c> Hopper world, 
		/// while their duplicated prefabs appear in the scene (currently unimplemented). 
		///
		/// The tiles that do not represent an entity are ignored.
		/// In order to aid debugging, in debug mode, a message is printed 
		/// if an associated entity type has not been found.
		///
		/// The textures associated with the tiles must have their name match the name of the desired entity type.
		/// Currently, the name must exactly match the name of the script, associated with the given entity type. 
		///
		/// </summary>
		public static void InstantiateEntities_ForTilesRepresentingEntityTypes(this Godot.TileMap tileMap)
		{
			foreach (int id in tileMap.TileSet.GetTilesIds())
			{
				var path = tileMap.TileSet.TileGetTexture(id).ResourcePath;

				// TODO: 
				// Entities with more than one sprite might have more information to the name of the sprite file. 
				// E.g. the "_default" part in Images/Player_default.png
				// This part has to either follow some pattern, so that it could be stripped with a regex, 
				// or we should rethink the way we're getting the entity types associated with a given tile.
				//
				// A different approach would be to make our own godot extension for tile map creation,
				// where we would reference prefabs instead of textures.
				// This would be a lot of work though, and comes with its own issues.
				var className = System.IO.Path.GetFileNameWithoutExtension(path);
				
				if (!Registry.Global.EntityFactory.TryGetByName(className, out var factory))
				{
#if DEBUG
					Console.WriteLine($"Ignoring {className} since it did not match any entity factories in the registry.");
					Godot.GD.Print($"Ignoring {className} since it did not match any entity factories in the registry.");
#endif
					continue;
				}

				foreach (Godot.Vector2 floatPosition in tileMap.GetUsedCellsById(id))
				{
					var intPosition = floatPosition.Convert();

					// Determine the direction the given tile has been rotated
					var orientation = tileMap.GetCellOrientation(intPosition);

					// Since we assume the factory has been set up previously, 
					// a sprite should also be created at the given position in the scene.
					Entity entity = World.Global.SpawnEntity(factory, intPosition, orientation);
					
					// TODO: remove cell at `pos`.
				}
			}
		}

		/// <summary>
		/// Gets the rotation of the cell, represented by a hopper vector.
		/// The default orientation is to the right: (1, 0).
		/// With this function, it is impossible to get a diagonal orientation, 
		/// or any orientation other than the 4 orthogonal directions.
		/// </summary>
		public static HopperIntVector2 GetCellOrientation(this Godot.TileMap tileMap, HopperIntVector2 position)
		{
			bool isTransposed = tileMap.IsCellTransposed(position.x, position.y);
			bool isYFlipped   = tileMap.IsCellYFlipped(position.x, position.y);

			if (isTransposed) 
			{
				return new HopperIntVector2(0, isYFlipped ? -1 : 1);
			}

			bool isXFlipped = tileMap.IsCellXFlipped(position.x, position.y);
			return new HopperIntVector2(isXFlipped ? -1 : 1, 0);
		}
	}
}
