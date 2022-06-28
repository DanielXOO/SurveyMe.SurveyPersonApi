using System.Data;
using System.Data.SqlClient;
using Dapper;
using SurveyPersonOptions.Data.Dapper.Abstracts;
using SurveyPersonOptions.Models.Options;

namespace SurveyPersonOptions.Data.Dapper;

public sealed class SurveyOptionsRepository : ISurveyOptionsRepository
{
    private readonly IDbTransaction _transaction;

    private readonly SqlConnection _connection;
    
    
    public SurveyOptionsRepository(SqlConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }
    
    
    public async Task<Guid> CreateAsync(SurveyOptions options)
    {
        const string sqlCreateSurveyOption = "EXEC AddNewSurveyOption @SurveyOptionsId, @SurveyId";

        var surveyOptionId = Guid.NewGuid();
        
        await _connection.ExecuteAsync(sqlCreateSurveyOption, new
        {
            SurveyOptionsId = surveyOptionId,
            SurveyId = options.SurveyId,
        }, _transaction);

        const string sqlCreatePersonalityOption = 
            "EXEC AddNewPersonalityOption @PersonalityOptionId, @SurveyOptionsId, @PropertyName, @IsRequired, @Type";
        
        foreach (var personalityOption in options.Options)
        {
            await _connection.ExecuteAsync(sqlCreatePersonalityOption, new
            {
                PersonalityOptionId = Guid.NewGuid(),
                SurveyOptionsId = surveyOptionId,
                PropertyName = personalityOption.PropertyName,
                IsRequired = personalityOption.IsRequired,
                Type = personalityOption.Type
            }, _transaction);
        }

        return surveyOptionId;
    }

    public async Task<SurveyOptions> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM GetSurveyOptionsById(@Id)";

        var optionsMap = new Dictionary<Guid, SurveyOptions>();
        
        await _connection.QueryAsync<SurveyOptions, PersonalityOption, SurveyOptions>(sql, 
            (surveyOptions, personalityOption) =>
            {
                personalityOption.SurveyOptionsId = surveyOptions.SurveyOptionsId;

                if (optionsMap.TryGetValue(surveyOptions.SurveyOptionsId, out var existingOption))
                {
                    surveyOptions = existingOption;
                }
                else
                {
                    surveyOptions.Options = new List<PersonalityOption>();
                    optionsMap.Add(surveyOptions.SurveyOptionsId, surveyOptions);
                }

                surveyOptions.Options.Add(personalityOption);

                return surveyOptions;
            }, 
            param: new { Id = id }, 
            splitOn: "PersonalityOptionId", 
            transaction: _transaction);
        
        optionsMap.TryGetValue(id, out var options);
        
        
        return options;
    }

    public async Task<SurveyOptions> GetBySurveyIdAsync(Guid surveyId)
    {
        const string sql = "SELECT * FROM GetSurveyOptionsBySurveyId(@SurveyId)";

        var options = await _connection
            .QueryFirstOrDefaultAsync<SurveyOptions>(sql, new
            {
                SurveyId = surveyId
            }, _transaction);

        return options;
    }

    public async Task UpdateAsync(SurveyOptions options)
    {
        const string sql = "EXEC EditPersonalityOption  @PersonalityOptionsId, @PropertyName," +
                           " @IsRequired, @Type, @SurveyOptionsId";

        foreach (var personalityOption in options.Options)
        {
            await _connection.ExecuteAsync(sql, new
            {
                PersonalityOptionsId = personalityOption.PersonalityOptionId,
                PropertyName = personalityOption.PropertyName,
                IsRequired = personalityOption.IsRequired,
                Type = personalityOption.Type,
                SurveyOptionsId = personalityOption.SurveyOptionsId
            }, _transaction);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = "EXEC DeleteSurveyOption @Id";

        await _connection.ExecuteAsync(sql, new
        {
            Id = id
        }, _transaction);
    }
}