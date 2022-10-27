using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;


namespace TelerikDocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ReportsControllerBase
    {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
             : base(reportServiceConfiguration)
        {
        }
    }
}
