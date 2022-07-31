using Banking.Account.Query.Application;
using Banking.Account.Query.Infrastructure;
using Banking.Account.Query.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Account.Query.Api;

public static class ApplicationRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);

        return services;
    }
    
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();


        //Db Migration
        MigrateDatabase(app);


        return app;
    }
    private static void MigrateDatabase(IHost app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dataContext = scope.ServiceProvider.GetRequiredService<BankAccountDbContext>();
            dataContext.Database.Migrate();
        }
    }


}