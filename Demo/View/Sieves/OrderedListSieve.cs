using System.Collections.Generic;
using Hopper.Core.History;
using Hopper.Controller;

namespace Hopper.View
{
    public class OrderedListSieve : IViewSieve
    {
        public int Weight { get; private set; }
        public bool IsFull => m_remainingCodes.Count == 0;
        public AnimationCode AnimationCode { get; private set; }

        private UpdateCode[] m_codes;
        private Queue<UpdateCode> m_remainingCodes;

        public OrderedListSieve(AnimationCode animationCode, params UpdateCode[] codes)
        {
            m_codes = codes;
            m_remainingCodes = new Queue<UpdateCode>();
            Reset();
            Weight = codes.Length;
            AnimationCode = animationCode;
        }

        public OrderedListSieve(AnimationCode animationCode, int weight, params UpdateCode[] codes)
        {
            m_codes = codes;
            m_remainingCodes = new Queue<UpdateCode>();
            Reset();
            Weight = weight;
            AnimationCode = animationCode;
        }

        public void Reset()
        {
            m_remainingCodes.Clear();
            foreach (var code in m_codes)
            {
                m_remainingCodes.Enqueue(code);
            }
        }

        public void Sieve(UpdateCode code)
        {
            if (IsFull == false && m_remainingCodes.Peek() == code)
            {
                m_remainingCodes.Dequeue();
            }
        }
    }
}