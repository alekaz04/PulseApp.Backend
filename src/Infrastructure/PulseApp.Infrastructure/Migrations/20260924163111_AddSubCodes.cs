using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserOwnerId",
                table: "SubscriptionPush",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Compliment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SubscriptionCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpireAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedCodeUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionCode_User_CreatedCodeUserId",
                        column: x => x.CreatedCodeUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPush_UserOwnerId",
                table: "SubscriptionPush",
                column: "UserOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Compliment_CreatedByUserId",
                table: "Compliment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionCode_Code",
                table: "SubscriptionCode",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionCode_CreatedCodeUserId",
                table: "SubscriptionCode",
                column: "CreatedCodeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compliment_User_CreatedByUserId",
                table: "Compliment",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionPush_User_UserOwnerId",
                table: "SubscriptionPush",
                column: "UserOwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compliment_User_CreatedByUserId",
                table: "Compliment");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionPush_User_UserOwnerId",
                table: "SubscriptionPush");

            migrationBuilder.DropTable(
                name: "SubscriptionCode");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionPush_UserOwnerId",
                table: "SubscriptionPush");

            migrationBuilder.DropIndex(
                name: "IX_Compliment_CreatedByUserId",
                table: "Compliment");

            migrationBuilder.DropColumn(
                name: "UserOwnerId",
                table: "SubscriptionPush");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Compliment");
        }
    }
}
