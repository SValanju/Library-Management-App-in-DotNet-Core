namespace Library_WebAPI.Helpers
{
    public static class AppSettingsHelper
    {
        private static IConfiguration _config;

        public static void Initialize(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static string GetSetting(string key)
        {
            return _config.GetSection(key).Value;
        }
    }
}
