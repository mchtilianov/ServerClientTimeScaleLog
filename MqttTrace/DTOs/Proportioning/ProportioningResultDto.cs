using Proton.Dto;

namespace MqttTrace.DTOs.Proportioning;

public sealed class ProportioningResultDto : ParameterizedDto
{
    public string ManufacturingOrderId { get; set; }
    public int BatchId { get; set; }
    public double Amount { get; set; }
    public string AmountUnit { get; set; }
    public bool HasErroneousProportionings { get; set; }
    public IEnumerable<ProportioningDto> Proportionings { get; set; }
}
public sealed class ProportioningDto : ParameterizedDto
{
    //[Required, ProtonStringLength(50)]
    public string ArticleId { get; set; }
    //[Required, ProtonStringLength(50)]
    public string ArticleName { get; set; }
    //[Required, ProtonStringLength(50)]
    public string IngredientLotId { get; set; }
    //[ProtonStringLength(50)]
    public string MixLotId { get; set; }
    //[Required, ProtonRange(0, double.MaxValue)]
    public double RequestedAmount { get; set; }
    //[ProtonStringLength(20)]
    public string RequestedAmountUnit { get; set; }
    public double ActualAmount { get; set; }
    public string ActualAmountUnit { get; set; }
    //[Required, ProtonStringLength(50)]
    public string IngredientBoxId { get; set; }
    //[Required, ProtonStringLength(50)]
    public string MixBoxId { get; set; }
    //[Required, ProtonRange(0, double.MaxValue)]
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
   // public bool OutsideProportioningTolerance { get; set; }
    public bool OutsideRejectionTolerance { get; set; }
}
