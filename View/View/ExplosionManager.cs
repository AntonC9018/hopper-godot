using System.Collections.Generic;
using Hopper.View.Utils;
using Hopper.Shared.Attributes;
using Hopper.TestContent;
using Hopper.Utils.Vector;
using Sprite = Godot.Sprite;

namespace Hopper.View
{
    public static partial class ExplosionManager
    {
        private static List<Sprite> explosionsList = new List<Sprite>();
        private static Sprite explosionPrefab;
        private static int visibleSprites = 0;
        
        // TODO: make sure that ClearExplosions happens first 

        [Export(Chain = "g" + nameof(Explosion) + ".Explosion")]
        public static void AddExplosion(IntVector2 position)
        {
            if (visibleSprites < explosionsList.Count)
            {
                var currentSprite = explosionsList[visibleSprites];
                
                currentSprite.Position = position.ToSceneVector();
                currentSprite.Visible = true;
                
                visibleSprites++;
            }
            else
            {
                var newSprite = (Sprite)explosionPrefab.Duplicate();
                newSprite.Position = position.ToSceneVector();
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