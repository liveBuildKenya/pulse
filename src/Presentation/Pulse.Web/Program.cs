using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddWebAuthnApi();
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
