
using Hot4.Core.Settings;
using Hot4.DataModel.Data;
using Hot4.Repository;
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

builder.Services.AddItemServices();
builder.Services.Configure<TemplateSettings>(builder.Configuration.GetSection("TemplateSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));



var app = builder.Build();

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
