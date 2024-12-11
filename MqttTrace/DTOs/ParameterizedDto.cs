namespace Proton.Dto;

public abstract class ParameterizedDto //: IParameterProvider
{
    string Time { get; set; }
    string Topic { get; set; }
    readonly Dictionary<string, object> Values = new();
/*
    #region IParameterProvider

    string IParameterProvider.GetVersion(ISystemSettings settings) => GetVersionCore(settings);
    object IParameterProvider.GetValue(string key)
    {
        Validate(key);
        return GetValueCore(key);
    }
    void IParameterProvider.AddValue(string key, object value)
    {
        Validate(key);
        AddValueCore(key, value);
    }

    #endregion

    protected virtual string GetVersionCore(ISystemSettings settings) => settings.Version;
    protected virtual object GetValueCore(string key) => Values.TryGetValue(key, out var value) ? value : null;
    protected virtual void AddValueCore(string key, object value)
    {
        if (!Values.TryAdd(key, value))
            Values[key] = value;
    }
    void Validate(string key)
    {
        ValidateValues();
        ValidateKey(key);
    }
    void ValidateKey(string key) { if (key.IsEmpty()) throw new ArgumentException("Key cannot be empty."); }
    void ValidateValues() { if (Values is null) throw new Exception("Values collection is not initialized."); }
    */
}
