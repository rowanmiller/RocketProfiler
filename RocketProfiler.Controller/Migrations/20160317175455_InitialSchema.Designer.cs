using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RocketProfiler.Controller;

namespace RocketProfiler.Controller.Migrations
{
    [DbContext(typeof(RocketProfilerSqliteContext))]
    [Migration("20160317175455_InitialSchema")]
    partial class InitialSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("RocketProfiler.Controller.Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sensors");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Sensor");
                });

            modelBuilder.Entity("RocketProfiler.Controller.SensorValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("SensorId");

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

            modelBuilder.Entity("RocketProfiler.Controller.RocketProfilerContextDesignTimeFactory+DesignTimeSensor", b =>
                {
                    b.HasBaseType("RocketProfiler.Controller.Sensor");


                    b.ToTable("DesignTimeSensor");

                    b.HasDiscriminator().HasValue("DesignTimeSensor");
                });

            modelBuilder.Entity("RocketProfiler.Controller.SensorValue", b =>
                {
                    b.HasOne("RocketProfiler.Controller.Sensor")
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
