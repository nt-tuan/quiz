using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Quiz.DBContext;
using ThanhTuan.Scripts.UpdateQuiz.Models;
using Slugify;
using ThanhTuan.Quiz.Repositories;
using System.Linq;

namespace Thanhtuan.Scripts.UpdateQuiz
{
  class Program
  {
    static string DbUrl;
    static ThanhTuan.Quiz.Entities.AnswerOption ToAnswerOption(Answer answer)
    {
      return new ThanhTuan.Quiz.Entities.AnswerOption
      {
        Content = answer.Content,
        IsCorrect = answer.IsCorrect
      };
    }

    static ThanhTuan.Quiz.Entities.Question ToQuestion(Question question)
    {
      return new ThanhTuan.Quiz.Entities.Question
      {
        Content = question.Content,
        AnswerOptions = question.Answers.Select(ToAnswerOption).ToList()
      };
    }
    static List<Quiz> ReadData(string dir)
    {
      var files = Directory.GetFiles(dir);
      var quizs = new List<Quiz>();
      Console.WriteLine("Reading files");
      foreach (var file in files)
      {
        if (file.Contains("error")) continue;
        var text = File.ReadAllText(file);
        var newQuizs = JsonSerializer.Deserialize<List<Quiz>>(text, new JsonSerializerOptions
        {
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        quizs.AddRange(newQuizs);
      }
      return quizs;
    }
    static ApplicationDbContext GetDbContext()
    {
      var dbOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
      dbOptionsBuilder = dbOptionsBuilder.UseNpgsql(DbUrl);
      var dbContext = new ApplicationDbContext(dbOptionsBuilder.Options);
      return dbContext;
    }
    static string TranslateTagKey(string key)
    {
      if (key == "title") return "topic";
      return key;
    }
    static List<ThanhTuan.Quiz.Entities.Label> GetLabels(Dictionary<string, string> tag, List<ThanhTuan.Quiz.Entities.Label> existedLabels)
    {
      var slugHelper = new SlugHelper();
      var labels = tag.Select(pair => new ThanhTuan.Quiz.Entities.Label
      {
        KeyId = TranslateTagKey(pair.Key),
        Value = slugHelper.GenerateSlug(pair.Value),
        DisplayName = pair.Value,
        IsVisible = "true"
      }).ToList();
      var repo = new LabelRepository(GetDbContext());
      labels = labels.Select(label =>
      {
        var existedLabel = existedLabels.FirstOrDefault(existedLabel => existedLabel.Value == label.Value);
        if (existedLabel != null)
        {
          return existedLabel;
        }
        var newLabel = repo.AddLabel(label, "admin").Result;
        existedLabels.Add(newLabel);
        return newLabel;
      }).ToList();
      return labels;
    }
    static void Execute(List<Quiz> quizs)
    {
      var dbContext = GetDbContext();
      var repo = new ExamRepository(dbContext);
      var slugHelper = new SlugHelper();
      var labelRepo = new LabelRepository(GetDbContext());
      var existedLabels = labelRepo.GetLabels().Result;
      foreach (var quiz in quizs)
      {
        var slug = slugHelper.GenerateSlug(quiz.Subtitle + "-" + quiz.Id.ToString());
        var existedExam = repo.GetExamBySlug(slug).Result;
        if (existedExam != null) continue;
        var newExam = new ThanhTuan.Quiz.Entities.Exam
        {
          Slug = slug,
          Title = quiz.Subtitle,
          Description = quiz.Title,
          IsVisible = true,
          Questions = quiz.Questions.Select(ToQuestion).ToList(),
        };
        var addedExam = repo.AddExam(newExam, "admin").Result;
        var labels = GetLabels(quiz.Tag, existedLabels);
        foreach (var label in labels)
        {
          repo.AttachLabel(addedExam.Id, label.Id).Wait();
        }
        Console.WriteLine($"Inserted {addedExam.Id} - {addedExam.Title}");
      }
    }
    static void Main(string[] args)
    {
      DbUrl = "Host=127.0.0.1;Database=quiz;Username=quiz;Password=quiz123";
      var quizs = ReadData("./Content/exams_v2");
      Console.WriteLine(quizs.Count);
      // var offset = 0;

      // while (offset < quizs.Count)
      // {
      //   var chuck = quizs.Skip(offset).Take(100).ToList();
      //   Console.WriteLine($"Updating quiz {offset} -> {offset + 100}");
      //   Execute(chuck);
      //   offset += 100;
      // }
    }
  }
}
