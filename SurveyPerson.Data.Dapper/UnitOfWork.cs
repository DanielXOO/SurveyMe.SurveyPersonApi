using System.Data;
using SurveyMe.Common.Logging.Abstracts;
using SurveyPerson.Data.Dapper.Abstracts;

namespace SurveyPerson.Data.Dapper;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IDbTransaction _transaction;

    private readonly ILogger _logger;

    public IOptionsRepository Options { get; }
    
    
    public UnitOfWork(IDbTransaction transaction, IOptionsRepository options, ILogger logger)
    {
        _transaction = transaction;
        Options = options;
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