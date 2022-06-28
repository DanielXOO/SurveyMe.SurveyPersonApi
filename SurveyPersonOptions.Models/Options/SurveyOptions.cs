namespace SurveyPersonOptions.Models.Options;

public sealed class SurveyOptions
{
    public Guid SurveyOptionsId { get; set; }

    public Guid SurveyId { get; set; }

    public List<PersonalityOption> Options { get; set; }
}