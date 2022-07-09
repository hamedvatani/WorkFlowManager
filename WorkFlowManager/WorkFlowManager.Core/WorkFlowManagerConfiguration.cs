namespace WorkFlowManager.Core;

public class WorkFlowManagerConfiguration
{
    public bool UsingSqlServer { get; set; }
    public bool UsingSqlite { get; set; }
    public string SqlServerConnectionString { get; set; }
    public string SqliteFilename { get; set; }

    public void UseSqlServer(string connectionString)
    {
        SqlServerConnectionString = connectionString;
        UsingSqlServer = true;
    }

    public void UseSqlite(string filename)
    {
        SqliteFilename = filename;
        UsingSqlite = true;
    }
}