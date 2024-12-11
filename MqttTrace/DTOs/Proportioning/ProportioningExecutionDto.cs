using Proton.Dto;

namespace MqttTrace.DTOs.Proportioning;
public class ProportioningExecutionDto : ParameterizedDto
{
    //[Required, ProtonStringLength(50)]
    public string ManufacturingOrderId { get; set; }
    //[Required, ProtonRange(0, int.MaxValue)]
    public int BatchId { get; set; }
    //[Required, ProtonStringLength(20)]
    public string ProportioningLocation { get; set; }
    public IEnumerable<ProportioningDto> Proportionings { get; set; }
}
