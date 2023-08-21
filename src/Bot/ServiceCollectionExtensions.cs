using System.Reflection;
using Dohyo.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Dohyo;

public static class ServiceCollectionExtensions
{
    public static void AddSlashCommands(this IServiceCollection services)
    {
        var commands = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsClass && x.IsAssignableTo(typeof(ICommand)));

        foreach (var command in commands)
        {
            services.Add(new ServiceDescriptor(typeof(ICommand), command, ServiceLifetime.Scoped));
        }
    }
}