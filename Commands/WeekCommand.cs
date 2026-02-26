using Telegram.Bot;
using Telegram.Bot.Types;
using ScheduleBot.Commands;

public class WeekCommand : ICommand
{
    protected readonly IScheduleRepository _scheduleRepository;

    public WeekCommand(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }


    public async Task ExecuteAsync(Update update, ITelegramBotClient botClient, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        var text = update.Message!.Text ?? string.Empty;

        var parts = text.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                "Использование: /week [группа]\nНапример: /week 10",
                cancellationToken: ct);
            return;
        }

        var groupName = parts[1].Trim();

        var schedule = _scheduleRepository.Load();
        var group = schedule.Groups
            .FirstOrDefault(g => string.Equals(g.Group, groupName, StringComparison.OrdinalIgnoreCase));

        if (group == null)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                $"Для группы {groupName} нет расписания либо этой группы не существует",
                cancellationToken: ct);
            return;
        }

        var lines = new List<string> { $"Полное расписание для {groupName}:" };

        foreach (var day in group.Days)
        {
            lines.Add($"\n{day.Day}:");
            if (day.Lessons == null || day.Lessons.Count == 0)
            {
                lines.Add("  нет уроков");
            }
            else
            {
                lines.AddRange(
                    day.Lessons.Select(
                        (l, i) => $"  {i + 1}. {l.Time} -- {l.Subject} {(string.IsNullOrEmpty(l.Teacher) ? "" : "(" + l.Teacher + ")")}"
                    )
                );
            }
        }

        await botClient.SendTextMessageAsync(chatId, string.Join('\n', lines), cancellationToken: ct);
    }
}

