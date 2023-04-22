using System.Runtime.CompilerServices;
using InjectLoggerToServices.Entities;
using Serilog;
using Serilog.Extensions.Logging;

namespace InjectLoggerToServices;

public static class InjectLoggerSample
{
    public static async Task Injecting_services_into_entities()
    {
        #region setup
        PrintSampleName();

        var serviceProvider = RegisterLogger();

        await SeedDbServices(serviceProvider)
            .GetAwaiter()
            .GetResult()
            .UpdateCustomerByName();

        #endregion








    }

    private static void PrintSampleName([CallerMemberName] string? methodName = null)
    {
        Console.WriteLine($">>>> Sample: {methodName}");
        Console.WriteLine();
    }

    private static async Task<IServiceProvider> SeedDbServices(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CustomerContext>();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.AddRangeAsync(
                new Customer { Name = "Alice", PhoneNumber = "+1 515 555 0123" },
                new Customer { Name = "Mac", PhoneNumber = "+1 515 555 0124" });

            await context.SaveChangesAsync();
        }

        return serviceProvider;

    }

    private static async Task UpdateCustomerByName(this IServiceProvider serviceProvider) //where TResult : IServiceProvider
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CustomerContext>();

            var customer = await context.Customers.SingleAsync(e => e.Name == "Alice");
            customer.PhoneNumber = "+1 515 555 0125";
        }
      //  return  serviceProvider;
    }

    public static ServiceProvider RegisterLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        var serviceProvider = new ServiceCollection()
            .AddDbContext<CustomerContext>(
                b => b.UseLoggerFactory(new SerilogLoggerFactory(Log.Logger))
                    .UseSqlServer(
                        @"Server=localhost,1437;"
                        + "Database=Blogs;"
                        + "Persist Security Info=False;User ID=sa;Password=1Secure*Password1;TrustServerCertificate=True;Connection Timeout=30;"
                    ).UseSnakeCaseNamingConvention())
            .BuildServiceProvider();
        return serviceProvider;
    }




}
