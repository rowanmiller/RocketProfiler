using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RocketProfiler.Controller.Migrations
{
    public partial class UseSensorInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Not doing this because SQLite doesn't support it
            //migrationBuilder.AlterColumn<int>(
            //    name: "SensorId",
            //    table: "SensorValue",
            //    nullable: false);

            migrationBuilder.AddColumn<double>(
                name: "MaxValue",
                table: "Sensors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Units",
                table: "Sensors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "Sensors");

            migrationBuilder.AlterColumn<int>(
                name: "SensorId",
                table: "SensorValue",
                nullable: true);
        }
    }
}
