using IdentityServer4.AccessTokenValidation;
using SurveyMe.Common.Logging;
using SurveyPersonOptions.Api.Extensions;
using SurveyPersonOptions.Data.Dapper;
using SurveyPersonOptions.Data.Dapper.Abstracts;
using SurveyPersonOptions.Services;
using SurveyPersonOptions.Services.Abstracts;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logBuilder =>
{
    logBuilder.AddLogger();
    logBuilder.AddFile(builder.Configuration.GetSection("Serilog:FileLogging"));
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDapperConnection(connectionString);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISurveyOptionsRepository, SurveyOptionsRepository>();
builder.Services.AddScoped<IOptionsService, SurveyPersonOptionsService>();

builder.Services.AddAutoMapper(opt =>
{
    opt.AddMaps(typeof(Program).Assembly);
}); 

builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        options.Authority = "https://localhost:7179";
        options.RequireHttpsMetadata = false;
        options.ApiName = "SurveyMeApi";
        options.ApiSecret = "api_secret";
        options.JwtValidationClockSkew = TimeSpan.FromSeconds(1);
    });

var app = builder.Build();

app.Services.CreateDbIfNotExists(connectionString);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();