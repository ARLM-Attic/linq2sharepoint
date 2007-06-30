Imports BdsSoft.SharePoint.Linq
Imports Microsoft.SharePoint
Imports Tests

Public Class Helpers
    Public Shared Function StrCmp(ByVal site As SPSite) As SharePointDataSource(Of People)
        Dim src As SharePointDataSource(Of People) = New SharePointDataSource(Of People)(site)
        src.CheckListVersion = False
        Dim res = From p In src _
                  Where StrComp(p.FirstName, "Bart") = 0 _
                  Select p
        Return res
    End Function

    Public Shared Function StrCompare(ByVal site As SPSite) As SharePointDataSource(Of People)
        Dim src As SharePointDataSource(Of People) = New SharePointDataSource(Of People)(site)
        src.CheckListVersion = False
        Dim res = From p In src _
                  Where p.FirstName = "Bart" _
                  Select p
        Return res
    End Function
End Class
