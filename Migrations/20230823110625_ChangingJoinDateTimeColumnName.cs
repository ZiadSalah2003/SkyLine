using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace skyline.Migrations
{
    /// <inheritdoc />
    public partial class ChangingJoinDateTimeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "Employees",
                newName: "JoinDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinDateTime",
                table: "Employees",
                newName: "JoinDate");
        }
    }
}
