using System;
using System.Text.RegularExpressions;

namespace RefactoredCommandSystem.Core.Shared
{
    public static class IdentifierFactory
    {
        private static readonly Regex NonWord = new("[^a-z0-9]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string CreateFromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Guid.NewGuid().ToString("N");
            }

            var normalized = NonWord.Replace(name.Trim().ToLowerInvariant(), "-").Trim('-');
            return string.IsNullOrWhiteSpace(normalized) ? Guid.NewGuid().ToString("N") : normalized;
        }
    }
}

