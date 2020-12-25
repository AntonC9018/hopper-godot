using System.Collections.Generic;
using Hopper.Core.History;
using Hopper.Controller;

namespace Hopper.View
{
    public class UnorderedListSieve : IViewSieve
    {
        public int Weight { get; private set; }
        public bool IsFull => m_remainingCodes.Count == 0;

        public AnimationCode AnimationCode { get; private set; }

        private UpdateCode[] m_codes;
        private HashSet<UpdateCode> m_remainingCodes;

        public UnorderedListSieve(AnimationCode animationCode, params UpdateCode[] codes)
        {
            m_codes = codes;
            m_remainingCodes = new HashSet<UpdateCode>(codes);
            Weight = codes.Length;
            AnimationCode = animationCode;
        }

        public UnorderedListSieve(AnimationCode animationCode, int weight, params UpdateCode[] codes)
        {
            m_codes = codes;
            m_remainingCodes = new HashSet<UpdateCode>(codes);
            Weight = weight;
            AnimationCode = animationCode;
        }

        public void Reset()
        {
            m_remainingCodes.UnionWith(m_codes);
        }

        public void Sieve(UpdateCode code)
        {
            if (m_remainingCodes.Contains(code))
            {
                m_remainingCodes.Remove(code);
            }
        }
    }
}