USE SurveyPersonDB

GO

CREATE FUNCTION dbo.GetOptionsBySurveyId(@SurveyId uniqueidentifier)
RETURNS TABLE 
AS
RETURN(SELECT TOP 1 * FROM SurveyPersonOptions
	WHERE SurveyPersonOptions.SurveyId = @SurveyId );

GO

CREATE PROCEDURE AddNewOption @Id uniqueidentifier, @SurveyId uniqueidentifier, 
	@RequireFirstName bit, @RequireSecondName 
	bit, @RequireAges bit, @RequireGender bit 
AS 
BEGIN
	INSERT INTO SurveyPersonOptions(Id, SurveyId, RequireFirstName, RequireSecondName, RequireAges, RequireGender)
	VALUES(@Id, @SurveyId, @RequireFirstName, @RequireSecondName, @RequireAges, @RequireGender)
END

GO

CREATE PROCEDURE DeleteOption @Id uniqueidentifier
AS 
BEGIN
	DELETE FROM SurveyPersonOptions
	WHERE SurveyPersonOptions.Id = @Id
END

GO

CREATE FUNCTION dbo.GetOptionsById(@Id uniqueidentifier)
RETURNS TABLE 
AS
RETURN(SELECT * FROM SurveyPersonOptions
	WHERE SurveyPersonOptions.Id = @Id );

GO

CREATE PROCEDURE UpdateOption @Id uniqueidentifier, @SurveyId uniqueidentifier, 
	@RequireFirstName bit, @RequireSecondName bit,
	@RequireAges bit, @RequireGender bit 
AS 
BEGIN
	UPDATE SurveyPersonOptions
	SET Id = @Id,
	Surveyid = @Surveyid,
	RequireFirstName = @RequireFirstName,
	RequireSecondName = @RequireSecondName,
	RequireAges = @RequireAges,
	RequireGender = @RequireGender
	WHERE SurveyPersonOptions.Id = @Id
END

GO