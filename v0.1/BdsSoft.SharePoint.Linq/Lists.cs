namespace BdsSoft.SharePoint.Linq
{
    using System;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Web.Services.Description;
    using System.Xml;

    [WebServiceBindingAttribute(Name = "ListsSoap", Namespace = "http://schemas.microsoft.com/sharepoint/soap/")]
    internal class Lists : SoapHttpClientProtocol
    {
        [SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetList", RequestNamespace = "http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace = "http://schemas.microsoft.com/sharepoint/soap/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public XmlNode GetList(string listName)
        {
            object[] results = this.Invoke("GetList", new object[] { listName });
            return (XmlNode)(results[0]);
        }

        [SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListItems", RequestNamespace = "http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace = "http://schemas.microsoft.com/sharepoint/soap/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public XmlNode GetListItems(string listName, string viewName, XmlNode query, XmlNode viewFields, string rowLimit, XmlNode queryOptions, string webID)
        {
            object[] results = this.Invoke("GetListItems", new object[] { listName, viewName, query, viewFields, rowLimit, queryOptions, webID });
            return (XmlNode)(results[0]);
        }
    }
}
