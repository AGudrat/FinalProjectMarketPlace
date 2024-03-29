﻿namespace MarketPlace.Catalog.Services
{
    public static class NameOperation
    {
        public static string CharacterRegulatory(string name)
            => name.Replace("\"", "")
                       .Replace("!", "")
                       .Replace("@", "")
                       .Replace("#", "")
                       .Replace("$", "")
                       .Replace("%", "")
                       .Replace("^", "")
                       .Replace("&", "")
                       .Replace("^", "")
                       .Replace("*", "")
                       .Replace("(", "")
                       .Replace(")", "")
                       .Replace("_", "")
                       .Replace("-", "")
                       .Replace("+", "")
                       .Replace("=", "")
                       .Replace("№", "")
                       .Replace(";", "")
                       .Replace("?", "")
                       .Replace(":", "")
                       .Replace(";", "")
                       .Replace("'", "")
                       .Replace("[", "")
                       .Replace("]", "")
                       .Replace("{", "")
                       .Replace("}", "")
                       .Replace("<", "")
                       .Replace(">", "")
                       .Replace("/", "")
                       .Replace(",", "")
                       .Replace("~", "")
                       .Replace("`", "")
                       .Replace(".", "-")
                       .Replace("ß", "")
                       .Replace("â", "a")
                       .Replace("î", "i")
                       .Replace("€", "")
                       .Replace("|", "")
                       .Replace("ö", "o")
                       .Replace("Ö", "o")
                       .Replace("ğ", "g")
                       .Replace("Ğ", "g")
                       .Replace("ə", "e")
                       .Replace("Ə", "e")
                       .Replace("ş", "s")
                       .Replace("Ş", "s")
                       .Replace("ç", "c")
                       .Replace("Ç", "c")
                       .Replace("ı", "i")
                       .Replace("I", "i")
                       .Replace("ü", "u")
                       .Replace("Ü", "u")
                       .Replace("æ", "");
    }
}
