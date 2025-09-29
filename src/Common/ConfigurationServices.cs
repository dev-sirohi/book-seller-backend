using System.Runtime.CompilerServices;

namespace BSB.src.Common
{
    public class ConfigurationServices
    {
        public static IConfiguration? Configuration {  get; private set; }
        public static string ConnectionString { get; private set; } = string.Empty;
        public static void SetConfiguration(IConfiguration configuration) {
            Configuration = configuration;
        }

        public static IConfiguration GetConfiguration() {
            return Configuration ?? throw new ArgumentNullException();
        }

        public static string? GetConnectionString(string name)
        {
            return ConfigurationServices.Configuration?.GetSection("DB").GetConnectionString(name);
        }
    }
}
