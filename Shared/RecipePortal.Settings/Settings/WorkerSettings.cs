namespace RecipePortal.Settings;

public class WorkerSettings : IWorkerSettings
{
    private readonly ISettingsSource sourse;
    private readonly IDbSettings dbSettings;
    private readonly IRabbitMqSettings rabbitMqSettings;
    private readonly IEmailSettings emailSettings;

    public WorkerSettings(ISettingsSource source) => this.sourse = source;

    public WorkerSettings(ISettingsSource source, IDbSettings dbSettings, IRabbitMqSettings rabbitMqSettings, IEmailSettings emailSettings)
    {
        this.sourse = source;
        this.dbSettings = dbSettings;
        this.rabbitMqSettings = rabbitMqSettings;
        this.emailSettings = emailSettings;
    }

    public IDbSettings Db => dbSettings ?? new DbSettings(sourse);
    public IRabbitMqSettings RabbitMq => rabbitMqSettings ?? new RabbitMqSettings(sourse);
    public IEmailSettings Email => emailSettings ?? new EmailSettings(sourse);
}
