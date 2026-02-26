using ScheduleBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

class Program
{
    private const string ScheduleJson = "schedule.json";

    public static async Task Main()
    {
        Console.WriteLine("Запуск бота...");

        var token = "8634974741:AAGXXJ0PNfpqfRWqcCPspkAIwB3cIuVSVNk";
        var botClient = new TelegramBotClient(token);

        var scheduleRepository = new JsonScheduleRepository(ScheduleJson);

        var dispatcher = new CommandDispatcher();
        dispatcher.Register("/start", new StartCommand());
        dispatcher.Register("/help", new HelpCommand());
        dispatcher.Register("/week", new WeekCommand(scheduleRepository));

        using var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() };

        botClient.StartReceiving(
            async (client, update, ct) => await dispatcher.DispatchAsync(update, client, ct),
            HandleErrorAsync,
            receiverOptions,
            cts.Token);

        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Бот запущен: @{me.Username}");
        Console.ReadLine();
        cts.Cancel();
    }

    static Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken ct)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
        return Task.CompletedTask;
    }
}

