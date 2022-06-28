USE SurveyPersonOptionsDb

GO

CREATE FUNCTION dbo.GetSurveyOptionsBySurveyId(@SurveyId uniqueidentifier)
RETURNS TABLE 
AS
RETURN(SELECT SurveyOptions.SurveyOptionsId, SurveyOptions.SurveyId,
		PersonalityOptions.PersonalityOptionId, PersonalityOptions.IsRequired,
		PersonalityOptions.PropertyName, PersonalityOptions.Type
	FROM SurveyOptions
	LEFT JOIN PersonalityOptions ON PersonalityOptions.SurveyOptionsId = SurveyOptions.SurveyOptionsId
	WHERE SurveyOptions.SurveyId = @SurveyId);

GO

CREATE PROCEDURE AddNewSurveyOption @SurveyOptionsId uniqueidentifier, @SurveyId uniqueidentifier	
AS 
BEGIN
	INSERT INTO SurveyOptions(SurveyOptionsId, SurveyId)
	VALUES(@SurveyOptionsId, @SurveyId)
END

GO

CREATE PROCEDURE AddNewPersonalityOption @PersonalityOptionId uniqueidentifier, @SurveyOptionsId uniqueidentifier,
	@PropertyName nvarchar(20), @IsRequired bit, @Type tinyint
AS 
BEGIN
	INSERT INTO PersonalityOptions(PersonalityOptionId, SurveyOptionsId, PropertyName, IsRequired, Type)
	VALUES(@PersonalityOptionId, @SurveyOptionsId, @PropertyName, @IsRequired, @Type)
END
GO

CREATE PROCEDURE DeleteSurveyOption @Id uniqueidentifier
AS 
BEGIN
	DELETE FROM PersonalityOptions
	WHERE PersonalityOptions.SurveyOptionsId = @Id


	DELETE FROM SurveyOptions
	WHERE SurveyOptions.SurveyOptionsId = @Id
END

GO

CREATE FUNCTION dbo.GetSurveyOptionsById(@Id uniqueidentifier)
RETURNS TABLE 
AS
RETURN(SELECT SurveyOptions.SurveyOptionsId, SurveyOptions.SurveyId,
		PersonalityOptions.PersonalityOptionId, PersonalityOptions.IsRequired,
		PersonalityOptions.PropertyName, PersonalityOptions.Type
	FROM SurveyOptions
	LEFT JOIN PersonalityOptions ON PersonalityOptions.SurveyOptionsId = SurveyOptions.SurveyOptionsId
	WHERE SurveyOptions.SurveyOptionsId = @Id );

GO

CREATE PROCEDURE EditSurveyOption @SurveyOptionsId uniqueidentifier, @SurveyId uniqueidentifier
AS 
BEGIN
	UPDATE SurveyOptions
	SET SurveyOptionsId = @SurveyOptionsId,
	Surveyid = @SurveyId
	WHERE SurveyOptions.SurveyOptionsId = @SurveyOptionsId
END

GO

CREATE PROCEDURE EditPersonalityOption  @PersonalityOptionsId uniqueidentifier, @PropertyName nvarchar(20),
	@IsRequired bit, @Type tinyint, @SurveyOptionsId uniqueidentifier
	AS 
BEGIN
	UPDATE PersonalityOptions
	SET @PersonalityOptionsId = @PersonalityOptionsId,
	SurveyOptionsId = @SurveyOptionsId,
	PropertyName = @PropertyName,
	IsRequired = @IsRequired,
	Type = @Type
END

GO