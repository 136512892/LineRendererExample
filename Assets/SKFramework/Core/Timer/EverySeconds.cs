using System;
using UnityEngine;

namespace SK.Framework
{
    public sealed class EverySeconds : ITimer
    {
        private float beginTime;

        private readonly float duration;

        private float pausedTime;
 
        private float remainingTime;

        private readonly bool isIgnoreTimeScale;

        private readonly MonoBehaviour executer;

        private Action<EverySeconds> onLaunch;
        private Action<EverySeconds> onExecute;
        private Action<EverySeconds> onPause;
        private Action<EverySeconds> onResume;
        private Action<EverySeconds> onStop;
        private readonly Action<EverySeconds> everyAction;
        private Func<bool> stopWhen;

        private int loops;

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public EverySeconds(Action<EverySeconds> everyAction, float duration = 1f, bool isIgnoreTimeScale = false, MonoBehaviour executer = null, int loops = -1)
        {
            this.duration = duration;
            this.everyAction = everyAction;
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
            this.loops = loops;
        }

        public EverySeconds OnLaunch(Action<EverySeconds> onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public EverySeconds OnExecute(Action<EverySeconds> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public EverySeconds OnPause(Action<EverySeconds> onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public EverySeconds OnResume(Action<EverySeconds> onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public EverySeconds OnStop(Action<EverySeconds> onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public EverySeconds StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }

        public void Launch()
        {
            beginTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            onLaunch?.Invoke(this);
            this.Begin(executer ?? Timer.Instance);
        }

        public void Pause()
        {
            IsPaused = true;
            pausedTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            onPause?.Invoke(this);
        }

        public void Resume()
        {
            IsPaused = false;
            beginTime += (isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - pausedTime;
            onResume?.Invoke(this);
        }

        public void Stop()
        {
            IsCompleted = true;
        }

        public bool Execute()
        {
            if (!IsCompleted && !IsPaused)
            {
                remainingTime = duration - ((isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - beginTime);
                remainingTime = Mathf.Clamp(remainingTime, 0f, duration);
                onExecute?.Invoke(this);
            }
            if (remainingTime <= 0) 
            {
                everyAction?.Invoke(this);
                if (--loops == 0)
                {
                    IsCompleted = true;
                }
                else
                {
                    beginTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
                }
            }
            if (!IsCompleted && stopWhen != null && stopWhen.Invoke())
            {
                IsCompleted = true;
            }
            if (IsCompleted)
            {
                onStop?.Invoke(this);
            }
            return IsCompleted;
        }
    }
}