namespace Proton.Dto;

public sealed class MachineDto
{
    public string Id { get; set; }
    public bool Enabled { get; set; }
    public Statuses Status { get; set; } = Statuses.None;
    public string Reason { get; set; }
    // public override string ToString() => this.GetLog();

    public enum Statuses { None, Completed, Failed };
}
