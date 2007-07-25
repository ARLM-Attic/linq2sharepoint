using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using EG = BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using System.Net;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    static class Helpers
    {
        public static void GetLists(Context context)
        {
            WebServices.Lists lists = GetListsProxy(context.ConnectionParameters.Parameters);
            XmlNode listsXml = lists.GetListCollection();

            List<EG.List> result = new List<EG.List>();

            foreach (XmlNode l in listsXml.ChildNodes)
            {
                if (l.Attributes["Hidden"].Value.ToLower() != "true")
                    result.Add(EG.List.FromCaml(l));
            }

            context.Lists = result;
        }

        public static EG.List GetList(Context context, string name)
        {
            WebServices.Lists lists = GetListsProxy(context.ConnectionParameters.Parameters);
            XmlNode listXml = lists.GetList(name);

            return EG.List.FromCaml(listXml);
        }

        private static WebServices.Lists GetListsProxy(EG.Connection parameters)
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
