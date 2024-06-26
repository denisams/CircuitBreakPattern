using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure HttpClient with Polly Circuit Breaker and Fallback
builder.Services.AddHttpClient("HttpClientWithCB")
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    })
    .AddPolicyHandler(GetCircuitBreakerPolicy())
    .AddPolicyHandler(GetFallbackPolicy());

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
}

static IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
{
    return Policy<HttpResponseMessage>
        .Handle<BrokenCircuitException>()
        .Or<HttpRequestException>()
        .FallbackAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("Fallback response due to Circuit Breaker.")
        });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

// Map endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
