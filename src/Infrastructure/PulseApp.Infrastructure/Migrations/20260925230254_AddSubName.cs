using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionPush_User_UserOwnerId",
                table: "SubscriptionPush");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionPush",
                table: "SubscriptionPush");

            migrationBuilder.RenameTable(
                name: "SubscriptionPush",
                newName: "Subscription");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionPush_UserOwnerId",
                table: "Subscription",
                newName: "IX_Subscription_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionPush_Endpoint",
                table: "Subscription",
                newName: "IX_Subscription_Endpoint");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Subscription",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SubscriptionCode",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_User_UserOwnerId",
                table: "Subscription",
                column: "UserOwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_User_UserOwnerId",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SubscriptionCode");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Subscription");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "SubscriptionPush");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_UserOwnerId",
                table: "SubscriptionPush",
                newName: "IX_SubscriptionPush_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_Endpoint",
                table: "SubscriptionPush",
                newName: "IX_SubscriptionPush_Endpoint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionPush",
                table: "SubscriptionPush",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionPush_User_UserOwnerId",
                table: "SubscriptionPush",
                column: "UserOwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
