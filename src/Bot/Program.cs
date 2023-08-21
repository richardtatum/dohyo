using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dohyo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

var host = await Host
    .CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.TryAddScoped<DiscordSocketClient>();
        services.TryAddScoped<CommandService>();
        services.TryAddScoped<LoggingService>();
        services.TryAddScoped<SlashCommandBuilderService>();
        services.TryAddScoped<SlashCommandHandler>();
        services.AddSlashCommands();
    })
    .StartAsync();

var client = host.Services.GetRequiredService<DiscordSocketClient>();
var command = host.Services.GetRequiredService<CommandService>();
var commandBuilder = host.Services.GetRequiredService<SlashCommandBuilderService>();
var commandHandler = host.Services.GetRequiredService<SlashCommandHandler>();
var logger = host.Services.GetRequiredService<LoggingService>();

var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

client.Ready += commandBuilder.OnReadyAsync;
client.Log += logger.LogAsync;
client.SlashCommandExecuted += commandHandler.HandleAsync;

command.Log += logger.LogAsync;

await Task.Delay(Timeout.Infinite);