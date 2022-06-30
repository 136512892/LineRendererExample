using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 倒计时/定时器
    /// </summary>
    public sealed class Countdown : ITimer
    {
        private float beginTime;

        private readonly float duration;

        private float pausedTime;

        private readonly bool isIgnoreTimeScale;

        private readonly MonoBehaviour executer;

        private Action<Countdown> onLaunch;
        private Action<Countdown> onExecute;
        private Action<Countdown> onPause;
        private Action<Countdown> onResume;
        private Action<Countdown> onStop;
        private Func<bool> stopWhen;

        /// <summary>
        /// 剩余计时时长
        /// </summary>
        public float RemainingTime { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public Countdown(float duration, bool isIgnoreTimeScale = false, MonoBehaviour executer = null)
        {
            this.duration = duration;
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
        }

        public Countdown OnLaunch(Action<Countdown> onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public Countdown OnExecute(Action<Countdown> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public Countdown OnPause(Action<Countdown> onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public Countdown OnResume(Action<Countdown> onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public Countdown OnStop(Action<Countdown> onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public Countdown StopWhen(Func<bool> predicate)
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
                RemainingTime = duration - ((isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - beginTime);
                RemainingTime = Mathf.Clamp(RemainingTime, 0f, duration);
                onExecute?.Invoke(this);
            }
            IsCompleted = RemainingTime <= 0;
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