using Reklaim_frontend.Components;

namespace Reklaim_frontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient<Reklaim_frontend.Services.AuthService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7000/");
        });

        // Typed HttpClients configured to talk to the deployed API.
        builder.Services.AddHttpClient<Reklaim_frontend.Services.PostService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7026/");
        });

        builder.Services.AddHttpClient<Reklaim_frontend.Services.ClaimService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7026/");
        });

        if (builder.Configuration.GetValue<bool>("Api:UseMockServices"))
        {
            // Singleton keeps local demo posts and claims available across circuits.
            builder.Services.AddSingleton<Reklaim_frontend.Services.IPostService, Reklaim_frontend.Services.MockPostService>();
            builder.Services.AddSingleton<Reklaim_frontend.Services.IClaimService, Reklaim_frontend.Services.MockClaimService>();
        }
        else
        {
            // Resolve the interfaces from the typed clients so BaseAddress is preserved.
            builder.Services.AddScoped<Reklaim_frontend.Services.IPostService>(serviceProvider =>
                serviceProvider.GetRequiredService<Reklaim_frontend.Services.PostService>());
            builder.Services.AddScoped<Reklaim_frontend.Services.IClaimService>(serviceProvider =>
                serviceProvider.GetRequiredService<Reklaim_frontend.Services.ClaimService>());
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        // Unknown URLs render the styled 404 page instead of the browser's blank one.
        app.UseStatusCodePagesWithReExecute("/not-found");

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
