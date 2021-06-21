using Microsoft.Extensions.Configuration;
using System;

namespace Training.Models.V2.Tool
{
    public class Setting
    {
        private static IConfiguration Configuration { get; set; }

        static Setting()
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        }

        public static string GovOpenData { get { return Configuration.GetSection("Setting:GovOpenData").Value; } }
        public static string RoadUrl { get { return Configuration.GetSection("Setting:RoadUrl").Value; } }
    }
}
