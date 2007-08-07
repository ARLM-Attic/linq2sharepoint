''
'' LINQ to SharePoint
'' http://www.codeplex.com/LINQtoSharePoint
'' 
'' Copyright Bart De Smet (C) 2007
'' info@bartdesmet.net - http://blogs.bartdesmet.net/bart
'' 
'' This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
''

Imports BdsSoft.SharePoint.Linq
Imports Microsoft.SharePoint
Imports Tests

Public Class Helpers
    Public Shared Function StrCmp(ByVal site As SPSite) As IQueryable(Of People)
        Dim src = New SharePointList(Of People)(New SharePointDataContext(site))
        src.Context.CheckListVersion = False
        Dim res = From p In src _
                  Where StrComp(p.FirstName, "Bart") = 0 _
                  Select p
        Return res
    End Function

    Public Shared Function StrCompare(ByVal site As SPSite) As IQueryable(Of People)
        Dim src = New SharePointList(Of People)(New SharePointDataContext(site))
        src.Context.CheckListVersion = False
        Dim res = From p In src _
                  Where p.FirstName = "Bart" _
                  Select p
        Return res
    End Function
End Class
