namespace Proton.Dto;

public sealed class DtoParser
{
    private static readonly Lazy<DtoParser> _instance = new Lazy<DtoParser>(() => new DtoParser());
    
    //TODO: make a hashmap from topic to type of DTO (I can't find the data yet to make this myself)
    private static Dictionary<string, object> topics = new Dictionary<string, object>();

    private DtoParser()
    {
    }

    public static DtoParser Instance => _instance.Value;

    public object TypeOf(string topic)
    {
        return topics[topic];
    }
}