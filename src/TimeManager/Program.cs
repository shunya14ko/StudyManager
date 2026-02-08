using Communication.Services;
using Microsoft.FluentUI.AspNetCore.Components;
using StudyManager.ServiceDefaults;
using TaskManager.Components;
using TaskManager.Validations.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddServiceDiscovery();
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();
builder.Services.AddHttpClient<HttpClientRest>();

builder.Services.AddScoped<WarningMessageStore>();

var app = builder.Build();

app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // エラー画面は不要なので、コンソールに出力するだけにする
    // app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
