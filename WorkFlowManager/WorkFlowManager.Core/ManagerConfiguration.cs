namespace WorkFlowManager.Core;

public class ManagerConfiguration
{
    public string RabbitMqHostName { get; set; } = "127.0.0.1";
    public string RabbitMqUserName { get; set; } = "guest";
    public string RabbitMqPassword { get; set; } = "guest";
    public string QueueName { get; set; } = "WorkFlowManager";
    public int Timeout { get; set; } = 30000;
    public bool UseSqlServerDb { get; set; } = true;
    public bool UseSqliteDb { get; set; }

    public string ConnectionString { get; set; } = "Server=.;Database=WFM_Db;Trusted_Connection=True;";

    public void UseSqlServer(string connectionString)
    {
        UseSqlServerDb = true;
        ConnectionString = connectionString;
    }

    public void UseSqlite(string filename)
    {
        UseSqliteDb = true;
        ConnectionString = $"Filename={filename}";
    }
}