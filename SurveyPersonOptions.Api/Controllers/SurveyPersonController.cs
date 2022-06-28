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
[Route("api/[controller]")]
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
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOptions(Guid id)
    {
        var options = await _optionsService.GetByIdAsync(id);

        var optionsResponse = _mapper.Map<SurveyOptionsResponseModel>(options);
        
        return Ok(optionsResponse);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> EditOptions(SurveyOptionsEditRequestModel editRequestModel, Guid id)
    {
        if (editRequestModel == null)
        {
            throw new BadRequestException("Model is empty");
        }

        if (id != editRequestModel.SurveyOptionsId)
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
    public async Task<IActionResult> DeleteOptions(Guid id)
    {
        await _optionsService.DeleteAsync(id);
        
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> AddOptions(SurveyOptionsCreateRequestModel optionsRequest)
    {
        if (optionsRequest == null)
        {
            throw new BadRequestException("Model is empty");
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

        await _optionsService.CreateAsync(options);
        
        return Ok();
    }
}