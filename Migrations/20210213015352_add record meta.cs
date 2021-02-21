using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dmc_auth.Migrations
{
  public partial class addrecordmeta : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "CreatedAt",
          table: "Questions",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "CreatedBy",
          table: "Questions",
          nullable: true);

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "DeletedAt",
          table: "Questions",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "DeletedBy",
          table: "Questions",
          nullable: true);

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "CreatedAt",
          table: "Exams",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "CreatedBy",
          table: "Exams",
          nullable: true);

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "DeletedAt",
          table: "Exams",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "DeletedBy",
          table: "Exams",
          nullable: true);

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "CreatedAt",
          table: "AnswerOptions",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "CreatedBy",
          table: "AnswerOptions",
          nullable: true);

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "DeletedAt",
          table: "AnswerOptions",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "DeletedBy",
          table: "AnswerOptions",
          nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "CreatedAt",
          table: "Questions");

      migrationBuilder.DropColumn(
          name: "CreatedBy",
          table: "Questions");

      migrationBuilder.DropColumn(
          name: "DeletedAt",
          table: "Questions");

      migrationBuilder.DropColumn(
          name: "DeletedBy",
          table: "Questions");

      migrationBuilder.DropColumn(
          name: "CreatedAt",
          table: "Exams");

      migrationBuilder.DropColumn(
          name: "CreatedBy",
          table: "Exams");

      migrationBuilder.DropColumn(
          name: "DeletedAt",
          table: "Exams");

      migrationBuilder.DropColumn(
          name: "DeletedBy",
          table: "Exams");

      migrationBuilder.DropColumn(
          name: "CreatedAt",
          table: "AnswerOptions");

      migrationBuilder.DropColumn(
          name: "CreatedBy",
          table: "AnswerOptions");

      migrationBuilder.DropColumn(
          name: "DeletedAt",
          table: "AnswerOptions");

      migrationBuilder.DropColumn(
          name: "DeletedBy",
          table: "AnswerOptions");
    }
  }
}
