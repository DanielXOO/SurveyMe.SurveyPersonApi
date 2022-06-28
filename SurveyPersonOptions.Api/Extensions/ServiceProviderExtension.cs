using SurveyPersonOptions.Data.Dapper;
using ILogger = SurveyMe.Common.Logging.Abstracts.ILogger;

namespace SurveyPersonOptions.Api.Extensions;

public static class ServiceProviderExtension
{
    public static void CreateDbIfNotExists(this IServiceProvider serviceProvider, string connectionString)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger>();

            DbInitializer.Initialize(logger ,connectionString);
        }
    }
}