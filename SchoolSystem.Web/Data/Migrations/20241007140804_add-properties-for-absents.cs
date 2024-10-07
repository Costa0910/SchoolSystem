using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addpropertiesforabsents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Staffs_TeacherId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_TeacherId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Attendances");

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ExclusionPercentage",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ExclusionPercentage",
                table: "Courses");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Attendances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_TeacherId",
                table: "Attendances",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Staffs_TeacherId",
                table: "Attendances",
                column: "TeacherId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
