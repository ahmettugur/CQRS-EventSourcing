using Banking.Account.Consumer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Banking.Account.Consumer.Infrastructure.Persistence;

public class BankAccountDbContext: DbContext
{
    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options): base(options)
    {
        
    }

    public DbSet<BankAccount>? BankAccounts { get; set; }
    
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BankAccountDbContext>
{
    public BankAccountDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        var builder = new DbContextOptionsBuilder<BankAccountDbContext>();
        if (string.IsNullOrEmpty(connectionString))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("ConnectionString");
        }
        builder.UseSqlServer(connectionString);
        return new BankAccountDbContext(builder.Options);
    }
}

//dotnet ef migrations add "Initial"
//dotnet ef database update