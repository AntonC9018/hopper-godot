using System;
using System.Diagnostics;

namespace Hopper.View.Animations
{
    public abstract class Animator
    {
        /// <summary>
        /// Sets a base class to be used by animations, with specific optimizations included
        /// </summary>

        // a single centralized Stopwatch instead of a local copy for each instance
        private static readonly Stopwatch AnimStopwatch = new Stopwatch();
        protected static readonly float Duration = 0.33f;
        
        private long startTime;
        private bool isRunning = false;

        public bool IsRunning => isRunning;

        static Animator()
        {
            AnimStopwatch.Start();
        }

        protected virtual void StartAnim()
        {
            isRunning = true;
            startTime = AnimStopwatch.ElapsedTicks;
        }

        public virtual void StopAnim()
        {
            isRunning = false;
        }

        protected float GetElapsed()
        {
            // if shit hits the fan
            // convert this back to double
            return (float)(AnimStopwatch.ElapsedTicks - startTime) / Stopwatch.Frequency;
        }

        // Call this every frame
        // Also include the actual animation here
        public virtual void CycleAnim()
        {
            if (!isRunning)
                return;
        }
    }
}