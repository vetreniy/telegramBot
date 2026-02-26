using Telegram.Bot;
using Telegram.Bot.Types;
using ScheduleBot.Commands;

public class TeacherCommand : ICommand
{
    private readonly IScheduleRepository _scheduleRepository;

    public TeacherCommand(IScheduleRepository scheduleRepository)
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
                "Использование: /teacher [название предмета]\n" +
                "Например: /teacher математика",
                cancellationToken: ct);
            return;
        }

        var subjectName = parts[1].Trim().ToLower(); 

        var schedule = _scheduleRepository.Load();

        var teacherInfo = new Dictionary<string, HashSet<string>>(); 

        foreach (var group in schedule.Groups ?? new List<GroupSchedule>())
        {
            foreach (var day in group.Days ?? new List<DaySchedule>())
            {
                foreach (var lesson in day.Lessons ?? new List<Lesson>())
                {
                    var lessonSubject = lesson.Subject?.ToLower() ?? "";

                    if (lessonSubject.Contains(subjectName) || subjectName.Contains(lessonSubject))
                    {
                        var key = lesson.Subject;
                        if (!teacherInfo.ContainsKey(key))
                        {
                            teacherInfo[key] = new HashSet<string>();
                        }

                        if (!string.IsNullOrEmpty(lesson.Teacher))
                        {
                            teacherInfo[key].Add(lesson.Teacher);
                        }
                    }
                }
            }
        }

        if (teacherInfo.Count == 0)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                $"Предмет \"{parts[1]}\" не найден в расписании.",
                cancellationToken: ct);
            return;
        }

        var response = new List<string> { $"По запросу \"{parts[1]}\" найдено:\n" };

        foreach (var item in teacherInfo)
        {
            var teachers = string.Join(", ", item.Value);
            response.Add($"{item.Key}:");
            response.Add($"   {teachers}");
            response.Add("");
        }

        await botClient.SendTextMessageAsync(
            chatId,
            string.Join("\n", response),
            cancellationToken: ct);
    }
}