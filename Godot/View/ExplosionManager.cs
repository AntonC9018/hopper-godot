using System.Collections.Generic;
using Hopper.Godot.Utils;
using Hopper.Shared.Attributes;
using Hopper.TestContent;
using Hopper.Utils.Vector;
using Sprite = Godot.Sprite;

namespace Hopper.Godot.View
{
    public static partial class ExplosionManager
    {
        private static List<Sprite> explosionsList = new List<Sprite>();
        private static Sprite explosionPrefab;
        private static int visibleSprites = 0;
        
        // TODO: make sure that ClearExplosions happens first 

        [Export(Chain = "@" + nameof(Explosion) + ".Explosion")]
        public static void AddExplosion(IntVector2 position)
        {
            if (visibleSprites < explosionsList.Count)
            {
                var currentSprite = explosionsList[visibleSprites];
                
                currentSprite.Position = position.Convert();
                currentSprite.Visible = true;
                
                visibleSprites++;
            }
            else
            {
                var newSprite = (Sprite)explosionPrefab.Duplicate();
                newSprite.Position = position.Convert();
                explosionsList.Add(newSprite);
            }
        }

        public static void ClearExplosions()
        {
            foreach (var sprite in explosionsList)
            {
                sprite.Visible = false;
            }

            visibleSprites = 0;
        }
    }
}