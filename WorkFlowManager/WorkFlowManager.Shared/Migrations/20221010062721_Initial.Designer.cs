﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkFlowManager.Shared.Data;

#nullable disable

namespace WorkFlowManager.Shared.Migrations
{
    [DbContext(typeof(WorkFlowManagerContext))]
    [Migration("20221010062721_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WorkFlowManager.Shared.Models.CartableItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("PossibleActions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StepId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.HasIndex("StepId");

                    b.ToTable("CartableItems");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.Entity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StarterRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StarterUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.EntityLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("LogType")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StepId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.HasIndex("StepId");

                    b.ToTable("EntityLogs");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.Flow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DestinationStepId")
                        .HasColumnType("int");

                    b.Property<int>("SourceStepId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DestinationStepId");

                    b.HasIndex("SourceStepId");

                    b.ToTable("Flows");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.Step", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AddOnWorkerClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddOnWorkerDllFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProcessType")
                        .HasColumnType("int");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StepType")
                        .HasColumnType("int");

                    b.Property<int>("WorkFlowId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkFlowId");

                    b.ToTable("Steps");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.WorkFlow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WorkFlows");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.CartableItem", b =>
                {
                    b.HasOne("WorkFlowManager.Shared.Models.Entity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkFlowManager.Shared.Models.Step", "Step")
                        .WithMany()
                        .HasForeignKey("StepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");

                    b.Navigation("Step");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.EntityLog", b =>
                {
                    b.HasOne("WorkFlowManager.Shared.Models.Entity", "Entity")
                        .WithMany("EntityLogs")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkFlowManager.Shared.Models.Step", "Step")
                        .WithMany()
                        .HasForeignKey("StepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");

                    b.Navigation("Step");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.Flow", b =>
                {
                    b.HasOne("WorkFlowManager.Shared.Models.Step", "DestinationStep")
                        .WithMany("Tails")
                        .HasForeignKey("DestinationStepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkFlowManager.Shared.Models.Step", "SourceStep")
                        .WithMany("Heads")
                        .HasForeignKey("SourceStepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DestinationStep");

                    b.Navigation("SourceStep");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.Step", b =>
                {
                    b.HasOne("WorkFlowManager.Shared.Models.WorkFlow", "WorkFlow")
                        .WithMany("Steps")
                        .HasForeignKey("WorkFlowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkFlow");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.Entity", b =>
                {
                    b.Navigation("EntityLogs");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.Step", b =>
                {
                    b.Navigation("Heads");

                    b.Navigation("Tails");
                });

            modelBuilder.Entity("WorkFlowManager.Shared.Models.WorkFlow", b =>
                {
                    b.Navigation("Steps");
                });
#pragma warning restore 612, 618
        }
    }
}
