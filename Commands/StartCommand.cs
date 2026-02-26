using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScheduleBot.Commands;

public class StartCommand : ICommand
{
    public async Task ExecuteAsync(Update update, ITelegramBotClient botClient, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        string text = "Привет! Я бот расписания\n" +
                      "Используй /help, чтобы увидеть список команд.";

        await botClient.SendTextMessageAsync(chatId, text, cancellationToken: ct);
    }
}