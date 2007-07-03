/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BdsSoft.SharePoint.Linq.Tools.EntityGenerator
{
    /// <summary>
    /// Set of helper methods used in code generation.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Get a friendly name for a field, usable in C# or VB as a property name.
        /// </summary>
        /// <param name="name">Name to create a friendly name for.</param>
        /// <returns>Friendly name for given name.</returns>
        public static string GetFriendlyName(string name)
        {
            //
            // Find parts of the name, separated by a space, dash or underscore, and create a Pascal-cased name.
            //
            StringBuilder sb = new StringBuilder();
            foreach (string s in name.Split(' ', '-', '_'))
            {
                //
                // Keep only letters and digits.
                //
                string s2 = SanitizeString(s);

                //
                // Process string if not empty.
                //
                if (s2 != String.Empty)
                {
                    //
                    // First letter of a part should be capitalized.
                    //
                    sb.Append(char.ToUpper(s2[0]));

                    //
                    // Leave original casing for the remainder.
                    //
                    if (s2.Length > 1)
                        sb.Append(s2.Substring(1));
                }
            }

            //
            // Check for starting with digit case.
            //
            string res = sb.ToString();
            if (res.Length != 0 && char.IsDigit(res[0]))
                res = '_' + res;

            return res;
        }

        /// <summary>
        /// Keeps only letters and digits from a given string.
        /// </summary>
        /// <param name="s">String to be sanitized.</param>
        /// <returns>Sanitized string.</returns>
        public static string SanitizeString(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);

            return sb.ToString();
        }
    }
}
