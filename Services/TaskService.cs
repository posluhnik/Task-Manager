using System.Text.Json;
using MyFirstSite.Models;

namespace MyFirstSite.Services;

public class TaskService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "tasks.json");

    public List<TaskItem> GetAll()
    {
        if (!File.Exists(_filePath)) return new List<TaskItem>();
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
    }

    public void Save(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.ReadAllText(_filePath);
        File.WriteAllText(_filePath, json);
    }

    public void Add(TaskItem task)
    {
        var tasks = GetAll();
        tasks.Add(task);
        Save(tasks);
    }

    public void Delete(Guid id)
    {
        var tasks = GetAll();
        tasks.RemoveAll(t => t.Id == id);
        Save(tasks);
    }
}