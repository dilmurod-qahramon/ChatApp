using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "messages");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "chats");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatId",
                table: "messages",
                newName: "IX_messages_ChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_messages",
                table: "messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chats",
                table: "chats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "chat_users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_users", x => new { x.ChatId, x.UserId });
                    table.ForeignKey(
                        name: "FK_chat_users_chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages");

            migrationBuilder.DropTable(
                name: "chat_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_messages",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chats",
                table: "chats");

            migrationBuilder.RenameTable(
                name: "messages",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "chats",
                newName: "Chats");

            migrationBuilder.RenameIndex(
                name: "IX_messages_ChatId",
                table: "Messages",
                newName: "IX_Messages_ChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    ChatsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.ChatsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatUser_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_UsersId",
                table: "ChatUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
