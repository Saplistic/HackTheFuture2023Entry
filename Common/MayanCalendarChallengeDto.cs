namespace Common;

public record MayanCalendarChallengeDto
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Day { get; set; }
}