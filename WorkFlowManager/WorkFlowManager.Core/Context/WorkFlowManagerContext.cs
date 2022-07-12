using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Context;

public class WorkFlowManagerContext:DbContext
{
    public DbSet<WorkFlow> WorkFlows { get; set; } = null!;
    public DbSet<Step> Steps { get; set; }

    public WorkFlowManagerContext(DbContextOptions options) : base(options)
    {
    }
}