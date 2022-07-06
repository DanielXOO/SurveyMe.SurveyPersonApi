using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyMe.Common.Exceptions;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using SurveyPersonOptions.Models.Options;
using SurveyPersonOptions.Services.Abstracts;

namespace SurveyPersonOptions.Api.Controllers;

[ApiController]
[Authorize]
[Route("/api/surveys/{surveyId:guid}/[controller]")]
public class SurveyPersonController : Controller
{
    private readonly IOptionsService _optionsService;

    private readonly IMapper _mapper;
    
    
    public SurveyPersonController(IOptionsService optionsService, IMapper mapper)
    {
        _optionsService = optionsService;
        _mapper = mapper;
    }


    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SurveyOptionsResponseModel))]
    [HttpGet]
    public async Task<IActionResult> GetOptions(Guid surveyId)
    {
        var options = await _optionsService.GetBySurveyIdAsync(surveyId);

        var optionsResponse = _mapper.Map<SurveyOptionsResponseModel>(options);
        
        return Ok(optionsResponse);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> EditOptions(SurveyOptionsEditRequestModel editRequestModel, 
        Guid id, Guid surveyId)
    {
        if (editRequestModel == null)
        {
            throw new BadRequestException("Model is empty");
        }

        if (id != editRequestModel.SurveyOptionsId || surveyId != editRequestModel.SurveyId)
        {
            throw new BadRequestException("Ids do not correspond");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                error => error.Key,
                error => error.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            
            throw new BadRequestException("Invalid data", errors);
        }

        var option = _mapper.Map<SurveyOptions>(editRequestModel);

        await _optionsService.UpdateAsync(option);
        
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOptions(Guid id, Guid surveyId)
    {
        await _optionsService.DeleteAsync(id);
        
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> AddOptions(SurveyOptionsCreateRequestModel optionsRequest, Guid surveyId)
    {
        if (optionsRequest == null)
        {
            throw new BadRequestException("Model is empty");
        }

        if (surveyId != optionsRequest.SurveyId)
        {
            throw new BadRequestException("Ids do not correspond");
        }
        
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                error => error.Key,
                error => error.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            
            throw new BadRequestException("Invalid data", errors);
        }

        var options = _mapper.Map<SurveyOptions>(optionsRequest);
        
        var surveyOptions = await _optionsService.CreateAsync(options);

        var surveyOptionsResponse = _mapper.Map<SurveyOptionsResponseModel>(surveyOptions);
        
        return CreatedAtAction(Url.Action(nameof(GetOptions)), surveyOptionsResponse);
    }
}