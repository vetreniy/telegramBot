using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScheduleBot.Commands;

public interface ICommand
{
    Task ExecuteAsync(Update update, ITelegramBotClient botClient, CancellationToken ct);
}