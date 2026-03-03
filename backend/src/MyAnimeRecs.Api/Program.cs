using MyAnimeRecs.Api.Endpoints;
using MyAnimeRecs.Infrastructure.External.Mal;
using MyAnimeRecs.Infrastructure;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' was not found.");

var malClientId = builder.Configuration["MalApi:ClientId"];
if (string.IsNullOrWhiteSpace(malClientId))
{
    throw new InvalidOperationException("MalApi:ClientId is required. Set it in configuration or environment variables.");
}

builder.Services.AddInfrastructure(connectionString, malClientId);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var errorFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = errorFeature?.Error;

        if (exception is MalApiException malException)
        {
            var statusCode = malException.StatusCode switch
            {
                HttpStatusCode.NotFound => StatusCodes.Status404NotFound,
                HttpStatusCode.Unauthorized => StatusCodes.Status401Unauthorized,
                HttpStatusCode.Forbidden => StatusCodes.Status403Forbidden,
                HttpStatusCode.TooManyRequests => StatusCodes.Status429TooManyRequests,
                _ => StatusCodes.Status502BadGateway
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "mal_api_error",
                message = "Failed to fetch data from MyAnimeList API.",
                details = malException.Message
            });
            return;
        }

        if (exception is ArgumentException argumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "bad_request",
                message = argumentException.Message
            });
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "internal_server_error",
            message = "An unexpected error occurred."
        });
    });
});

app.UseHttpsRedirection();

app.MapImportEndpoints();
app.MapRecommendationEndpoints();
app.MapUserEndpoints();

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

app.Run();
