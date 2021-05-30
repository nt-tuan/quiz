using Microsoft.EntityFrameworkCore.Migrations;

namespace dmc_auth.Migrations
{
    public partial class fixnaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelCollection_Colletions_CollectionId",
                table: "LabelCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colletions",
                table: "Colletions");

            migrationBuilder.RenameTable(
                name: "Colletions",
                newName: "Collections");

            migrationBuilder.RenameIndex(
                name: "IX_Colletions_Slug_DeletedAt",
                table: "Collections",
                newName: "IX_Collections_Slug_DeletedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collections",
                table: "Collections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelCollection_Collections_CollectionId",
                table: "LabelCollection",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelCollection_Collections_CollectionId",
                table: "LabelCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collections",
                table: "Collections");

            migrationBuilder.RenameTable(
                name: "Collections",
                newName: "Colletions");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_Slug_DeletedAt",
                table: "Colletions",
                newName: "IX_Colletions_Slug_DeletedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colletions",
                table: "Colletions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelCollection_Colletions_CollectionId",
                table: "LabelCollection",
                column: "CollectionId",
                principalTable: "Colletions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
