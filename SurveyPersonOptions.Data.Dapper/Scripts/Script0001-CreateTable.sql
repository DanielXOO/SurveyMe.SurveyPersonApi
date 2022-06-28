USE SurveyPersonOptionsDb

CREATE TABLE SurveyOptions(
	SurveyOptionsId uniqueidentifier PRIMARY KEY,
	SurveyId uniqueidentifier UNIQUE NOT NULL,
	)

CREATE TABLE PersonalityOptions(
	PersonalityOptionId uniqueidentifier PRIMARY KEY,
	SurveyOptionsId uniqueidentifier NOT NULL,
	PropertyName nvarchar(20) NOT NULL,
	IsRequired bit NOT NULL,
	Type tinyint NOT NULL
	FOREIGN KEY (SurveyOptionsId) REFERENCES SurveyOptions(SurveyOptionsId)
	)