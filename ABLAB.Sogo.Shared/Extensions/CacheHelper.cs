namespace ABLAB.Sogo.Shared.Extensions
{
    public static class CacheHelper
    {
        private const double DefaultCacheHours = 3;

        public static bool CacheOutOfDate(DateTime lastUpdate, TimeSpan? cacheDuration = null)
        {
            cacheDuration ??= TimeSpan.FromHours(DefaultCacheHours);
            return DateTime.Now - lastUpdate > cacheDuration;
        }
    }
}
