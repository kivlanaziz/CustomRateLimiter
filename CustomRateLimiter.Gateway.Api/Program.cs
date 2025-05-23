using CustomRateLimiter.Gateway.Api.RateLimiter;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<CustomRateOptions>(options =>
{
    options.MaxToken = 100;
    options.RefillPerSecond = 0.167;
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<CustomRateLimiterMiddleware>();

var webApiBaseUrl = builder.Configuration["WebApi"];
app.Map("/getUserTickets", async (HttpContext context, [FromServices] IHttpClientFactory httpClientFactory) =>
{
    var userId = context.Request.Query["userId"].ToString();
    var client = httpClientFactory.CreateClient();

    var targetUri = new Uri($"{webApiBaseUrl}/api/Ticket/tickets?userId={userId}");
    var requestMessage = new HttpRequestMessage(HttpMethod.Get, targetUri);

    string[] allowedHeaders = { "Authorization", "User-Agent", "X-Request-ID" };
    foreach (var header in context.Request.Headers)
    {
        if (allowedHeaders.Contains(header.Key, StringComparer.OrdinalIgnoreCase))
            requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
    }

    using var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
    context.Response.StatusCode = (int)response.StatusCode;

    foreach (var header in response.Headers)
    {
        context.Response.Headers[header.Key] = header.Value.ToArray();
    }

    foreach (var header in response.Content.Headers)
    {
        context.Response.Headers[header.Key] = header.Value.ToArray();
    }

    context.Response.Headers.Remove("transfer-encoding");

    await response.Content.CopyToAsync(context.Response.Body);
});


app.Run();
