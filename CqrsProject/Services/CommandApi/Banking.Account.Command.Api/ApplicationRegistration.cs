using System.Reflection;
using Banking.Account.Command.Application;
using Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;
using Banking.Account.Command.Application.Models;
using Banking.Account.Command.Infrastructure;
using EventBus.Base;
using EventBus.Kafka;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.Abstraction;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace Banking.Account.Command.Api;

public static class ApplicationRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddApplicationServices(configuration);
        
        services.AddInfrastructureServices(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });


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

        app.UseCors("CorsPolicy");

        app.MapControllers();
        
        return app;
    }
}