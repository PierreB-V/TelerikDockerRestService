using Microsoft.Extensions.DependencyInjection.Extensions;
using Telerik.Reporting.Services;
using Telerik.Reporting.Cache.File;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

string reportsPath = builder.Configuration["Reports:Path"];
builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        // The default ReportingEngineConfiguration will be initialized from appsettings.json or appsettings.{EnvironmentName}.json:
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

        
        // In case the ReportingEngineConfiguration needs to be loaded from a specific configuration file, use the approach below:
        //ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IWebHostEnvironment>()),
        HostAppId = "Net6RestServiceWithCors",
        Storage = new FileStorage(),
        ReportSourceResolver = new TypeReportSourceResolver()
            .AddFallbackResolver(new UriReportSourceResolver(reportsPath))


    }); ; ;

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
