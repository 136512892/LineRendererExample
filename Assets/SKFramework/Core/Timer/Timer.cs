using System;
using UnityEngine;

namespace SK.Framework
{
    [AddComponentMenu("")]
    public sealed class Timer : MonoBehaviour
    {
        private static Timer instance;

        public static Timer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.Timer]").AddComponent<Timer>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        public static Clock Clock(bool isIgnoreTimeScale = false)
        {
            return new Clock(isIgnoreTimeScale, Instance);
        }

        public static Countdown Countdown(float duration, bool isIgnoreTimeScale = false)
        {
            return new Countdown(duration, isIgnoreTimeScale, Instance);
        }

        public static Chronometer Chronometer(bool isIgnoreTimeScale = false)
        {
            return new Chronometer(isIgnoreTimeScale, Instance);
        }

        public static EverySeconds EverySecond(Action<EverySeconds> everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, 1f, isIgnoreTimeScale, Instance, loops);
        }

        public static EverySeconds EverySeconds(float seconds, Action<EverySeconds> everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, seconds, isIgnoreTimeScale, Instance, loops);
        }

        public static EveryFrames EveryFrame(Action<EveryFrames> everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, 1, Instance, loops);
        }

        public static EveryFrames EveryFrames(int frameCount, Action<EveryFrames> everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, frameCount, Instance, loops);
        } 

        public static EveryFrames NextFrame(Action callback)
        {
            return new EveryFrames(s => callback(), 1, Instance, 1);
        }

        public static Alarm Alarm(int hour, int minute, int second, Action callback)
        {
            return new Alarm(hour, minute, second, callback, Instance);
        }
    }
}