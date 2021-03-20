﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreExample.Migrations
{
    public partial class orders_change_date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangeDate",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChangeDate",
                table: "Orders");
        }
    }
}
