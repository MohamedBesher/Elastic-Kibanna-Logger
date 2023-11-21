using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elastic_Kibanna_Logger.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            _logger.LogError(new ArgumentNullException("userId"), "something went wrong");
            _logger.LogInformation("helllo world");
        }
    }
}