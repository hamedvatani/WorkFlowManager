using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Service.Models;

namespace WorkFlowManager.Service.Data;

public class WorkFlowManagerContext : DbContext
{
    public DbSet<WorkFlow> WorkFlows { get; set; } = null!;
    public DbSet<Step> Steps { get; set; } = null!;
    public DbSet<Flow> Flows { get; set; } = null!;
    public DbSet<AddOneWorker> AddOneWorkers { get; set; } = null!;
    public DbSet<Entity> Entities { get; set; } = null!;

    public WorkFlowManagerContext(DbContextOptions options) : base(options)
    {
    }
}