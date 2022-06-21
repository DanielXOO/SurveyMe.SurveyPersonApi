USE SurveyPersonDB

CREATE TABLE SurveyPersonOptions(
	Id uniqueidentifier PRIMARY KEY,
	SurveyId uniqueidentifier UNIQUE NOT NULL,
	RequireFirstName bit NOT NULL,
	RequireSecondName bit NOT NULL,
	RequireAges bit NOT NULL,
	RequireGender bit NOT NULL
	)