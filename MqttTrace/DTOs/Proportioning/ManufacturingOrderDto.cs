using Proton.Dto;

namespace MqttTrace.DTOs.Proportioning;

//[TotalAmount, ScheduledReadyTime, IngredientAvailable]
public sealed class ManufacturingOrderDto : ParameterizedDto
{
    //request
    //[Required, ProtonStringLength(50)]
    public string ManufacturingOrderId { get; set; }
    //[Required, ProtonRange(0, int.MaxValue)]
    public int NumberOfBatches { get; set; }
    //[Required, ProtonRange(0, double.MaxValue)]
    public double TotalRequestedAmount { get; set; }
    //[Required, WeightUnit]
    public string TotalRequestedAmountUnit { get; set; }
    //[Required]
    public DateTime ScheduledReadyTime { get; set; }
    //[ProtonStringLength(50)]
    public string ProductId { get; set; }
    //[ProtonStringLength(50)]
    public string ArticleId { get; set; }
    //[ProtonStringLength(50)]
    public string Destination { get; set; }
    public IEnumerable<IngredientDto> Ingredients { get; set; }
    
    //response
    public string Status { get; set; }
    public string Reason { get; set; }

    //public override string ToString() => this.GetLog();
}
