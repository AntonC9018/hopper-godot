using System.Collections.Generic;
using Godot;

namespace Hopper_Godot.View
{
    public static class ExplosionManager
    {
        private static List<Sprite> explosionsList = new List<Sprite>();
        private static Sprite explosionPrefab;
        private static int visibleSprites = 0;
        
        // TODO: make sure that ClearExplosions happens first 

        public static void AddExplosion(Vector2 position)
        {
            if (visibleSprites < explosionsList.Count)
            {
                var currentSprite = explosionsList[visibleSprites];
                
                currentSprite.Position = position;
                currentSprite.Visible = true;
                
                visibleSprites++;
            }
            else
            {
                var newSprite = (Sprite)explosionPrefab.Duplicate();
                newSprite.Position = position;
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