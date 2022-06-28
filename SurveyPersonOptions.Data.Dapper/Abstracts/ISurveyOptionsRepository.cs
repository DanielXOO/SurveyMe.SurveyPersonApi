using SurveyPersonOptions.Models.Options;

namespace SurveyPersonOptions.Data.Dapper.Abstracts;

public interface ISurveyOptionsRepository
{
    Task CreateAsync(SurveyOptions options);

    Task<SurveyOptions> GetByIdAsync(Guid id);

    Task<SurveyOptions> GetBySurveyIdAsync(Guid surveyId);
    
    Task UpdateAsync(SurveyOptions options);

    Task DeleteAsync(Guid id);
}