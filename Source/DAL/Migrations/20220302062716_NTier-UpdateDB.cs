using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class NTierUpdateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    City = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BuiltDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Library_CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 3, 2, 6, 27, 16, 29, DateTimeKind.Utc).AddTicks(4523)),
                    Library_ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 3, 2, 6, 27, 16, 39, DateTimeKind.Utc).AddTicks(5847)),
                    Library_CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Library_ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libraries");
        }
    }
}
