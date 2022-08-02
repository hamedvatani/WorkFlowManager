using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Service.Core;
using WorkFlowManager.Service.Data;
using WorkFlowManager.Service.Models;
using WorkFlowManager.Service.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WorkFlowManagerContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IWorkFlowRepository, TestRepository>();
builder.Services.AddScoped<IWorkFlowManagerCore, WorkFlowManagerCore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
