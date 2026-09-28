using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinanceTracker.Web;
using FinanceTracker.Web.Services.Interfaces;
using FinanceTracker.Web.Services.Implementations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("No se encontró la configuración ApiBaseUrl");

builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new Uri(apiBaseUrl)
});

builder.Services.AddScoped<IUsuarioApiService, UsuarioApiService>();
builder.Services.AddScoped<ICuentaApiService, CuentaApiService>();
builder.Services.AddScoped<ICategoriaApiService, CategoriaApiService>();
builder.Services.AddScoped<ITransaccionApiService, TransaccionApiService>();

await builder.Build().RunAsync();
