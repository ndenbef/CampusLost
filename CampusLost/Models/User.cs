namespace CampusLost.Models;

public class User
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Campus { get; set; } = "Campus centre";
    public string FullName => $"{FirstName} {LastName}".Trim();
}
