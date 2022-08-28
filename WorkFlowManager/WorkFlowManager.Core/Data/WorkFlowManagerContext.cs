using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Core.Data;

public class WorkFlowManagerContext : DbContext
{
    public DbSet<WorkFlow> WorkFlows { get; set; } = null!;
    public DbSet<Step> Steps { get; set; } = null!;
    public DbSet<Flow> Flows { get; set; } = null!;
    public DbSet<Entity> Entities { get; set; } = null!;
    public DbSet<EntityLog> EntityLogs { get; set; } = null!;
    public DbSet<UserRoleCartable> UserRoleCartables { get; set; } = null!;
    public DbSet<ServiceCartable> ServiceCartables { get; set; } = null!;

    public WorkFlowManagerContext()
    {
    }

    public WorkFlowManagerContext(DbContextOptions options) : base(options)
    {
    }
}