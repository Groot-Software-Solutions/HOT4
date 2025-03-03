using Hot.Econet.API.Installers;
using Hot.Econet.Prepaid.Models;
using Hot.Econet.Prepaid.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

var Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName ?? "")
             .AddJsonFile("appsettings.json", false)
             .AddUserSecrets(Assembly.GetExecutingAssembly())
             .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfiguration>(Configuration);
builder.Services.InstallServicesInAssembly(Configuration);
builder.Services.AddEndpointDefinitions(typeof(Program));


var app = builder.Build();
app.UseEndpointDefinitions();
app.UseHttpsRedirection(); 

app.Run();
