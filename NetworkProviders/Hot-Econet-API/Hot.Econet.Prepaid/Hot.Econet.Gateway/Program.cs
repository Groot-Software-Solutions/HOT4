using Hot.Econet.Prepaid.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

 

app.MapGet("/account-balance-usd", () =>
{
     string URL = "https://emr-econet-zw.patternmatched.com:7001/xmlrpc"; 
 string USDUsername = "CommXmlUSD";
 string USDPassword = "CommXmlUSDPMT20";
var service = new ApiClient(USDUsername, USDPassword, URL);
    var result = service.AccountBalance();
    return result;
})
.WithName("account-balance-usd")
.WithOpenApi();

app.Run();
 
