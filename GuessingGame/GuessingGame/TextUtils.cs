namespace GuessingGame
{
    internal static class TextUtils
    {
        public static string GetArticle(string description)
        {
            return "aeiou".Contains(description.ToLower()[0].ToString())
                ? "an " + description
                : "a " + description;
        }
    }
}