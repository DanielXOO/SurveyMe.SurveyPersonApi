using AutoMapper;
using SurveyPerson.Api.Models.Requests.Options;
using SurveyPerson.Api.Models.Response.Options;
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