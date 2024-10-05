using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeteacherproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Staffs_TeacherId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Grades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Grades",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Staffs_TeacherId",
                table: "Grades",
                column: "TeacherId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
