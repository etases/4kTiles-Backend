using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4kTiles_Backend.Migrations
{
    public partial class CreatorSong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "creatorid",
                table: "song",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateddate",
                table: "song",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_song_creatorid",
                table: "song",
                column: "creatorid");

            migrationBuilder.AddForeignKey(
                name: "FK_song_account_creatorid",
                table: "song",
                column: "creatorid",
                principalTable: "account",
                principalColumn: "accountid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_song_account_creatorid",
                table: "song");

            migrationBuilder.DropIndex(
                name: "IX_song_creatorid",
                table: "song");

            migrationBuilder.DropColumn(
                name: "creatorid",
                table: "song");

            migrationBuilder.DropColumn(
                name: "updateddate",
                table: "song");
        }
    }
}
