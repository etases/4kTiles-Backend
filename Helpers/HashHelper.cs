namespace _4kTiles_Backend.Helpers;

public static class HashHelper
{
    /// <summary>
    /// Hash the text
    /// </summary>
    /// <param name="text">the text</param>
    /// <returns>the hashed string</returns>
    public static string Hash(this string text) => BCrypt.Net.BCrypt.HashPassword(text);

    /// <summary>
    /// Verify the text with the hashed string
    /// </summary>
    /// <param name="text">the text</param>
    /// <param name="hash">the hashed string</param>
    /// <returns></returns>
    public static bool VerifyHash(this string text, string hash) => BCrypt.Net.BCrypt.Verify(text, hash);
}