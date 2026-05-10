using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicAction.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteerOrganizationsAndHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VolunteerOrganization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StudentID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerOrganization_Account_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerHour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hours = table.Column<double>(type: "REAL", nullable: false),
                    WorkDescription = table.Column<string>(type: "TEXT", nullable: false),
                    VolunteerOrganizationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerHour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerHour_VolunteerOrganization_VolunteerOrganizationID",
                        column: x => x.VolunteerOrganizationID,
                        principalTable: "VolunteerOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerHour_VolunteerOrganizationID",
                table: "VolunteerHour",
                column: "VolunteerOrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerOrganization_StudentID",
                table: "VolunteerOrganization",
                column: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VolunteerHour");

            migrationBuilder.DropTable(
                name: "VolunteerOrganization");
        }
    }
}
