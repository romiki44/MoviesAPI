using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Filters
{
    // priklad vlastneho filtra
    public class MyActionFilter : IActionFilter
    {
        private readonly ILogger<MyActionFilter> logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            this.logger = logger;
        }

        // pred akciou
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("OnActionExecuting");
        }

        // po akcii
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("OnActionExecuted");
        }

        
    }
}
