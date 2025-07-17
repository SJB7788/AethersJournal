using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AethersJournal.Migrations
{
    /// <inheritdoc />
    public partial class journalEntryConversationIdDeleteSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Conversations_ConversationId",
                table: "JournalEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Conversations_ConversationId",
                table: "JournalEntries",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Conversations_ConversationId",
                table: "JournalEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Conversations_ConversationId",
                table: "JournalEntries",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id");
        }
    }
}
