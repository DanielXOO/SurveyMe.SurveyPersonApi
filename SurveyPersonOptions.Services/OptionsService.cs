using SurveyMe.Common.Exceptions;
using SurveyPersonOptions.Data.Dapper.Abstracts;
using SurveyPersonOptions.Models.Options;
using SurveyPersonOptions.Services.Abstracts;

namespace SurveyPersonOptions.Services;

public sealed class OptionsService : IOptionsService
{
    private readonly IUnitOfWork _unitOfWork;
    
    
    public OptionsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<Guid>  CreateAsync(SurveyOptions options)
    {
        var surveyOptionsId = Guid.NewGuid();
        options.SurveyOptionsId = surveyOptionsId;
        
        await _unitOfWork.SurveyOptions.CreateAsync(options);
        
        _unitOfWork.Commit();

        return surveyOptionsId;
    }

    public async Task<SurveyOptions> GetByIdAsync(Guid id)
    {
        var options = await _unitOfWork.SurveyOptions.GetByIdAsync(id);

        if (options == null)
        {
            throw new NotFoundException("Options for survey do not found");
        }
        
        return options;
    }

    public async Task<SurveyOptions> GetBySurveyIdAsync(Guid surveyId)
    {
        var options = await _unitOfWork.SurveyOptions.GetBySurveyIdAsync(surveyId);

        if (options == null)
        {
            throw new NotFoundException("Options for survey do not found");
        }
        
        return options;
    }

    public async Task UpdateAsync(SurveyOptions options)
    {
        await _unitOfWork.SurveyOptions.UpdateAsync(options);
        
        _unitOfWork.Commit();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _unitOfWork.SurveyOptions.DeleteAsync(id);
        
        _unitOfWork.Commit();
    }
}