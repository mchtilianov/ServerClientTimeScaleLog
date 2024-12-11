using Proton.Dto;

namespace MqttTrace.DTOs.Proportioning;


//[Loggable, PropLocId]
public sealed class IngredientDto : ParameterizedDto
{
    //[Required, ProtonStringLength(50)]
    public string ArticleId { get; set; }
    //[Required, ProtonStringLength(50)]
    public string Name { get; set; }
    //[Required, ProtonRange(0, double.MaxValue)]
    public double RequestedAmount { get; set; }
    //[Required, WeightUnit]
    public string RequestedAmountUnit { get; set; }
    //[ProtonStringLength(50)]
    public string ProportioningClusterSequence { get; set; }
    //[ProtonStringLength(20)]
    public string PropLocGroupId { get; set; }
    public double? UpperProportioningTolerance { get; set; }
    //[RequiredIfNotEmpty(nameof(UpperProportioningTolerance)), PercentUnit]
    public string UpperProportioningToleranceUnit { get; set; }
    public double? LowerProportioningTolerance { get; set; }
    //[RequiredIfNotEmpty(nameof(LowerProportioningTolerance)), PercentUnit]
    public string LowerProportioningToleranceUnit { get; set; }
    public double? UpperRejectionTolerance { get; set; }
    //[RequiredIfNotEmpty(nameof(UpperRejectionTolerance)), PercentUnit]
    public string UpperRejectionToleranceUnit { get; set; }
    public double? LowerRejectionTolerance { get; set; }
    //[RequiredIfNotEmpty(nameof(LowerRejectionTolerance)), PercentUnit]
    public string LowerRejectionToleranceUnit { get; set; }
    public double? Accuracy { get; set; }
    //[RequiredIfNotEmpty(nameof(Accuracy)), PercentUnit]
    public string AccuracyUnit { get; set; }
}
