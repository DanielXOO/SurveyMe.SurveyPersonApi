﻿namespace SurveyPerson.Api.Models.Requests.Options;

public sealed class SurveyOptionsCreateRequestModel
{
    public Guid SurveyId { get; set; }

    public bool RequireFirstName { get; set; }

    public bool RequireSecondName { get; set; }

    public bool RequireGender { get; set; }

    public bool RequireAges { get; set; }
}