using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ScheduleBot.Commands;

public class CommandDispatcher
{
    private readonly Dictionary<string, ICommand> _commands = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string trigger, ICommand command)
    {
        _commands[trigger] = command;
    }

    public async Task DispatchAsync(Update update, ITelegramBotClient botClient, CancellationToken ct)
    {
        if (update.Message == null || update.Message.Type != MessageType.Text)
            return;

        var text = update.Message.Text!.Trim();
        var cmd = text.Split(' ', 2)[0];

        if (_commands.TryGetValue(cmd, out var command))
        {
            await command.ExecuteAsync(update, botClient, ct);
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                "Неизвестная команда. Используйте /help", cancellationToken: ct);
        }
    }
}