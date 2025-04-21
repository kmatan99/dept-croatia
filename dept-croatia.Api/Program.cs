using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Models;
using dept_croatia.Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Rate limiting and retry policy
// Slightly more strict than MovieDB rate limiting to avoid blocking apikey
var rateLimiter = Policy.RateLimitAsync<HttpResponseMessage>(
    numberOfExecutions: 30,
    perTimeSpan: TimeSpan.FromSeconds(1));

var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(retryCount: 5, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var policyHandler = Policy.WrapAsync(retryPolicy, rateLimiter);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient<IMovieDBService, MovieDBService>().AddPolicyHandler(policyHandler);
builder.Services.AddHttpClient<IYoutubeService, YoutubeService>().AddPolicyHandler(policyHandler);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

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

app.MapControllers();

app.Run();
