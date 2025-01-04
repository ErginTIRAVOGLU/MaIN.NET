using MaIN.Domain.Configuration;
using MaIN.Infrastructure.Repositories;
using MaIN.Infrastructure.Repositories.Abstract;
using MaIN.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MaIN.Infrastructure;

public static class Bootstrapper
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = new MaINSettings();
        configuration.GetSection("MaIN").Bind(settings);
        if (settings.MongoDbSettings != null)
        {
            services.AddSingleton<IMongoClient, MongoClient>(sp =>
                new MongoClient(settings.MongoDbSettings?.ConnectionString));

            services.AddSingleton<IChatRepository, MongoChatRepository>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var database = mongoClient.GetDatabase(settings.MongoDbSettings?.DatabaseName!);
                return new MongoChatRepository(database, settings.MongoDbSettings?.ChatsCollection!);
            });

            services.AddSingleton<IAgentRepository, MongoAgentRepository>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var database = mongoClient.GetDatabase(settings.MongoDbSettings?.DatabaseName!);
                return new MongoAgentRepository(database, settings.MongoDbSettings?.AgentsCollection!);
            });

            services.AddSingleton<IAgentFlowRepository, MongoAgentFlowRepository>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var database = mongoClient.GetDatabase(settings.MongoDbSettings?.DatabaseName!);
                return new MongoAgentFlowRepository(database, settings.MongoDbSettings?.FlowsCollection!);
            });
        }
        else
        {
            services.AddSingleton<IAgentFlowRepository, DefaultAgentFlowRepository>();
            services.AddSingleton<IAgentRepository, DefaultAgentRepository>();
            services.AddSingleton<IChatRepository, DefaultChatRepository>();
        }

        return services;
    }
}