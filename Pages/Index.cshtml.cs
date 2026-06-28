using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstSite.Models;
using MyFirstSite.Services;

namespace MyFirstSite.Pages;

public class IndexModel : PageModel
{
    private readonly TaskService _taskService;
    public IndexModel(TaskService taskService)
    {
        _taskService = taskService;
    }


    public List<TaskItem> Tasks { get; set; } = new();

    [BindProperty]
    public TaskItem NewTask { get; set; } = new();

    public void OnGet()
    {
        Tasks = _taskService.GetAll();
    }

    public IActionResult OnPostAdd()
    {
        if (!string.IsNullOrWhiteSpace(NewTask.Title))
        {
            _taskService.Add(NewTask);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(Guid id)
    {
        _taskService.Delete(id);
        return RedirectToPage();
    }
}