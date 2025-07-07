using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AethersJournal.Migrations
{
    /// <inheritdoc />
    public partial class updateJournalRelationshipToConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "JournalEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "Conversations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_ConversationId",
                table: "JournalEntries",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Conversations_ConversationId",
                table: "JournalEntries",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Conversations_ConversationId",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_ConversationId",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "Conversations");
        }
    }
}
