var builder = WebApplication.CreateBuilder();

// Blazor is built on top of Razor syntax (.razor files)
// We also add server-side interactivity
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseAntiforgery(); // This is for Cross-Site-Request-Forgery protection built-in in ASP.NET Core

app.MapRazorComponents<blazorapp.Components.App>() // The full namespace path to App.razor
    .AddInteractiveServerRenderMode();

app.Run();