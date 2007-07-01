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
 * 0.2.1 - Entity generator creation
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BdsSoft.SharePoint.Linq.Tools.EntityGenerator
{
    /// <summary>
    /// Helper class to parse and validate the command-line arguments.
    /// </summary>
    public class EntityGeneratorArgs
    {
        /// <summary>
        /// Url to the SharePoint site.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// User name to connect to the SharePoint web services (optional).
        /// </summary>
        /// <remarks>If not specified, integrated authentication will be used using default network credentials.</remarks>
        public string User { get; set; }

        /// <summary>
        /// Password to connect to the SharePoint web services (optional).
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Domain name to connect to the SharePoint web services (optional).
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Namespace to put code in.
        /// </summary>
        public string Namespace { get; set; }
    }
}
