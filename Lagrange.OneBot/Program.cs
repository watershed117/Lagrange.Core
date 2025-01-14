using System.Reflection;
using System.Runtime;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        GCSettings.LatencyMode = GCLatencyMode.Batch;

        if (!File.Exists("appsettings.json"))
        {
            Console.WriteLine("No exist config file, create it now...");

            var assm = Assembly.GetExecutingAssembly();
            using var istr = assm.GetManifestResourceStream("Lagrange.OneBot.Resources.appsettings.json")!;
            using var temp = File.Create("appsettings.json");
            istr.CopyTo(temp);
        }

        var hostBuilder = new LagrangeAppBuilder(args)
            .ConfigureConfiguration("appsettings.json", false, true)
            .ConfigureBots()
            .ConfigureOneBot();

        hostBuilder.Build().Run();
    }
}