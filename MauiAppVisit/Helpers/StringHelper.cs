using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MauiAppVisit.Helpers
{
    public static class StringHelper
    {
        public static bool CheckAccent(string text)
        {
            Regex regex = new Regex("[áàâãäéèêëíìîïóòôõöúùûüçÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÇ]");
            return regex.IsMatch(text);
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();

            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in arrayText)
            { if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark) { sbReturn.Append(letter); } }

            return sbReturn.ToString();
        }

        public static string RemoveCharacteresSpecials(string input)
        {
            string pattern = @"[^a-zA-Z0-9 ]";
            string replacement = "";

            string result = Regex.Replace(input, pattern, replacement);

            return result;
        }
    }
}
