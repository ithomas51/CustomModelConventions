using InjectLoggerToServices.Entities;
using InjectLoggerToServices.Extensions;

namespace InjectLoggerToServices;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers
        => Set<Customer>();

    #region OnConfiguring
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(new LoggerInjectionInterceptor());
    #endregion
}
