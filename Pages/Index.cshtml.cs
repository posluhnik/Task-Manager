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

    public string CurrentUserId { get; set; } = string.Empty;

    private string GetOrCreateUserId()
    {
        if (Request.Cookies.TryGetValue("UserIdentifier", out var userId) && !string.IsNullOrEmpty(userId))
        {
            return userId;
        }

        var newUserId = Guid.NewGuid().ToString();

        Response.Cookies.Append("UserIdentifier", newUserId, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true
        });

        return newUserId;
    }

    public void OnGet()
    {
        CurrentUserId = GetOrCreateUserId();
        Tasks = _taskService.GetByUserId(CurrentUserId);
    }

    public IActionResult OnPostAdd()
    {
        CurrentUserId = GetOrCreateUserId();

        if (!string.IsNullOrWhiteSpace(NewTask.Title))
        {
            NewTask.UserId = CurrentUserId;
            _taskService.Add(NewTask);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(Guid id)
    {
        CurrentUserId = GetOrCreateUserId();
        _taskService.Delete(id, CurrentUserId);
        return RedirectToPage();
    }
}