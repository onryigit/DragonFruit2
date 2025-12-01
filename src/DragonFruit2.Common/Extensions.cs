using System.Text;

namespace DragonFruit2.Common;

public static class Extensions
{
    extension(string s)
    {
        public string ToKebabCase()
        {
            if (string.IsNullOrEmpty(s)) return s;
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (char.IsUpper(c))
                {
                    if (i > 0 && (char.IsLower(s[i - 1]) || char.IsDigit(s[i - 1])))
                        sb.Append('-');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().ToLower();
        }
    }
}
