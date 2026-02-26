public class GroupSchedule
{
    public string Group { get; set; } = string.Empty;
    public List<DaySchedule> Days { get; set; } = new();
}