namespace Proton.Dto;

public sealed class AlarmDto
{
    //was AlarmLevel
    public int Level { get; set; }
    public string Text { get; set; }
    public string Tag { get; set; }

    public string MachineId { get; set; }
    public string UniqueId { get; set; }
}
