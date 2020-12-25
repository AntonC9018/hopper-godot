using System;
using Hopper.Controller;

namespace Hopper.View
{
    public class Timer : Godot.Node, ITimer
    {
        public event Action<int> TimerEvent;

        private double m_millis = 0;
        private bool m_isWorking = false;

        public override void _Process(float delta)
        {
            if (m_isWorking)
            {
                m_millis += delta;
                FireEvent();
            }
        }

        public void Pause()
        {
            m_isWorking = false;
        }

        public void Reset()
        {
            m_millis = 0;
        }

        public void Resume()
        {
            m_isWorking = true;
        }

        public void Start()
        {
            m_isWorking = true;
            Reset();
        }

        public void Stop()
        {
            m_isWorking = false;
            Reset();
        }

        public void Set(int millis)
        {
            m_millis = millis;
        }

        private void FireEvent()
        {
            TimerEvent?.Invoke((int)(m_millis * 1000));
        }
    }
}