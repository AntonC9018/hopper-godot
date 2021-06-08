using System;
using System.Diagnostics;

namespace Hopper.View.Animations
{
    public abstract class Animator
    {
        /// <summary>
        /// Sets a base class to be used by animations, with specific optimizations included
        /// </summary>
        
        
        // a single centralized stopwatch instead of a local copy for each instance
        private static readonly Stopwatch AnimStopwatch = new Stopwatch();

        private long startTime;
        private bool isRunning = false;

        public bool IsRunning => isRunning;

        // changed type from float to double to prevent possible future issues due to long to float cast 
        protected readonly double Duration = 0.33;

        static Animator()
        {
            AnimStopwatch.Start();
        }

        protected virtual void StartAnim()
        {
            isRunning = true;
            startTime = AnimStopwatch.ElapsedTicks;
        }

        protected virtual void StopAnim()
        {
            isRunning = false;
        }

        protected double GetElapsed()
        {
            return (double)(AnimStopwatch.ElapsedTicks - startTime) / Stopwatch.Frequency;
        }
        
        // Call this every frame
        // Also include the actual animation here
        public virtual void CycleAnim() {}
        
        
    }
}