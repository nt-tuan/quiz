using Microsoft.EntityFrameworkCore;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Data
{
  public class ApplicationDbContext : DbContext
  {
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }

    public ApplicationDbContext(
        DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Question>().HasOne(u => u.Exam).WithMany(u => u.Questions).HasForeignKey(u => u.ExamId);
      modelBuilder.Entity<AnswerOption>().HasOne(u => u.Question).WithMany(u => u.AnswerOptions).HasForeignKey(u => u.QuestionId);
    }
  }
}
