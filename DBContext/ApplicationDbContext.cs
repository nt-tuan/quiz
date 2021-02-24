using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.DBContext
{
  public class ApplicationDbContext : DbContext
  {
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<LabelKey> LabelKeys { get; set; }

    public ApplicationDbContext(
        DbContextOptions options) : base(options)
    {
      ChangeTracker.StateChanged += UpdateTimestamps;
    }

    private static void UpdateTimestamps(object sender, EntityEntryEventArgs e)
    {
      if (e.Entry.Entity is BaseEntity entity)
      {
        switch (e.Entry.State)
        {
          case EntityState.Deleted:
            entity.DeletedAt = DateTimeOffset.Now;
            e.Entry.State = EntityState.Modified;
            break;
          case EntityState.Modified:
            entity.UpdatedAt = DateTimeOffset.Now;
            break;
          case EntityState.Added:
            entity.CreatedAt = DateTimeOffset.Now;
            break;
        }
      }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Question>().HasOne(u => u.Exam).WithMany(u => u.Questions).HasForeignKey(u => u.ExamId);
      modelBuilder.Entity<AnswerOption>().HasOne(u => u.Question).WithMany(u => u.AnswerOptions).HasForeignKey(u => u.QuestionId);
      modelBuilder.Entity<LabelKey>().HasKey(u => u.Key);
      modelBuilder.Entity<Label>().HasIndex(u => new { u.DeletedAt, u.KeyId, u.Value });
      modelBuilder.Entity<Exam>().HasMany(u => u.Labels).WithMany(label => label.Exams).UsingEntity<ExamLabel>(
        r => r.HasOne(u => u.Label).WithMany(),
        r => r.HasOne(u => u.Exam).WithMany()
      );
      modelBuilder.Entity<Exam>().HasIndex(u => new { u.Slug, u.DeletedAt }).IsUnique();

    }
  }
}
