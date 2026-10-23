namespace CampusLost.Helpers;

/// <summary>Couleurs utilisables depuis le C# (miroir de Colors.xaml).</summary>
public static class Theme
{
    public static readonly Color Green = Color.FromArgb("#00A878");
    public static readonly Color GreenSoft = Color.FromArgb("#E6F6EE");
    public static readonly Color Ink = Color.FromArgb("#17324D");
    public static readonly Color Muted = Color.FromArgb("#7A90A4");
    public static readonly Color Line = Color.FromArgb("#DCE6EE");
    public static readonly Color LineStrong = Color.FromArgb("#D5DEE6");
    public static readonly Color Red = Color.FromArgb("#E5586B");
    public static readonly Color RedSoft = Color.FromArgb("#FDEBEE");
}

public static class Validation
{
    public static bool IsEmail(string? s) =>
        !string.IsNullOrWhiteSpace(s) &&
        System.Text.RegularExpressions.Regex.IsMatch(s.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static bool IsStrongPassword(string? s) =>
        s is { Length: >= 8 } && s.Any(char.IsLetter) && s.Any(char.IsDigit);
}
