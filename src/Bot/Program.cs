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
        services.TryAddScoped<SlashCommandService>();
        services.TryAddScoped<SlashCommandHandler>();
    });

var x = await builder.StartAsync();

var client = x.Services.GetRequiredService<DiscordSocketClient>();
var command = x.Services.GetRequiredService<CommandService>();
var slashCommand = x.Services.GetRequiredService<SlashCommandService>();
var slashHandler = x.Services.GetRequiredService<SlashCommandHandler>();
var logger = x.Services.GetRequiredService<LoggingService>();

var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

client.Ready += slashCommand.OnReadyAsync;
client.Log += logger.LogAsync;
client.SlashCommandExecuted += slashHandler.HandleAsync;

command.Log += logger.LogAsync;

await Task.Delay(-1);