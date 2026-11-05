using System.Text.Json.Serialization;
using SensorApi.Repositories;
using SensorApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSingleton<IReadingRepository, InMemoryReadingRepository>();
builder.Services.AddScoped<IReadingService, ReadingService>();

var app = builder.Build();

app.MapControllers();

app.Run();