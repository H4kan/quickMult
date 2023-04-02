using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using qm.console;
using qm.console.Options;
using qm.generator;
using qm.reader;
using qm.utils.Interfaces;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var app = services.GetRequiredService<App>();
    await Parser.Default.ParseArguments<RunOptions, CompareOptions, GenerateOptions>(args)
        .MapResult(
            (RunOptions options) => app.Run(options),
            (CompareOptions options) => app.Compare(options),
            (GenerateOptions options) => app.Generate(options),
            err => Task.FromResult(-1)
        );
}
catch (Exception)
{
    await SomethingWenWrong();
}


static Task SomethingWenWrong()
{
    Console.WriteLine("Something went wrong, check the entered arguments and try again.");
    return Task.FromResult(-1);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) => ConfigureServices(services));
}

static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<App>();
    services.AddSingleton<IQmReader, QmReader>();
    services.AddSingleton<IQmWriter, QmWriter>();

    services.AddScoped<IMatrixGenerator, MatrixGenerator>();
}