using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace BasicWeb.Pages
{
    public class IndexModel : PageModel
    {
        public string HostName => DisplayLocalHostName();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public string DisplayLocalHostName()
        {
            var hostName = "Undefined";
            try
            {
                // Get the local computer host name.
                hostName = Dns.GetHostName();

            }
            catch (SocketException)
            {
                // ignored
            }
            catch (Exception)
            {
                // ignored
            }

            return hostName;
        }
    }
}
