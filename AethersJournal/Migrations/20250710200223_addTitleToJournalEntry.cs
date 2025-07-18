﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AethersJournal.Migrations
{
    /// <inheritdoc />
    public partial class addTitleToJournalEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "JournalEntries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "JournalEntries");
        }
    }
}
