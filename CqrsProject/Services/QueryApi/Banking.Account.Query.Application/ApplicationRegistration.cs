using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Account.Query.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        return services;
    }
    
}