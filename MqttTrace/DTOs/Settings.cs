/*
using Microsoft.Extensions.Configuration;
using Serilog;
namespace MqttInvoker;
using Proton.Core;
using Proton.JobManagement.Common;
using Proton.Model;

#region Context

public sealed class DataContext : ProtonContext
{
    protected override string GetConnectionStringCore() => Settings.Create(Array.Empty<string>()).DbConnection;
}

#endregion

#region Settings

interface ITippingSettings { IEnumerable<string> GetTippers(); }
interface IBoxManagementSettings { IEnumerable<string> GetIntakers(); }

sealed class Settings : IMqttSettings, ILogSettings, IDbSettings, ITippingSettings, IBoxManagementSettings, IUnitSettings
{
    public static Settings Instance;
    public static Settings Create(string[] args) => Instance ??= new Settings(args);

    private readonly IConfigurationRoot Configuration;
    private Settings(string[] args) => Configuration = new ConfigurationBuilder()
                                                          .AddJsonFile("appsettings.json")
                                                          .AddCommandLine(args)
                                                          .Build();
    #region ICommunicationSettings

    public CommunicationType CommunicationType => Configuration[$"Communication:type"].ToEnum<CommunicationType>();
    public string Url => Configuration["Communication:url"];
    public int Port => Configuration.GetValue<int>("Communication:port");
    public bool UseAppServer => throw new NotSupportedException();
    public bool UseTls => Configuration.GetValue<bool>("Communication:useTls");
    public int ReconnectDelay => Configuration.GetValue<int>("Communication:reconnectDelay");
    public int KeepAlive => Configuration.GetValue<int>("Communication:keepAlive");
    public string Username => Configuration.GetValue<string>("Communication:username");
    public string Password => Configuration.GetValue<string>("Communication:password");
    public IEnumerable<(string id, string url)> GetTopics()
    {
        var section = Configuration.GetSection($"Communication:topics");
        foreach (var item in section.GetChildren())
            yield return (item.GetValue<string>("id"), item.GetValue<string>("url"));
    }
    public bool AutoGenerateId => Configuration.GetValue<bool>("Services:generateId");
    public bool SendAfterComplete => Configuration.GetValue<bool>("Services:sendAfterComplete");
    public int ScheduleLookAheadTime => Configuration.GetValue<int>("Services:proportioning:scheduleLookAheadTime");

    #endregion

    public (IEnumerable<Topic> SendTo, IEnumerable<Topic> Subscriptions) GetTopicInfo(ServiceType type)
    {
        var section = Configuration.GetSection($"Services:{type}");
        if (!section.Exists()) return default;
        var sendTo = GetTopics(section, "topics:sendTo");
        var subscriptions = GetTopics(section, "topics:subscribe");
        return (sendTo, subscriptions);
    }
    private static IEnumerable<Topic> GetTopics(IConfigurationSection section, string source) =>
        section.GetSection(source).GetChildren().Select(cs => Topic.GetTopic(cs.Value));

    public string[] GetStorage() => Configuration.GetSection("Services:storage").GetChildren().Select(i => i.Value).ToArray();

    #region IDbSettings

    public string DbConnection => Configuration["DbSettings:connection"];
    public string MySqlDbConnection => throw new NotSupportedException();
    public string PgSqlDbConnection => throw new NotSupportedException();
    public InitDbOptions InitDbOption => throw new NotSupportedException();

    #endregion

    #region ILogSettings

    public string FolderLogPath => Configuration["LogSettings:folderPath"];
    public string ApplicationLogFile => Path.Combine(FolderLogPath, Configuration["LogSettings:application"]);
    public string GetServiceLogFile(ServiceType service) => Path.Combine(FolderLogPath, Configuration[$"LogSettings:{service}"]);
    public RollingInterval GetRollingInterval() => Configuration["LogSettings:rollingInterval"].ToEnum<RollingInterval>();
    public int GetFileSizeLimitBytes() => Configuration.GetValue<int>("LogSettings:fileSizeLimitBytes");
    public bool GetRollOnFileSizeLimit() => Configuration.GetValue<bool>("LogSettings:rollOnFileSizeLimit");
    public TimeSpan GetRetainedFileTimeLimit() => TimeSpan.FromHours(Configuration.GetValue<int>("LogSettings:retainedFileTimeLimit"));

    #endregion

    #region Tipping

    public IEnumerable<string> GetTippers() => Configuration.GetSection("Services:tipping:tippers")
                                              .GetChildren().Select(cs => cs.Value);

    #endregion

    #region BoxManagement

    public IEnumerable<string> GetIntakers() => Configuration.GetSection("Services:boxManagement:intakers")
                                               .GetChildren().Select(cs => cs.Value);

    #endregion

    #region IUnitSettings

    string IUnitSettings.WeightUnit => Configuration["SystemSettings:weightUnit"];
    string IUnitSettings.LiquidUnit => Configuration["SystemSettings:liquidUnit"];
    string IUnitSettings.PercentUnit => Configuration["SystemSettings:percentUnit"];

    #endregion
}

#endregion

#region Extensions

static class Extensions
{
    public static bool IsConfigurableFromFile(this ServiceType serviceType) => serviceType != ServiceType.BoxManagement;
    public static bool HasSubPath(this ServiceType serviceType) => serviceType == ServiceType.StockManagement || 
                                                                   serviceType == ServiceType.Proportioning ||
                                                                   serviceType == ServiceType.Tipping;
}

#endregion
*/
