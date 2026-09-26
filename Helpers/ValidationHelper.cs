using System.Text.RegularExpressions;

namespace Reklaim_frontend.Helpers;

public static class ValidationHelper
{
    private static readonly Regex EmailShapeRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled);

    /// <summary>
    /// Checks that the email looks like a real address AND ends with the
    /// university's allowed domain (e.g. "@university.edu").
    /// </summary>
    public static bool IsValidUniversityEmail(string? email, string allowedDomain)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var trimmed = email.Trim();

        if (!EmailShapeRegex.IsMatch(trimmed))
        {
            return false;
        }

        return trimmed.EndsWith(allowedDomain, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsValidPassword(string? password, out string? error)
    {
        error = null;

        if (string.IsNullOrWhiteSpace(password))
        {
            error = "Password is required.";
            return false;
        }

        if (password.Length < 8)
        {
            error = "Password must be at least 8 characters.";
            return false;
        }

        return true;
    }

    public static bool IsRequired(string? value, string fieldLabel, out string? error)
    {
        error = string.IsNullOrWhiteSpace(value) ? $"{fieldLabel} is required." : null;
        return error is null;
    }
}