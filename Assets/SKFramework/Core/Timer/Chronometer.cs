using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SK.Framework
{
    /// <summary>
    /// 秒表
    /// </summary>
    public sealed class Chronometer : ITimer
    {
        public sealed class Record
        {
            public object context;

            public float time;

            public Record(object context, float time)
            {
                this.context = context;
                this.time = time;
            }
        }

        private float beginTime;

        private float pausedTime;

        private readonly bool isIgnoreTimeScale;

        private readonly MonoBehaviour executer;

        private Action<Chronometer> onLaunch;
        private Action<Chronometer> onExecute;
        private Action<Chronometer> onPause;
        private Action<Chronometer> onResume;
        private Action<Chronometer> onStop;
        private Func<bool> stopWhen;

        private readonly List<Record> records;

        public float ElapsedTime { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public ReadOnlyCollection<Record> Records
        {
            get
            {
                return new ReadOnlyCollection<Record>(records);
            }
        }

        public Chronometer(bool isIgnoreTimeScale = false, MonoBehaviour executer = null)
        {
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
            records = new List<Record>();
        }

        public Chronometer OnLaunch(Action<Chronometer> onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public Chronometer OnExecute(Action<Chronometer> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public Chronometer OnPause(Action<Chronometer> onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public Chronometer OnResume(Action<Chronometer> onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public Chronometer OnStop(Action<Chronometer> onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public Chronometer StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }
        public void Shot(object context = null)
        {
            records.Add(new Record(context, ElapsedTime));
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