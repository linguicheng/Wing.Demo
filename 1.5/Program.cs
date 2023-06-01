using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Wing;

namespace _1._5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).AddWing(builder => builder.AddConsul());
    }
}
