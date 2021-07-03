using Microsoft.Extensions.Configuration;

namespace Server.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration) =>
            configuration.GetConnectionString("DefaultConnection");
    }
}
