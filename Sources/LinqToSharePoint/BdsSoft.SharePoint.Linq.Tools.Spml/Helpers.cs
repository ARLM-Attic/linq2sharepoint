using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using System.Net;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    static class Helpers
    {
        public static void GetLists(Context context)
        {
            WebServices.Lists lists = GetListsProxy(context.ConnectionParameters);
            XmlNode listsXml = lists.GetListCollection();

            List<List> result = new List<List>();

            foreach (XmlNode l in listsXml.ChildNodes)
            {
                if (l.Attributes["Hidden"].Value.ToLower() != "true")
                    result.Add(List.FromCaml(l));
            }

            context.Lists = result;
        }

        public static List GetList(Context context, string name)
        {
            WebServices.Lists lists = GetListsProxy(context.ConnectionParameters);
            XmlNode listXml = lists.GetList(name);

            return List.FromCaml(listXml);
        }

        private static WebServices.Lists GetListsProxy(ConnectionParameters parameters)
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
