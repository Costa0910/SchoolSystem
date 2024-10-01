using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addmanytomany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Admins_CreateById",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Admins_CreateById",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Courses_CourseId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CreateById",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreateById",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "CreateById",
                table: "Courses",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CreateById",
                table: "Courses",
                newName: "IX_Courses_CreatedById");

            migrationBuilder.CreateTable(
                name: "CourseSubject",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubject", x => new { x.CoursesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_CourseSubject_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubject_SubjectsId",
                table: "CourseSubject",
                column: "SubjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Admins_CreatedById",
                table: "Courses",
                column: "CreatedById",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Admins_CreatedById",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseSubject");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Courses",
                newName: "CreateById");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CreatedById",
                table: "Courses",
                newName: "IX_Courses_CreateById");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreateById",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CreateById",
                table: "Subjects",
                column: "CreateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Admins_CreateById",
                table: "Courses",
                column: "CreateById",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Admins_CreateById",
                table: "Subjects",
                column: "CreateById",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Courses_CourseId",
                table: "Subjects",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
