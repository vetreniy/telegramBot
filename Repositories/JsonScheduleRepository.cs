using System.Text.Json;

public class JsonScheduleRepository : IScheduleRepository
{
    private readonly string _path;
    private readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

    public JsonScheduleRepository(string path)
    {
        _path = path;
        Console.WriteLine($"Путь к файлу: {Path.GetFullPath(_path)}");
        if (!File.Exists(_path))
        {
            var sample = new ScheduleFile
            {
                Groups = new List<GroupSchedule>
                {
                   new GroupSchedule
                   {
                       Group = "11",
                       Days = new List<DaySchedule>
                       {
                           new DaySchedule { Day = "Понедельник", Lessons = new List<Lesson> {
                               new Lesson("09:00","Классный час","Чертова"),
                               
                           } },
                           new DaySchedule { Day = "Вторник", Lessons = new List<Lesson> {
                               new Lesson("08:10","Физика (проф)","Толмачёва"),
                               
                           } },
                           new DaySchedule { Day = "Среда", Lessons = new List<Lesson>() {
                               new Lesson("08:10","Химия (проф)","Стёпина"),
                               new Lesson("09:00","Химия (проф)","Стёпина"),
                               
                           } },
                           new DaySchedule { Day = "Четверг", Lessons = new List<Lesson>() {
                               new Lesson("09:00","География","Мелехов"),
                               
                           } },
                           new DaySchedule { Day = "Пятница", Lessons = new List<Lesson>() {
                               new Lesson("09:00","ВиС","Чертова"),
                             
                           } }
                       }
                   }
               }
            };
            File.WriteAllText(_path, JsonSerializer.Serialize(sample, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    public ScheduleFile Load()
    {
        using var s = File.OpenRead(_path);
        return JsonSerializer.Deserialize<ScheduleFile>(s, _opts) ?? new ScheduleFile();

    }
    public void Save(ScheduleFile schedule)
    {
        try
        {
            Console.WriteLine($"Сохраняем {schedule.Groups.Count} групп в файл {_path}");

            var json = JsonSerializer.Serialize(schedule, _opts);

            File.WriteAllText(_path, json);

            Console.WriteLine("Сохраненные группы:");
            foreach (var group in schedule.Groups)
            {
                Console.WriteLine($"  - {group.Group}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
        }
    }
}