using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace dmc_auth.Migrations
{
    public partial class collection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labels_DeletedAt_KeyId_Value",
                table: "Labels");

            migrationBuilder.CreateTable(
                name: "Colletions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colletions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabelCollection",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "integer", nullable: false),
                    LabelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelCollection", x => new { x.CollectionId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_LabelCollection_Colletions_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Colletions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelCollection_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Labels_DeletedAt_KeyId_Value",
                table: "Labels",
                columns: new[] { "DeletedAt", "KeyId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colletions_Slug_DeletedAt",
                table: "Colletions",
                columns: new[] { "Slug", "DeletedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabelCollection_LabelId",
                table: "LabelCollection",
                column: "LabelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelCollection");

            migrationBuilder.DropTable(
                name: "Colletions");

            migrationBuilder.DropIndex(
                name: "IX_Labels_DeletedAt_KeyId_Value",
                table: "Labels");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_DeletedAt_KeyId_Value",
                table: "Labels",
                columns: new[] { "DeletedAt", "KeyId", "Value" });
        }
    }
}
