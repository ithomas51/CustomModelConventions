using Conventions;
using Extensions;

public class BlogsContext : DbContext
{
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Author> Authors => Set<Author>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(
                @"Server=localhost,1437;"
                + "Database=Blogs;"
                + "Persist Security Info=False;User ID=sa;Password=1Secure*Password1;TrustServerCertificate=True;Connection Timeout=30;"
            ).UseSnakeCaseNamingConvention()
            .LogTo(DdlLogger.LogDbInfo, new[] { RelationalEventId.CommandExecuted });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeaturedPost>();

        modelBuilder.Entity<Post>()
            .Property(e => e.Title)
            .HasMaxLength(128);

        modelBuilder.Entity<Post>()
            .HasDiscriminator<string>("PostType")
            .HasValue<Post>("Standard")
            .HasValue<FeaturedPost>("Featured");
    }


    // value converter run only once for every entity
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
       // configurationBuilder.Conventions.Add( ()=> use);
        configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));
        configurationBuilder.Conventions.Add(_ => new MaxStringLengthConvention(256));
        configurationBuilder.Conventions.Add(_ => new DiscriminatorLengthConvention3());
    }
}
