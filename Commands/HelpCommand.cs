using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScheduleBot.Commands;

public class HelpCommand : ICommand
{
    public async Task ExecuteAsync(Update update, ITelegramBotClient botClient, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        string text = "Доступные команды:\n" +
                      "/start - приветствие\n" +
                      "/help - помощь\n" +
                      "/week - расписание на неделю\n" +
                      "/teacher [предмет] - узнать, кто ведет предмет\n";

        await botClient.SendTextMessageAsync(chatId, text, cancellationToken: ct);
    }
}