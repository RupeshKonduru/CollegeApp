using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "Email", "StdName" },
                values: new object[,]
                {
                    { 1, "Anantapur", new DateTime(1993, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "ksudharshan1010@gmail.com", "Sudharshan K" },
                    { 2, "USA", new DateTime(1991, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "gireesh@gmail.com", "Gireesh" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
