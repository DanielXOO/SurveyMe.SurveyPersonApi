using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyMe.Common.Exceptions;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using SurveyPersonOptions.Models.Options;
using SurveyPersonOptions.Services.Abstracts;

namespace SurveyPersonOptions.Api.Controllers;

/// <summary>
/// Controller for interaction with survey personality options
/// </summary>
[ApiController]
[Authorize]
[Route("/api/surveys/{surveyId:guid}/[controller]")]
public class SurveyPersonController : Controller
{
    private readonly IOptionsService _optionsService;

    private readonly IMapper _mapper;
    
    
    /// <summary>
    /// Controller's constructor
    /// </summary>
    /// <param name="optionsService">Survey options service instance</param>
    /// <param name="mapper">Automapper instance</param>
    public SurveyPersonController(IOptionsService optionsService, IMapper mapper)
    {
        _optionsService = optionsService;
        _mapper = mapper;
    }


    /// <summary>
    /// Endpoint for getting options by survey id
    /// </summary>
    /// <param name="surveyId">Survey id</param>
    /// <returns>Survey personality options</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SurveyOptionsResponseModel))]
    [HttpGet]
    public async Task<IActionResult> GetOptions(Guid surveyId)
    {
        var options = await _optionsService.GetBySurveyIdAsync(surveyId);

        var optionsResponse = _mapper.Map<SurveyOptionsResponseModel>(options);
        
        return Ok(optionsResponse);
    }

    /// <summary>
    /// Endpoint for edit survey options
    /// </summary>
    /// <param name="editRequestModel"></param>
    /// <param name="id"></param>
    /// <param name="surveyId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
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

    /// <summary>
    /// Endpoint for deleting options for survey
    /// </summary>
    /// <param name="id">Options id</param>
    /// <param name="surveyId">Survey id</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOptions(Guid id, Guid surveyId)
    {
        await _optionsService.DeleteAsync(id);
        
        return NoContent();
    }

    /// <summary>
    /// Endpoint for adding options for survey 
    /// </summary>
    /// <param name="optionsRequest">Options model</param>
    /// <param name="surveyId">Survey id</param>
    /// <returns>Created survey model</returns>
    /// <exception cref="BadRequestException">If model is null or id do not match</exception>
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