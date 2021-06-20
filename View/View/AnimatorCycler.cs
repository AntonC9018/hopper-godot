using Godot;
using Hopper.Core;
using Hopper.Shared.Attributes;
using Hopper.Utils;
using Hopper.View.Animations;

namespace Hopper.View
{
    public class AnimatorCycler : Node
    {
        private readonly DoubleList<Animator> allAnimators;

        private readonly Index<Animator>[] animatorIndices = {};
        
        public override void _Process(float delta)
        {
            foreach (var animator in allAnimators)
            {
                animator.CycleAnim();
            }
        }

        [Hopper.Shared.Attributes.Export(Chain = "gWorld.SpawnEntity")]
        public void AnalyzeNewEntity(Entity entity)
        {
            if (entity.TryGetComponent(AttackAnim.Index, out var attackAnim))
                allAnimators.AddMaybeWhileIterating(attackAnim);
            
            if (entity.TryGetComponent(MovementAnim.Index, out var movementAnim))
                allAnimators.AddMaybeWhileIterating(movementAnim);
            
            if (entity.TryGetComponent(GetHitAnim.Index, out var getHitAnim))
                allAnimators.AddMaybeWhileIterating(getHitAnim);

            var list = new { };
        }
        
        
    }
}