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

        builder.Services.AddScoped<Reklaim_frontend.Services.AuthService>();

        builder.Services.AddHttpClient<Reklaim_frontend.Services.AuthService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7000/");
        });

        // PostService: typed HttpClient configured to talk to the Hub API
        builder.Services.AddScoped<Reklaim_frontend.Services.PostService>();
        builder.Services.AddHttpClient<Reklaim_frontend.Services.PostService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7026/");
        });

        builder.Services.AddScoped<Reklaim_frontend.Services.ClaimService>();
        builder.Services.AddHttpClient<Reklaim_frontend.Services.ClaimService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7026/");
        });

        // Mock mode for UI development: use in-memory post store. Swap to real PostService when backend is ready.
        // Singleton so posts created during a demo survive page refreshes and show up for every tab.
        builder.Services.AddSingleton<Reklaim_frontend.Services.IPostService, Reklaim_frontend.Services.MockPostService>();
        builder.Services.AddSingleton<Reklaim_frontend.Services.IClaimService, Reklaim_frontend.Services.MockClaimService>();

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
