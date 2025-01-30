
using Hot4.Core.Middleware;
using Hot4.Core.Settings;
using Hot4.DataModel.Data;
using Hot4.Server.Components;
using Hot4.Service;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("HotdbConn");

builder.Services.AddDbContext<HotDbContext>(options =>
    options.UseSqlServer(connectionString));



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.RootDirectory = "/content";
});
builder.Services.AddItemServices();
//builder.Services.AddRazorPages();


builder.Services.Configure<TemplateSettings>(builder.Configuration.GetSection("TemplateSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ValueSettings>(builder.Configuration.GetSection("ValueSettings"));
builder.Services.Configure<NetworkSettings>(builder.Configuration.GetSection("Network_Setting"));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));



var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
