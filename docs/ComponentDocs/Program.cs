using ComponentDocs.Components;
using ComponentDocs.Data;

var builder = WebApplication.CreateBuilder(args);
// Validate metadata before accepting requests, rather than failing on a visitor's first page.
foreach (var component in Catalog.All)
    _ = ExampleCatalog.Find(component.ComponentType);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
var app = builder.Build();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
