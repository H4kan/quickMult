using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using qm.algorithm.MatrixMultiplication;
using qm.algorithm.QmAlgorithm;
using qm.algorithm.QmAlgorithmFactory;
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
            async (RunOptions options) => await app.Run(options),
            async (CompareOptions options) => await app.Compare(options),
            async (GenerateOptions options) => await app.Generate(options),
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

    services.AddSingleton<IMatrixGenerator, MatrixGenerator>();

    services.AddSingleton(typeof(NaiveMultiplication<>));
    services.AddSingleton(typeof(StrassenMultiplication<>));
    services.AddSingleton(typeof(HybridMultiplication<>));

    services.AddSingleton(typeof(IQmAlgorithm<>), typeof(QmAlgorithm<>));
    services.AddSingleton(typeof(IQmAlgorithmFactory<>), typeof(QmAlgorithmFactory<>));
}