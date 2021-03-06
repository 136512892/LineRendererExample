using System;
using UnityEngine;

namespace SK.Framework
{
    public interface ISingleton 
    {
        void OnInit();
    }

    public static class Singleton<T> where T : class, ISingleton
    {
        private static T instance;

        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if(null == instance)
                    {
                        instance = Activator.CreateInstance<T>();
                        instance.OnInit();
                    }
                }
                return instance;
            }
        }
        public static void Dispose()
        {
            instance = null;
        }
    }

    public static class MonoSingleton<T> where T : MonoBehaviour, ISingleton
    {
        private static T instance;

        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if(null == instance)
                    {
                        instance = UnityEngine.Object.FindObjectOfType<T>() ?? new GameObject($"[{typeof(T).Name}]").AddComponent<T>();
                        instance.OnInit();
                    }
                }
                return instance;
            }
        }
        public static void Dispose()
        {
            instance = null;
        }
    }
}