namespace SK.Framework
{
    public class TimeHelper
    {
        public static float Convert2Seconds(float v, TimeUnit timeUnit) 
        {
            switch (timeUnit)
            {
                case TimeUnit.Millsecond: return v * .001f;
                case TimeUnit.Minute: return v * 60f;
                case TimeUnit.Hour: return v * 3600f;
                case TimeUnit.Day: return v * 3600f * 24f;
                default: return v;
            }
        }

        /// <summary>
        /// HH:mm:ss
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string ToStandardTimeFormat(float seconds)
        {
            int second = (int)seconds;
            int hour = second / 3600;
            int minute = second % 3600 / 60;
            second = second % 3600 % 60;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }

        /// <summary>
        /// HH:mm:ss:fff
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string ToHMSFTimeFormat(float seconds)
        {
            int millseconds = (int)(seconds * 1000);
            int hour = millseconds / 3600000;
            int minute = millseconds % 3600000 / 60000;
            int second = millseconds % 3600000 % 60000 / 1000;
            millseconds = millseconds % 3600000 % 60000 % 1000;
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", hour, minute, second, millseconds);
        }

        /// <summary>
        /// mm:ss:fff
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string ToMSFTimeFormat(float seconds)
        {
            int millseconds = (int)(seconds * 1000);
            int minute = millseconds % 3600000 / 60000;
            int second = millseconds % 3600000 % 60000 / 1000;
            millseconds = millseconds % 3600000 % 60000 % 1000;
            return string.Format("{0:D2}:{1:D2}:{2:D3}", minute, second, millseconds);
        }
    }
}