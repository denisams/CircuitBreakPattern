using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao cont�iner.
builder.Services.AddControllers();

// Configura��o do HttpClient com Polly Circuit Breaker
builder.Services.AddHttpClient("HttpClientWithCB")
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    })
    .AddHttpMessageHandler(() =>
        new PolicyHttpMessageHandler(GetCircuitBreakerPolicy()));

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
}

var app = builder.Build();

// Configurar o pipeline de requisi��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
