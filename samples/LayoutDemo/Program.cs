using LayoutDemo.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
var app = builder.Build();
app.Use(async (context, next) =>
{
    // Permite explicitamente o uso de unload no documento se necessário
    context.Response.Headers.Append("Permissions-Policy", "unload=(self)");
    await next();
});
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
