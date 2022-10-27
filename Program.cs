using Microsoft.Extensions.DependencyInjection.Extensions;
using Telerik.Reporting.Services;
using Telerik.Reporting.Cache.File;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var reportingAssembly = Assembly.Load("Reporting");
if (null != reportingAssembly)
{
    builder.Services.AddControllers()
        .AddApplicationPart(reportingAssembly)
        .AddNewtonsoftJson();
        
    builder.Services.AddReportingService(builder.Configuration);
}
else
{
    builder.Services.AddControllers();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());



app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
