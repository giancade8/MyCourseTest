﻿using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MyCourse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string firstArguments = args.FirstOrDefault();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.UseHttpSys<>();
                .UseStartup<Startup>();
    }
}
