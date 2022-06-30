using System;
using UnityEngine;

namespace SK.Framework
{
    public sealed class EveryFrames : ITimer
    {
        private int beginFrame;

        private readonly int duration;

        private int pausedFrame;

        private int remainingFrame;

        private readonly MonoBehaviour executer;

        private Action<EveryFrames> onLaunch;
        private Action<EveryFrames> onExecute;
        private Action<EveryFrames> onPause;
        private Action<EveryFrames> onResume;
        private Action<EveryFrames> onStop;
        private readonly Action<EveryFrames> everyAction;
        private Func<bool> stopWhen;

        private int loops;

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public EveryFrames(Action<EveryFrames> everyAction, int duration = 1, MonoBehaviour executer = null, int loops = -1)
        {
            this.duration = duration;
            this.everyAction = everyAction;
            this.executer = executer;
            this.loops = loops;
        }

        public EveryFrames OnLaunch(Action<EveryFrames> onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public EveryFrames OnExecute(Action<EveryFrames> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public EveryFrames OnPause(Action<EveryFrames> onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public EveryFrames OnResume(Action<EveryFrames> onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public EveryFrames OnStop(Action<EveryFrames> onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public EveryFrames StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }

        public void Launch()
        {
            beginFrame = Time.frameCount;
            onLaunch?.Invoke(this);
            this.Begin(executer ?? Timer.Instance);
        }

        public void Pause()
        {
            IsPaused = true;
            pausedFrame = Time.frameCount;
            onPause?.Invoke(this);
        }

        public void Resume()
        {
            IsPaused = false;
            beginFrame += Time.frameCount - pausedFrame;
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
                remainingFrame = duration - (Time.frameCount - beginFrame);
                onExecute?.Invoke(this);
            }
            if (remainingFrame <= 0)
            {
                everyAction?.Invoke(this);
                if (--loops == 0)
                {
                    IsCompleted = true;
                }
                else
                {
                    beginFrame = Time.frameCount;
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