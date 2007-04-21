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
}