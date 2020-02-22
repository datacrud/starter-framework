namespace Project.Core.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str) || string.IsNullOrEmpty(str);
        }

        public static string RemoveSpace(this string name)
        {
            var replace = name.Replace(" ", "");
            return replace;
        }

        public static string ToTenancyName(this string name)
        {
            var replace = name.Replace(" ", "");
            replace = replace.Replace(".", "");
            replace = replace.Replace("-", "");
            replace = replace.Replace("_", "");
            replace = replace.Replace(",", "");
            return replace.ToLower();
        }

        public static string RemoveDash(this string name)
        {            
            var replace = name.Replace("-", "");
            return replace;
        }

        public static string ToIdentityGeneratedTokenFormat(this string name)
        {
            return name.Replace(" ", "+");
        }


        public static string ToCapitalize(this string str)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string ToTenantBaseUrl(this string tenancyName)
        {
            return $"http://{tenancyName}.datacrud.com".ToLower();
        }

    }
}
