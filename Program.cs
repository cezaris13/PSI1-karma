using Karma.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Karma
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new KarmaContext();
            var path = db.DbPath;
           // db.Add(new CharityEvent("save someone2", "save them good2", Guid.NewGuid(), CharityEventState.WaitingForApproval));
            //db.SaveChanges();
            var blogs = db.Events
                    .OrderBy(b => b.Id);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, builder) =>
                {
                builder.AddUserSecrets<Program>()
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
