using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AethersJournal.Migrations
{
    /// <inheritdoc />
    public partial class updateJournalContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_ConversationId",
                table: "JournalEntries");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_ConversationId",
                table: "JournalEntries",
                column: "ConversationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_ConversationId",
                table: "JournalEntries");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_ConversationId",
                table: "JournalEntries",
                column: "ConversationId");
        }
    }
}
