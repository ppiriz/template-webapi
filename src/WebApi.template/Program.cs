using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApi.template
{
    /// <summary>
    /// Main Kestrel entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Startup method for Kestrel.
        /// </summary>
        /// <param name="args">Kestrel runtime arguments.</param>
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder().UseKestrel()
                                           .UseContentRoot(Directory.GetCurrentDirectory())
                                           .UseIISIntegration()
                                           .UseStartup<Startup>()
                                           .UseApplicationInsights()
                                           .Build();

            host.Run();
        }
    }
}
