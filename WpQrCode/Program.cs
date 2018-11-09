using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WpQrCode.Helpers;

namespace WpQrCode
{
    public class Program
    {
        public static void Main(string[] args)
        {

            MainAsync(args).Wait();

        }
        static async Task MainAsync(string[] args)
        {
            var url = await AuxNotStatic.GetInfoMotorAux("WpQrCode", 1);
            var host = WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(url.Url)
                .UseIISIntegration()
                .UseStartup<Startup>()
                //.UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
