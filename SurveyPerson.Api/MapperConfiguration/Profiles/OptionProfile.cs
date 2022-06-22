using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options;
using SurveyMe.SurveyPersonApi.Models.Response.Options;
using SurveyPerson.Models.Options;

namespace SurveyPerson.Api.MapperConfiguration.Profiles;

public class OptionProfile : Profile
{
    public OptionProfile()
    {
        CreateMap<SurveyPersonOptions, SurveyOptionsResponseModel>();
        CreateMap<SurveyOptionsCreateRequestModel, SurveyPersonOptions>();
        CreateMap<SurveyOptionsEditRequestModel, SurveyPersonOptions>();
    }
}