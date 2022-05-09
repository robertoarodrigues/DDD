namespace DDD.Core.Utils;
public static class StringUtils
{
    public static string OnlyNuber(this string str, string input)
    {
        return new string(input.Where(char.IsDigit).ToArray());
    }
}
