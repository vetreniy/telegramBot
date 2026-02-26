public interface IScheduleRepository
{
    ScheduleFile Load();
    void Save(ScheduleFile schedule);
}