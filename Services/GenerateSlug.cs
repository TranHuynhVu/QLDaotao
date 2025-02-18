using System.Globalization;
using System.Text;

namespace QLDaoTao.Services
{
    public class GenerateSlug
    {
        public static string Slugify(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            // Convert Vietnamese characters to Latin characters
            input = RemoveDiacritics(input);

            var stringBuilder = new StringBuilder();
            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c) || c == '-')
                {
                    stringBuilder.Append(c);
                }
                else if (char.IsWhiteSpace(c))
                {
                    stringBuilder.Append("-");
                }
            }

            return stringBuilder.ToString().ToLower();
        }

        private static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
