namespace Astro.WebUI;

using System.Diagnostics.CodeAnalysis;
using Astro.WebUI.Components;

[ExcludeFromCodeCoverage]
public class Program
{
    private const int EXIT_SUCCESS = 0;

    private const string SERVICE_NAME = "Astro";
    private const string SERVICE_NAMESPACE = "The Jetsons";

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddObservability(SERVICE_NAME, SERVICE_NAMESPACE);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        app.UseObservability();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseAntiforgery();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        await app.RunAsync();

        return EXIT_SUCCESS;
    }
}
