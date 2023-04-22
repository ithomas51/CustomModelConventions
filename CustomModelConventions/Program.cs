using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
    .WriteTo.File("serilog-file.txt")
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up...");
using (var context = new BlogsContext())
{
    context.Database.EnsureDeleted();

    await Task.Run(
        async () =>
            await context.Database.EnsureCreatedAsync()
    );

    await Task.Run(
        () =>
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    );
}

Console.WriteLine("Done.");
Environment.Exit(0);
