using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PG_PT_L5_Async
{
    class DNS
    {
        private static string[] hostNames = { "www.microsoft.com", "www.apple.com",
            "www.google.com", "www.ibm.com", "cisco.netacad.net",
            "www.oracle.com", "www.nokia.com", "www.hp.com", "www.dell.com",
            "www.samsung.com", "www.toshiba.com", "www.siemens.com",
            "www.amazon.com", "www.sony.com", "www.canon.com", "www.alcatel-lucent.com",
            "www.acer.com", "www.motorola.com" };

        public static List<(string, string)> GenerateIPs()
        {
            List<(string, string)> result = new List<(string, string)>();
            hostNames.AsParallel().ForAll(
                hostName =>
                {
                    lock (result)
                        result.Add((hostName, Dns.GetHostAddresses(hostName).First().ToString()));
                });
            return result;
        }
    }
}
