using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using SurveyPersonOptions.Models.Options;

namespace SurveyPersonOptions.Api.MapperConfiguration.Profiles;

public class OptionProfile : Profile
{
    public OptionProfile()
    {
        CreateMap<PersonalityOptionCreateRequestModel, PersonalityOption>();
        CreateMap<PersonalityOptionEditRequestModel, PersonalityOption>();

        CreateMap<PersonalityOption, PersonalityOptionResponseModel>();
        
        CreateMap<SurveyOptions, SurveyOptionsResponseModel>();
        CreateMap<SurveyOptionsCreateRequestModel, SurveyOptions>();
        CreateMap<SurveyOptionsEditRequestModel, SurveyOptions>();
    }
}