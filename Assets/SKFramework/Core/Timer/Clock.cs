using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 时钟/计时器
    /// </summary>
    public sealed class Clock : ITimer
    {
        private float beginTime;

        private float pausedTime;

        private readonly bool isIgnoreTimeScale;

        private readonly MonoBehaviour executer;

        private Action<Clock> onLaunch;
        private Action<Clock> onExecute;
        private Action<Clock> onPause;
        private Action<Clock> onResume;
        private Action<Clock> onStop;
        private Func<bool> stopWhen;

        /// <summary>
        /// 已经计时
        /// </summary>
        public float ElapsedTime { get; private set; }
       
        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public Clock(bool isIgnoreTimeScale = false, MonoBehaviour executer = null)
        {
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
        }

        public Clock OnLaunch(Action<Clock> onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public Clock OnExecute(Action<Clock> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public Clock OnPause(Action<Clock> onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public Clock OnResume(Action<Clock> onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public Clock OnStop(Action<Clock> onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public Clock StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }

        public void Launch()
        {
            beginTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            onLaunch?.Invoke(this);
            this.Begin(executer?? Timer.Instance);
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
                ElapsedTime = (isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - beginTime;
                onExecute?.Invoke(this);
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