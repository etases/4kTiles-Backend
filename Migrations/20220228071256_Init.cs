using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _4kTiles_Backend.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    accountid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    hashedpassword = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletedreason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "NULL::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.accountid);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    genreid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    genrename = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.genreid);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    roleid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rolename = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.roleid);
                });

            migrationBuilder.CreateTable(
                name: "song",
                columns: table => new
                {
                    songid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    songname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    author = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    bpm = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "100"),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    releasedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ispublic = table.Column<bool>(type: "boolean", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletedreason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "NULL::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_song", x => x.songid);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    tagid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tagname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ispublishertag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.tagid);
                });

            migrationBuilder.CreateTable(
                name: "accountrole",
                columns: table => new
                {
                    arid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accountid = table.Column<int>(type: "integer", nullable: false),
                    roleid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("accountrole_pkey", x => x.arid);
                    table.ForeignKey(
                        name: "accountrole_accountid_fkey",
                        column: x => x.accountid,
                        principalTable: "account",
                        principalColumn: "accountid");
                    table.ForeignKey(
                        name: "accountrole_roleid_fkey",
                        column: x => x.roleid,
                        principalTable: "role",
                        principalColumn: "roleid");
                });

            migrationBuilder.CreateTable(
                name: "accountsong",
                columns: table => new
                {
                    asid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accountid = table.Column<int>(type: "integer", nullable: false),
                    songid = table.Column<int>(type: "integer", nullable: false),
                    bestscore = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("accountsong_pkey", x => x.asid);
                    table.ForeignKey(
                        name: "accountsong_accountid_fkey",
                        column: x => x.accountid,
                        principalTable: "account",
                        principalColumn: "accountid");
                    table.ForeignKey(
                        name: "accountsong_songid_fkey",
                        column: x => x.songid,
                        principalTable: "song",
                        principalColumn: "songid");
                });

            migrationBuilder.CreateTable(
                name: "songgenre",
                columns: table => new
                {
                    sgid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    songid = table.Column<int>(type: "integer", nullable: false),
                    genreid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("songgenre_pkey", x => x.sgid);
                    table.ForeignKey(
                        name: "songgenre_genreid_fkey",
                        column: x => x.genreid,
                        principalTable: "genre",
                        principalColumn: "genreid");
                    table.ForeignKey(
                        name: "songgenre_songid_fkey",
                        column: x => x.songid,
                        principalTable: "song",
                        principalColumn: "songid");
                });

            migrationBuilder.CreateTable(
                name: "songreport",
                columns: table => new
                {
                    reportid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    songid = table.Column<int>(type: "integer", nullable: false),
                    accountid = table.Column<int>(type: "integer", nullable: false),
                    reporttitle = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    reportreason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    reportdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    reportstatus = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("songreport_pkey", x => x.reportid);
                    table.ForeignKey(
                        name: "songreport_accountid_fkey",
                        column: x => x.accountid,
                        principalTable: "account",
                        principalColumn: "accountid");
                    table.ForeignKey(
                        name: "songreport_songid_fkey",
                        column: x => x.songid,
                        principalTable: "song",
                        principalColumn: "songid");
                });

            migrationBuilder.CreateTable(
                name: "songtag",
                columns: table => new
                {
                    stid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    songid = table.Column<int>(type: "integer", nullable: false),
                    tagid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("songtag_pkey", x => x.stid);
                    table.ForeignKey(
                        name: "songtag_songid_fkey",
                        column: x => x.songid,
                        principalTable: "song",
                        principalColumn: "songid");
                    table.ForeignKey(
                        name: "songtag_tagid_fkey",
                        column: x => x.tagid,
                        principalTable: "tag",
                        principalColumn: "tagid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_accountrole_accountid",
                table: "accountrole",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_accountrole_roleid",
                table: "accountrole",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_accountsong_accountid",
                table: "accountsong",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_accountsong_songid",
                table: "accountsong",
                column: "songid");

            migrationBuilder.CreateIndex(
                name: "IX_songgenre_genreid",
                table: "songgenre",
                column: "genreid");

            migrationBuilder.CreateIndex(
                name: "IX_songgenre_songid",
                table: "songgenre",
                column: "songid");

            migrationBuilder.CreateIndex(
                name: "IX_songreport_accountid",
                table: "songreport",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_songreport_songid",
                table: "songreport",
                column: "songid");

            migrationBuilder.CreateIndex(
                name: "IX_songtag_songid",
                table: "songtag",
                column: "songid");

            migrationBuilder.CreateIndex(
                name: "IX_songtag_tagid",
                table: "songtag",
                column: "tagid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountrole");

            migrationBuilder.DropTable(
                name: "accountsong");

            migrationBuilder.DropTable(
                name: "songgenre");

            migrationBuilder.DropTable(
                name: "songreport");

            migrationBuilder.DropTable(
                name: "songtag");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "song");

            migrationBuilder.DropTable(
                name: "tag");
        }
    }
}
