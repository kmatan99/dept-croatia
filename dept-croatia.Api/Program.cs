using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Models;
using dept_croatia.Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Rate limiting and retry policy
var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(retryCount: 5, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient<IMovieDBService, MovieDBService>().AddPolicyHandler(retryPolicy);
builder.Services.AddScoped<ISearchService, SearchService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 20 * 1024 * 1024;
});

builder.Services.Configure<ApiConfig>(builder.Configuration.GetSection("Api"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler(exception =>
{
    exception.Run(async context =>
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        var feature = context.Features.Get<IExceptionHandlerFeature>();

        if (feature?.Error != null)
        {
            logger.LogError(feature.Error, "An unhandled exception occurred.");
        }

        await context.Response.WriteAsync($"An unexpected error occurred. Please try again later. {feature?.Error.Message}");
    });
});

app.MapControllers();

app.Run();
