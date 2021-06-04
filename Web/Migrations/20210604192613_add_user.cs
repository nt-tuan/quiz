using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Web.Migrations
{
  public partial class adduser : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "users",
          columns: table => new
          {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            first_name = table.Column<string>(type: "text", nullable: true),
            last_name = table.Column<string>(type: "text", nullable: true),
            username = table.Column<string>(type: "text", nullable: true),
            password = table.Column<string>(type: "text", nullable: true),
            created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            created_by = table.Column<string>(type: "text", nullable: true),
            deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            deleted_by = table.Column<string>(type: "text", nullable: true),
            updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            updated_by = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_users", x => x.id);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "users");
    }
  }
}
