namespace Hopper.View
{
    public class ExplosionScent : SceneEnt
    {
        public static readonly int TotalTicks = 3;
        public int m_ticksLeft;

        public object Pos { get; internal set; }

        public ExplosionScent() : base()
        {
            m_ticksLeft = TotalTicks;
        }

        public void Update() => m_ticksLeft--;
        public bool ShouldRemove() => m_ticksLeft == 0;
    }
}