namespace Astro.WebUI;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Astro.WebUI.Components;
using Microsoft.AspNetCore.DataProtection;

[ExcludeFromCodeCoverage]
public class Program
{
    private const int EXIT_SUCCESS = 0;

    private const string SERVICE_NAME = "Astro";
    private const string SERVICE_NAMESPACE = "The Jetsons";

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory,
        });

        builder.AddObservability(SERVICE_NAME, SERVICE_NAMESPACE);
        ConfigureDataProtection(builder);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseObservability();

        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Astro API v1");
        });

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

    private static void ConfigureDataProtection(WebApplicationBuilder builder)
    {
        var keysPath = Environment.GetEnvironmentVariable("DATA_PROTECTION_KEYS_PATH")
            ?? Path.Combine(builder.Environment.ContentRootPath, ".aspnet", "DataProtection-Keys");

        Directory.CreateDirectory(keysPath);

        var dataProtectionBuilder = builder.Services.AddDataProtection()
            .SetApplicationName(SERVICE_NAME)
            .PersistKeysToFileSystem(new DirectoryInfo(keysPath));

        var certificatePath = Environment.GetEnvironmentVariable("DATA_PROTECTION_CERT_PATH");
        var certificatePassword = Environment.GetEnvironmentVariable("DATA_PROTECTION_CERT_PASSWORD");

        if (!string.IsNullOrWhiteSpace(certificatePath)
            && !string.IsNullOrWhiteSpace(certificatePassword)
            && File.Exists(certificatePath))
        {
            var certificate = X509CertificateLoader.LoadPkcs12FromFile(
                certificatePath,
                certificatePassword,
                X509KeyStorageFlags.DefaultKeySet);

            dataProtectionBuilder.ProtectKeysWithCertificate(certificate);
        }
    }
}
