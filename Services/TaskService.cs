using System.Text.Json;
using MyFirstSite.Models;

namespace MyFirstSite.Services;

public class TaskService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "tasks.json");

    private List<TaskItem> LoadAllFromFile()
    {
        if (!File.Exists(_filePath)) return new List<TaskItem>();
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
    }

    private void SaveAllToFile(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public List<TaskItem> GetByUserId(string userId)
    {
        return LoadAllFromFile().Where(t => t.UserId == userId).ToList();
    }

    public void Add(TaskItem task)
    {
        var tasks = LoadAllFromFile();
        tasks.Add(task);
        SaveAllToFile(tasks);
    }

    public void Delete(Guid id, string userId)
    {
        var tasks = LoadAllFromFile();
        tasks.RemoveAll(t => t.Id == id && t.UserId == userId);
        SaveAllToFile(tasks);
    }
}