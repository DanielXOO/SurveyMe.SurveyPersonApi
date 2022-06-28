using SurveyPersonOptions.Models.Options;

namespace SurveyPersonOptions.Services.Abstracts;

public interface IOptionsService
{
    Task CreateAsync(SurveyOptions options);
    
    Task<SurveyOptions> GetByIdAsync(Guid id);

    Task<SurveyOptions> GetBySurveyIdAsync(Guid surveyId);
    
    Task UpdateAsync(SurveyOptions options);

    Task DeleteAsync(Guid id);
}