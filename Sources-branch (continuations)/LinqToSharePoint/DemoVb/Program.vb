''
'' LINQ-to-SharePoint
'' http://www.codeplex.com/LINQtoSharePoint
'' 
'' Copyright Bart De Smet (C) 2007
'' info@bartdesmet.net - http://blogs.bartdesmet.net/bart
'' 
'' This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
''

''
'' NOTE: This class is used as a first collection of tests that will be replaced by a more complete unit test framework over time.
''       Because of the tight coupling with the SharePoint configuration and list definitions on a test machine, this class will be
''       of little use to project "readers" except for its value as a set of sample queries.
''

Imports BdsSoft.SharePoint.Linq

Module Program

    Sub Main()
        Dim users = New SharePointList(Of Demo)(New SharePointDataContext(New Uri("http://localhost")))
        users.Context.Log = Console.Out
        users.Context.CheckListVersion = False


        Dim d1 As Demo = users.GetEntityById(1)
        Dim d2 As Demo = users.GetEntityById(2)
        Dim d3 As Demo = users.GetEntityById(3)


        Console.WriteLine("QUERY 1")
        Console.WriteLine("-------" & vbCrLf)

        Dim res1 = From u In users Select u

        For Each s In res1
            Console.WriteLine("{0} {1} = {2} is {3}", s.FirstName, s.LastName, s.FullName, s.MembershipType)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 2")
        Console.WriteLine("-------" & vbCrLf)

        Dim res2 = From u In users Select u.FullName

        For Each s In res2
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 3")
        Console.WriteLine("-------" & vbCrLf)

        Dim res3 = From u In users _
                   Select Name = u.FullName, Age = u.Age

        For Each s In res3
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 4")
        Console.WriteLine("-------" & vbCrLf)

        Dim res4 = From u In users _
                   Order By u.Age Descending _
                    Select Name = u.FullName, Age = u.Age

        For Each s In res4
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 5")
        Console.WriteLine("-------" & vbCrLf)

        Dim res5 = From u In users _
                       Order By u.FirstName Ascending, u.LastName Descending _
                       Select Name = u.FullName, Age = u.Age

        For Each s In res5
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 6")
        Console.WriteLine("-------" & vbCrLf)

        Dim res6 = From u In users _
                   Where u.FirstName.Equals("Bart") _
                       Select u.FirstName

        For Each s In res6
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        'PROBLEM: u.FirstName = "Bart" (FIXED)
        Console.WriteLine("QUERY 7")
        Console.WriteLine("-------" & vbCrLf)

        Dim res7 = From u In users _
                       Where (StrComp(u.FirstName, "Bart") = 0 And u.Age.Value >= 24) Or (u.LastName.StartsWith("De") And u.LastName.Contains("Smet")) _
                       Select u.FullName

        For Each s In res7
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 8")
        Console.WriteLine("-------" & vbCrLf)

        Dim res8 = From u In users _
                   Where u.Age.Value >= 24 _
                   Select u.FullName

        For Each s In res8
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 9")
        Console.WriteLine("-------" & vbCrLf)

        Dim res9 = From u In users _
        Where (24 <= u.Age.Value) _
                       Select u.FullName

        For Each s In res9
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 10")
        Console.WriteLine("--------" & vbCrLf)

        Dim res10 = From u In users _
                        Where Not (u.Age.Value > 24) _
                        Select u.FullName

        For Each s In res10
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 11")
        Console.WriteLine("--------" & vbCrLf)

        Dim res11 = From u In users _
                        Where Not (24 < u.Age.Value) _
                        Select u.FullName

        For Each s In res11
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        'PROBLEM: u.Age.Value * 2 (FIXED)
        Console.WriteLine("QUERY 12")
        Console.WriteLine("--------" & vbCrLf)

        Dim res12 = From u In users _
                        Select u.Age.Value * 2

        For Each s In res12
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 13")
        Console.WriteLine("--------" & vbCrLf)

        Dim res13 = From u In users _
                        Select QueryHelper(u)

        For Each s In res13
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 14")
        Console.WriteLine("--------" & vbCrLf)

        Dim res14 = (From u In users _
                     Select u).Take(1)

        For Each s In res14
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 15")
        Console.WriteLine("--------" & vbCrLf)

        'PROBLEM: usr.FirstName = "Johan" (FIXED)
        'PROBLEM: usr.FirstName <> "Johan" (FIXED)
        Dim res15 = (From usr In users _
                     Where usr.Age.Value >= 24 And (usr.FirstName.StartsWith("B") Or usr.FirstName = "Johan") And usr.Age.HasValue _
                     Order By usr.Age Descending, usr.FirstName Ascending _
                     Select usr.FirstName, usr.LastName, usr.Age, usr.MemberSince).Take(1)

        For Each s In res15
            Console.WriteLine(s)
        Next
        Console.WriteLine()

        'PROBLEM: Where ... And usr.ShortBio <> Nothing _ (FIXED)
        'PROBLEM: Select usr.Age (FIXED)
        Console.WriteLine("QUERY 16")
        Console.WriteLine("--------" & vbCrLf)

        Dim startDate As New DateTime(2007, 1, 1)
        Dim firstNameStartsWith As String = "B"
        Dim maximumAge1 As Integer = 24
        Dim lastNameContains As String = "Smet"
        Dim minimumAge2 As Integer = 50
        Dim res16 = From usr In users _
                        Where ((usr.FirstName.StartsWith(firstNameStartsWith) And Not (usr.Age.Value > maximumAge1)) _
                                  Or (usr.LastName.Contains(lastNameContains) And usr.Age.Value >= minimumAge2)) _
                              And usr.Member.Value = True _
                              And usr.ShortBio <> Nothing _
                              And usr.MemberSince.Value >= startDate _
                        Order By usr.MemberSince Descending, usr.Age Ascending _
                        Select Name = usr.FirstName & " " & usr.LastName, usr.Age, usr.Homepage, usr.ShortBio, usr.Activities, usr.MembershipType, usr.MemberSince, usr.FavoriteFood, usr.FavoriteFoodOther

        For Each s In res16
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 17")
        Console.WriteLine("--------" & vbCrLf)

        Dim res17 = From u In users Select "Constant"

        For Each s In res17
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 18")
        Console.WriteLine("--------" & vbCrLf)

        Dim res18 = From u In users Select String.Format("{0} is {1}", u.FirstName, u.Age)

        For Each s In res18
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        'PROBLEM: u.Age.Value (FIXED)
        Console.WriteLine("QUERY 19")
        Console.WriteLine("--------" & vbCrLf)

        Dim res19 = From u In users Select u.FirstName & " is " & IIf(u.Age.HasValue, u.Age.Value, "")

        For Each s In res19
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Try
            Console.WriteLine("QUERY 20")
            Console.WriteLine("--------" & vbCrLf)

            Dim res20 = From u In users Where (SomeBool(u.FirstName)) Select u

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + vbCrLf)
        End Try


        Try
            Console.WriteLine("QUERY 21")
            Console.WriteLine("--------" & vbCrLf)

            Dim res21 = From u In users Where u.FirstName.EndsWith("t") Select u

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + vbCrLf)
        End Try


        Try
            Console.WriteLine("QUERY 22")
            Console.WriteLine("--------" & vbCrLf)

            Dim res21 = From u In users Where Not u.FirstName.StartsWith("B") Select u

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + vbCrLf)
        End Try


        Console.WriteLine("QUERY 23")
        Console.WriteLine("--------" & vbCrLf)

        Dim res23 = From u In users Where (u.FavoriteFood.Value <= FavoriteFood.Pizza) Select u.FullName

        For Each s In res23
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 24")
        Console.WriteLine("--------" & vbCrLf)

        Dim res24 = From u In users Where u.Activities.Value <> Activities.Adventure Select u.FullName

        For Each s In res24
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 25")
        Console.WriteLine("--------" & vbCrLf)

        Dim res25 = From u In users Where (u.FavoriteFoodOther = "Spaghetti") Select u.FullName

        For Each s In res25
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 26")
        Console.WriteLine("--------" & vbCrLf)

        Dim res26 = From u In users _
                    Where u.FavoriteFood.Value <> FavoriteFood.Pizza _
                    Select u.FullName, u.FavoriteFood, u.FavoriteFoodOther

        For Each s In res26
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 27")
        Console.WriteLine("--------" & vbCrLf)

        Dim res27 = From u In users _
        Where (Not Not (u.Homepage <> Nothing)) _
                        Select u.FullName

        For Each s In res27
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 28")
        Console.WriteLine("--------" & vbCrLf)

        Dim res28 = From u In users _
                        Where Not Not (u.Homepage = Nothing) _
                        Select u.FullName

        For Each s In res28
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 29")
        Console.WriteLine("--------" & vbCrLf)

        'SEMANTICS!!!
        Dim res29 = From u In users _
                        Where u.Activities.Value = (Activities.Adventure Or Activities.Quiz) _
                        Select u.FullName

        For Each s In res29
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 30")
        Console.WriteLine("--------" & vbCrLf)

        'SEMANTICS!!!
        Dim res30 = From u In users _
                        Where (u.Activities.Value = (Activities.Adventure Or Activities.Quiz Or Activities.Culture)) _
                        Select u.FullName

        For Each s In res30
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 31")
        Console.WriteLine("--------" & vbCrLf)

        Dim res31 = From u In users _
                        Where u.Test.Value = Test.LaurelHardy _
                        Select u.FullName

        For Each s In res31
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Try
            Console.WriteLine("QUERY 32")
            Console.WriteLine("--------" & vbCrLf)

            Dim res32 = From u In users _
                            Where (u.Test.Value And Test.LaurelHardy) = Test.LaurelHardy _
                            Select u.FullName

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + "" & vbCrLf)
        End Try


        Console.WriteLine("QUERY 33")
        Console.WriteLine("--------" & vbCrLf)

        Dim res33 = From u In users _
                        Where u.FirstName = "Bart".ToString() _
                        Select u.FullName

        For Each s In res33
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 34")
        Console.WriteLine("--------" & vbCrLf)

        Dim res34 = From u In users _
                        Where u.FirstName.ToString().ToString() = "Bart" _
                        Select u.FullName

        For Each s In res34
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Try
            Console.WriteLine("QUERY 35")
            Console.WriteLine("--------" & vbCrLf)

            Dim res35 = From u In users _
                            Where u.FirstName = u.LastName _
                            Select u.FullName

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + "" & vbCrLf)
        End Try


        Console.WriteLine("QUERY 36")
        Console.WriteLine("--------" & vbCrLf)

        Dim res36 = From u In users _
                        Where CType(u.Age.Value, Integer) = 24 And CType(u.Age.Value, UInteger) = 24 And CType(u.Age.Value, Long) = 24 _
                        Select u.FullName

        For Each s In res36
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 37")
        Console.WriteLine("--------" & vbCrLf)

        Dim res37 = From u In users _
                        Where (u.Homepage = "http://www.bartdesmet.net") _
                        Select u.FullName

        For Each s In res37
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 38")
        Console.WriteLine("--------" & vbCrLf)

        Dim res38 = From u In users _
                        Where u.Homepage = New Uri("http://www.bartdesmet.net") _
                        Select u.FullName

        For Each s In res38
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 39")
        Console.WriteLine("--------" & vbCrLf)

        Try
            Dim res39 = From u In users _
                            Order By u.FirstName & "-" Ascending _
                            Select u

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + "" & vbCrLf)
        End Try


        Console.WriteLine("QUERY 40")
        Console.WriteLine("--------" & vbCrLf)

        Dim res40 = From u In users _
                        Order By u.FirstName.ToString() Ascending _
                        Select u.FullName

        For Each s In res40
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 41")
        Console.WriteLine("--------" & vbCrLf)

        Try
            Dim dummy As String = "Bart"
            Dim res41 = From u In users _
                            Order By dummy.Length Ascending _
                            Select u.FullName

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + "" & vbCrLf)
        End Try


        Console.WriteLine("QUERY 42")
        Console.WriteLine("--------" & vbCrLf)

        Try
            Dim res42 = From u In users _
                            Where (u.FirstName.StartsWith(u.LastName)) _
                            Select u

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + "" & vbCrLf)
        End Try


        Console.WriteLine("QUERY 43")
        Console.WriteLine("--------" & vbCrLf)

        Try
            Dim res43 = From u In users _
                            Where (u.Age.Value >= u.AccountBalance.Value) _
                            Select u

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + "" & vbCrLf)
        End Try


        Console.WriteLine("QUERY 44")
        Console.WriteLine("--------" & vbCrLf)

        Dim res44 = From u In users _
                        Where (u.FavoriteFoodOther = "Lasagne") _
                        Select u.FullName

        For Each s In res44
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 45")
        Console.WriteLine("--------" & vbCrLf)

        Dim res45 = From u In users _
                        Where (u.Age.Value >= 24) _
                        Select u.FullName

        For Each s In res45
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 46")
        Console.WriteLine("--------" & vbCrLf)

        Dim res46 = From u In users _
                        Where Not (24 <> u.Age.Value) _
                        Select u.FullName

        For Each s In res46
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 47")
        Console.WriteLine("--------" & vbCrLf)

        Dim res47 = From u In users _
                        Where u.Age.HasValue And u.Age.HasValue <> False And Not (u.Age.HasValue <> True) And Not (u.Age.HasValue = False) _
                        Select u.FullName

        For Each s In res47
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 48")
        Console.WriteLine("--------" & vbCrLf)

        Dim res48 = From u In users _
                    Where Not (u.Member.HasValue And Not u.Member.HasValue) _
                    Select u.FullName

        For Each s In res48
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 49")
        Console.WriteLine("--------" & vbCrLf)

        Dim res49 = From u In users _
                        Where Not (u.Member.Value And Not u.Member.Value) _
                        Select u.FullName

        For Each s In res49
            Console.WriteLine(s)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 50")
        Console.WriteLine("--------" & vbCrLf)

        Dim res50 = From u In users _
                        Order By u.Age.Value Descending _
                        Select u

        For Each s In res50
            Console.WriteLine(s.Age)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 51")
        Console.WriteLine("--------" & vbCrLf)

        Try
            Dim res51 = From u In users _
                            Order By u.Age.HasValue Descending _
                            Select u

            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Not OK" & vbCrLf)
            Console.ResetColor()
        Catch ex As NotSupportedException
            Console.WriteLine("OK - " + ex.Message + vbCrLf)
        End Try


        Console.WriteLine("QUERY 52")
        Console.WriteLine("--------" & vbCrLf)

        Dim res52 = From u In users Where u.FirstName Is Nothing And u.LastName IsNot Nothing Select u

        For Each u In res52
            Console.WriteLine(u.FullName)
        Next
        Console.WriteLine()


        Console.WriteLine("QUERY 53")
        Console.WriteLine("--------" & vbCrLf)

        Dim res53 = From u In users _
                        Where u.ShortBio.StartsWith("<div>Bart") _
                        Select u.FullName, u.ShortBio

        For Each u In res53
            Console.WriteLine(u)
        Next
    End Sub

    Function QueryHelper(ByRef u As Demo) As String
        Return "Dummy"
    End Function

    Function SomeBool(ByVal s As String) As Boolean
        Return False
    End Function

End Module
