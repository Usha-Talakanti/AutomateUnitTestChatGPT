using AutomateUnitTestChatGPT.Extensions;
using AutomateUnitTestChatGPT.Models;
using AutomateUnitTestChatGPT.Service;
using Inventory.Location.Common.Interfaces;
using Retail.OData.Client;

var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot config =
               new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddLocalSettingsJson()
               .AddEnvironmentVariables()
               .Build();

   var configBuild = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddLocalSettingsJson();

// Does App Insights Environment Variable exist?
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY")))
{
    // No. Create App Insights Environment variable from local.settings.json variable
    Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", configBuild.Build()["APPINSIGHTS_INSTRUMENTATIONKEY"]);
}
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("keyVaultBaseUrl")))
{
    // No. Create App Insights Environment variable from local.settings.json variable
    Environment.SetEnvironmentVariable("keyVaultBaseUrl", configBuild.Build()["keyVaultBaseUrl"]);
}
configBuild.AddEnvironmentVariables()
                    .Build();


// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("*") // Angular dev server URL
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureODataService();
builder.Services.AddKeyVaultSecrets("https://csc-resteam-dev.vault.azure.net/", new List<string> { });
Setting.GetResourceGroupName("location-test");

builder.Services.AddOptions();

//builder.Services.Configure<ServiceDetails>(_configuration.GetSection("ServiceDetails"));
//builder.Services.AddApplicationInsightsLogging();
builder.Services.AddTransient<IODataServiceHelper, ODataServiceHelper>();
builder.Services.AddTransient<IDynamicsContextFactory, DynamicsContextFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
