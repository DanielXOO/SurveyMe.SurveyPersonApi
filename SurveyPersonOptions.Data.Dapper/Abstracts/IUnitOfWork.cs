namespace SurveyPersonOptions.Data.Dapper.Abstracts;

public interface IUnitOfWork
{
    ISurveyOptionsRepository SurveyOptions { get; }

    void Commit();
}