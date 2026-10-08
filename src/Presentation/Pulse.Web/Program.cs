using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Pulse.Web.Common;
using Pulse.Web.WebAuthentication.Services;

namespace Pulse.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Default HttpClient for same-origin static files and local API calls
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient("WebAuthnApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7286");
            }).AddHttpMessageHandler<AssertionHandler>();

            builder.Services.AddScoped<WebAuthnApiService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<TemporaryStorage>();
            builder.Services.AddScoped<AssertionHandler>();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddFluxor(configuration =>
            {
                configuration.ScanAssemblies(typeof(Program).Assembly);
                if (builder.HostEnvironment.IsDevelopment())
                    configuration.UseReduxDevTools();
            });

            await builder.Build().RunAsync();
        }
    }
}
