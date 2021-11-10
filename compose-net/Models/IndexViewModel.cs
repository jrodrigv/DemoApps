using System;
using System.Net;
using System.Net.Sockets;

namespace app.Models
{
    public class IndexViewModel
    {
        public string HostName => DisplayLocalHostName();

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
