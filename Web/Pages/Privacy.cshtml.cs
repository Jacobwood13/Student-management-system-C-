using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementSystem;

namespace web.Pages;

public class PrivacyModel : PageModel
{
    public void OnGet()
    {
        var a = new DatabaseManager();
        var s = a.GetAllStudents();
        this.OrganisationName = s.FirstOrDefault()?.Name ?? "PAM Group";
    }
    public string OrganisationName { get; set; } = string.Empty;

}
