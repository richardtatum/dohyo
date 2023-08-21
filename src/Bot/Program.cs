using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dohyo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host
    .CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.TryAddScoped<DiscordSocketClient>();
        services.TryAddScoped<CommandService>();
        services.TryAddScoped<LoggingService>();
        services.TryAddScoped<SlashCommandBuilderService>();
        services.TryAddScoped<SlashCommandHandler>();
    });

var host = await builder.StartAsync();

var client = host.Services.GetRequiredService<DiscordSocketClient>();
var command = host.Services.GetRequiredService<CommandService>();
var slashCommand = host.Services.GetRequiredService<SlashCommandBuilderService>();
var slashHandler = host.Services.GetRequiredService<SlashCommandHandler>();
var logger = host.Services.GetRequiredService<LoggingService>();

var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

client.Ready += slashCommand.OnReadyAsync;
client.Log += logger.LogAsync;
client.SlashCommandExecuted += slashHandler.HandleAsync;

command.Log += logger.LogAsync;

await Task.Delay(-1);