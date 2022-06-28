using System.Data;
using SurveyMe.Common.Logging.Abstracts;
using SurveyPersonOptions.Data.Dapper.Abstracts;

namespace SurveyPersonOptions.Data.Dapper;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IDbTransaction _transaction;

    private readonly ILogger _logger;

    public ISurveyOptionsRepository SurveyOptions { get; }
    
    
    public UnitOfWork(IDbTransaction transaction, ISurveyOptionsRepository surveyOptions, ILogger logger)
    {
        _transaction = transaction;
        SurveyOptions = surveyOptions;
        _logger = logger;
    }
    

    public void Commit()
    {
        try
        {
            _transaction.Commit();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Transaction rollback");
            _transaction.Rollback();
        }
    }

    public void Dispose()
    {
        _transaction.Connection?.Close();
        _transaction.Connection?.Dispose();
        _transaction.Dispose();
    }
}