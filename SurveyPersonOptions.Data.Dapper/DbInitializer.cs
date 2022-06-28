using DbUp;
using SurveyMe.Common.Logging.Abstracts;

namespace SurveyPersonOptions.Data.Dapper;

public static class DbInitializer
{
    public static void Initialize(ILogger logger, string connectionString)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var dbUpdater = DeployChanges.To.SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DbInitializer).Assembly).Build();

        if (!dbUpdater.IsUpgradeRequired())
        {
            return;
        }
        
        var result = dbUpdater.PerformUpgrade();

        if (!result.Successful)
        {
            logger.LogCritical(result.Error, "Db initialize error");
        }
    }
}