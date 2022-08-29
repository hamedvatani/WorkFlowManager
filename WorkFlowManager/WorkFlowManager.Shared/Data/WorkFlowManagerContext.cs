using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Shared.Models;

namespace WorkFlowManager.Shared.Data;

public class WorkFlowManagerContext : DbContext
{
    public DbSet<WorkFlow> WorkFlows { get; set; } = null!;
    public DbSet<Step> Steps { get; set; } = null!;
    public DbSet<Flow> Flows { get; set; } = null!;
    public DbSet<Entity> Entities { get; set; } = null!;
    public DbSet<EntityLog> EntityLogs { get; set; } = null!;
    public DbSet<UserRoleCartable> UserRoleCartables { get; set; } = null!;
    public DbSet<ServiceCartable> ServiceCartables { get; set; } = null!;

    protected WorkFlowManagerContext()
    {
    }

    public WorkFlowManagerContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Server=.;Database=WFM_Db;Trusted_Connection=True;");
    }
}