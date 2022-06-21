using SurveyMe.Common.Logging;
using SurveyPerson.Api.Extensions;
using SurveyPerson.Data.Dapper;
using SurveyPerson.Data.Dapper.Abstracts;
using SurveyPerson.Services;
using SurveyPerson.Services.Abstracts;

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
builder.Services.AddScoped<IOptionsRepository, OptionsRepository>();
builder.Services.AddScoped<IOptionsService, OptionsService>();

builder.Services.AddAutoMapper(opt =>
{
    opt.AddMaps(typeof(Program).Assembly);
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

app.UseAuthorization();

app.MapControllers();

app.Run();