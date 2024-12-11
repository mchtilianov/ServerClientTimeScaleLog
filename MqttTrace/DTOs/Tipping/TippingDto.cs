namespace MqttTrace.DTOs.Tipping;

public sealed class TippingDto // : IProductLotIdProvider
{
    // [Required, ProtonStringLength(50)]
    public string ManufacturingOrderId { get; set; }
    // [Required, ProtonRange(0, int.MaxValue)]
    public int BatchId { get; set; }
    // [ProtonStringLength(50)]
    public string LineId { get; set; }
    public string[] Boxes { get; set; }
    public string Status { get; set; }
    public string Reason { get; set; }

    // public string ProductLotId => AppService.FormatLotId(ManufacturingOrderId, BatchId);
    // public override string ToString() => this.GetLog();
}