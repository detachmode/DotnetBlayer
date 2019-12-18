using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace bla
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

            webBuilder.UseStartup<Startup>().UseUrls(
              "http://localhost:5555",
              "https://localhost:5556");
          });
  }
}