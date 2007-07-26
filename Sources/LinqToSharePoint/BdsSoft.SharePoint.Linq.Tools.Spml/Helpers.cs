/*
 * LINQ to SharePoint
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
 * 0.2.2 - Introduction of entity wizard
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using System.Net;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    static class Helpers
    {
        public static void GetLists(WizardContext context)
        {
            WebServices.Lists lists = GetListsProxy(context.ConnectionParameters.Parameters);
            XmlNode listsXml = lists.GetListCollection();

            List<List> result = new List<List>();

            foreach (XmlNode l in listsXml.ChildNodes)
            {
                if (l.Attributes["Hidden"].Value.ToLower() != "true")
                    result.Add(List.FromCaml(l));
            }

            context.Lists = result;
        }

        public static List GetList(WizardContext context, string name)
        {
            WebServices.Lists lists = GetListsProxy(context.ConnectionParameters.Parameters);
            XmlNode listXml = lists.GetList(name);

            return List.FromCaml(listXml);
        }

        private static WebServices.Lists GetListsProxy(Connection parameters)
        {
            WebServices.Lists lists = new WebServices.Lists();
            lists.Url = parameters.Url.TrimEnd('/') + "/_vti_bin/lists.asmx"; ;

            if (!parameters.CustomAuthentication)
                lists.Credentials = CredentialCache.DefaultNetworkCredentials;
            else
                lists.Credentials = new NetworkCredential(parameters.User, parameters.Password, parameters.Domain);

            return lists;
        }
    }
}
