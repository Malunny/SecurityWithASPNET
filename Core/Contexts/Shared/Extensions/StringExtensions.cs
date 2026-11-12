using System.Text;

namespace Domain.Contexts.Shared.Extensions;

public static class StringExtensions
{
    public static string ToBase64(this string value) => Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
}