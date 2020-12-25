using Hopper.Core.History;
using Hopper.Controller;
using Hopper.Utils.Vector;

namespace Hopper.View
{
    public class SimpleSieve : IViewSieve
    {
        public int Weight { get; private set; }
        public bool IsFull { get; private set; }

        public AnimationCode AnimationCode { get; private set; }

        private UpdateCode m_code;

        public SimpleSieve(AnimationCode animationCode, UpdateCode code, int weight = 1)
        {
            m_code = code;
            Weight = weight;
            AnimationCode = animationCode;
        }

        public void Reset()
        {
            IsFull = false;
        }

        public void Sieve(UpdateCode code)
        {
            if (code == m_code)
            {
                IsFull = true;
            }
        }
    }
}