using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Data;

public class WorkFlowManagerContext : DbContext
{
    public DbSet<WorkFlow> WorkFlows { get; set; } = null!;
    public DbSet<Step> Steps { get; set; } = null!;
    public DbSet<Flow> Flows { get; set; } = null!;
    public DbSet<AddOnWorker> AddOnWorkers { get; set; } = null!;
    public DbSet<Entity> Entities { get; set; } = null!;
    public DbSet<EntityFlow> EntityFlows { get; set; } = null!;

    public WorkFlowManagerContext(DbContextOptions options) : base(options)
    {
    }
}