using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RocketProfiler.Controller;

namespace RocketProfiler.Controller.Migrations
{
    [DbContext(typeof(RocketProfilerSqliteContext))]
    partial class RocketProfilerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20230");

            modelBuilder.Entity("RocketProfiler.Controller.Run", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.ToTable("Runs");
                });

            modelBuilder.Entity("RocketProfiler.Controller.SensorInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<double>("MaxValue");

                    b.Property<string>("Name");

                    b.Property<string>("Units");

                    b.HasKey("Id");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("RocketProfiler.Controller.SensorValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SensorId");

                    b.Property<int>("SnapshotId");

                    b.Property<DateTime>("Timestamp");

                    b.Property<double?>("Value");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.HasIndex("SnapshotId");

                    b.ToTable("SensorValue");
                });

            modelBuilder.Entity("RocketProfiler.Controller.Snapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RunId");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("RunId");

                    b.ToTable("Snapshot");
                });

            modelBuilder.Entity("RocketProfiler.Controller.SensorValue", b =>
                {
                    b.HasOne("RocketProfiler.Controller.SensorInfo")
                        .WithMany()
                        .HasForeignKey("SensorId");

                    b.HasOne("RocketProfiler.Controller.Snapshot")
                        .WithMany()
                        .HasForeignKey("SnapshotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RocketProfiler.Controller.Snapshot", b =>
                {
                    b.HasOne("RocketProfiler.Controller.Run")
                        .WithMany()
                        .HasForeignKey("RunId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
