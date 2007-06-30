/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

/*
 * Version history:
 * 
 * 0.2.0 - Restructuring of class files in project
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BdsSoft.SharePoint.Linq.Tools.SpMetal
{
    /// <summary>
    /// Internal helper class to parse and validate the command-line arguments.
    /// </summary>
    internal class Args
    {
        /// <summary>
        /// Url to the SharePoint site.
        /// </summary>
        public string Url;

        /// <summary>
        /// Name of the list on the SharePoint site to query for.
        /// </summary>
        public string List;

        /// <summary>
        /// Output file (optional).
        /// </summary>
        /// <remarks>If no output file is specified, the list's name will be used with an extension of .cs.</remarks>
        public string File;

        /// <summary>
        /// User name to connect to the SharePoint web services (optional).
        /// </summary>
        /// <remarks>If not specified, integrated authentication will be used using default network credentials.</remarks>
        public string User;

        /// <summary>
        /// Password to connect to the SharePoint web services (optional).
        /// </summary>
        public string Password;

        /// <summary>
        /// Domain name to connect to the SharePoint web services (optional).
        /// </summary>
        public string Domain;

        /// <summary>
        /// Output code language.
        /// </summary>
        public string Language;

        /// <summary>
        /// Parses the given command-line arguments.
        /// </summary>
        /// <param name="args">Arguments to be parsed.</param>
        /// <returns>An instance of Args containing the argument values; null if the argument list is invalid.</returns>
        internal static Args Parse(string[] args)
        {
            //
            // Valid argument count, length and prefixes?
            //
            if (!CheckArgs(args))
                return null;

            Args res = new Args();

            //
            // Url shouldn't be empty and should start with either http:// or https://.
            //
            string url = FindArg(args, "url");
            if (url == null || url.Length == 0)
                return null;
            res.Url = url;
            url = url.ToLower();
            if (!(url.StartsWith("http://") && url.Length > 7) && !(url.StartsWith("https://") && url.Length > 8))
                return null;

            //
            // List shouldn't be empty.
            //
            string list = FindArg(args, "list");
            if (list == null || list.Length == 0)
                return null;
            res.List = list;

            //
            // Output language can be set optionally and should be CS or VB (case-insensitive).
            //
            string language = FindArg(args, "language");
            if (language != null && language.Length != 0)
            {
                language = language.ToUpper();

                if (language != "CS" && language != "VB")
                    return null;
                else
                    res.Language = language;
            }
            else
                res.Language = "CS";

            //
            // Out can optionally be empty; in that case, take the lists's name suffixed with the language extension.
            // v0.2.0.0 - File name will be deferred from the loaded list name, not the -list parameter which could be a GUID too.
            //
            res.File = FindArg(args, "out");

            //
            // Only process authentication arguments if a user argument is present.
            //
            string user = FindArg(args, "user");
            if (user != null && user.Length != 0)
            {
                //
                // Password required if user has been specified. Password can be the empty string.
                //
                string password = FindArg(args, "password");
                if (password != null)
                {
                    res.User = user;
                    res.Password = password;

                    //
                    // Optional domain name.
                    //
                    string domain = FindArg(args, "domain");
                    if (domain != null && domain.Length != 0)
                        res.Domain = domain;
                }
                else
                    return null;
            }

            return res;
        }

        /// <summary>
        /// Checks that all arguments have a valid prefix ('/' or '-') and have a suitable minimal length.
        /// </summary>
        /// <param name="args">Arguments to be checked.</param>
        /// <returns>True if valid; false otherwise.</returns>
        private static bool CheckArgs(string[] args)
        {
            //
            // Minimum required: -url and -list
            //
            if (args.Length < 2)
                return false;

            foreach (string arg in args)
            {
                //
                // Check for valid prefix.
                //
                if (!arg.StartsWith("-") && !arg.StartsWith("/"))
                    return false;

                //
                // Minimal length as in "-out:x"
                //
                if (arg.Length <= 5)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Finds an argument with the specified name.
        /// </summary>
        /// <param name="args">Arguments collection to search in.</param>
        /// <param name="prefix">Name of the argument requested.</param>
        /// <returns>Value of the argument, i.e. the portion after the ':' without optional double quotes; null if the argument isn't found.</returns>
        private static string FindArg(string[] args, string prefix)
        {
            //
            // Prefix should end with :
            //
            prefix += ":";
            int n = prefix.Length;

            foreach (string arg in args)
            {
                //
                // Trim the '-' or '/'.
                //
                string a = arg.Substring(1);

                //
                // Case-insensitive prefix-match. Returns the "-trimmed version of the argument value.
                //
                if (a.ToLower().StartsWith(prefix) && a.Length >= n)
                    return a.Substring(n).Trim('\"');
            }

            return null;
        }
    }
}
