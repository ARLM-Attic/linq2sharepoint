''
'' LINQ-to-SharePoint
'' http://www.codeplex.com/LINQtoSharePoint
'' 
'' Copyright Bart De Smet (C) 2007
'' info@bartdesmet.net - http://blogs.bartdesmet.net/bart
'' 
'' This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
''

Imports System
Imports BdsSoft.SharePoint.Linq

''' <summary>
''' Demo
''' </summary>
<List("Demo", Id:="34c90895-fbf3-4da7-a260-4b3ddc67146d", Version:=39, Path:="/Lists/Demo")> _
Class Demo
    Inherits SharePointListEntity

    ''' <summary>
    ''' Title
    ''' </summary>
    <Field("Title", FieldType.Text, Id:="fa564e0f-0c70-4ab9-b863-0177e6ddd247")> _
    Public Property Title() As String
        Get
            Return CType(GetValue("Title"), String)
        End Get
        Set(ByVal value As String)
            SetValue("Title", value)
        End Set
    End Property

    ''' <summary>
    ''' First name
    ''' </summary>
    <Field("First name", FieldType.Text, Id:="f5164cc0-8a1b-423d-8669-e86bb65a3118")> _
    Public Property FirstName() As String
        Get
            Return CType(GetValue("FirstName"), String)
        End Get
        Set(ByVal value As String)
            SetValue("FirstName", value)
        End Set
    End Property

    ''' <summary>
    ''' Last name
    ''' </summary>
    <Field("Last name", FieldType.Text, Id:="42de3b10-2186-4db1-9345-0ec91fb8a62d")> _
    Public Property LastName() As String
        Get
            Return CType(GetValue("LastName"), String)
        End Get
        Set(ByVal value As String)
            SetValue("LastName", value)
        End Set
    End Property

    ''' <summary>
    ''' Age
    ''' </summary>
    <Field("Age", FieldType.Number, Id:="95feebc1-5719-440d-8c95-eccfc2c88f4d")> _
    Public Property Age() As Nullable(Of Double)
        Get
            Return CType(GetValue("Age"), Nullable(Of Double))
        End Get
        Set(ByVal value As Nullable(Of Double))
            SetValue("Age", value)
        End Set
    End Property

    ''' <summary>
    ''' Member
    ''' </summary>
    <Field("Member", FieldType.Boolean, Id:="32c5c166-6890-4685-acd4-c8d3ad78031c")> _
    Public Property Member() As Nullable(Of Boolean)
        Get
            Return CType(GetValue("Member"), Nullable(Of Boolean))
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            SetValue("Member", value)
        End Set
    End Property

    ''' <summary>
    ''' Member since
    ''' </summary>
    <Field("Member since", FieldType.DateTime, Id:="3320e4c8-b7d2-4a7f-b828-d995167bbecc")> _
    Public Property MemberSince() As Nullable(Of System.DateTime)
        Get
            Return CType(GetValue("MemberSince"), Nullable(Of System.DateTime))
        End Get
        Set(ByVal value As Nullable(Of System.DateTime))
            SetValue("MemberSince", value)
        End Set
    End Property

    ''' <summary>
    ''' Homepage
    ''' </summary>
    <Field("Homepage", FieldType.URL, Id:="72e5a0be-72cf-4be1-8e38-f234e25a05c5")> _
    Public Property Homepage() As Url
        Get
            Return CType(GetValue("Homepage"), Url)
        End Get
        Set(ByVal value As Url)
            SetValue("Homepage", value)
        End Set
    End Property

    ''' <summary>
    ''' Account balance
    ''' </summary>
    <Field("Account balance", FieldType.Currency, Id:="f8a1efb7-3bac-444a-a122-6245754b47da")> _
    Public Property AccountBalance() As Nullable(Of Double)
        Get
            Return CType(GetValue("AccountBalance"), Nullable(Of Double))
        End Get
        Set(ByVal value As Nullable(Of Double))
            SetValue("AccountBalance", value)
        End Set
    End Property

    ''' <summary>
    ''' Short biography
    ''' </summary>
    <Field("Short Bio", FieldType.Note, Id:="0460ce4a-1282-4ba7-8235-30cf74cb80ab")> _
    Public Property ShortBio() As String
        Get
            Return CType(GetValue("ShortBio"), String)
        End Get
        Set(ByVal value As String)
            SetValue("ShortBio", value)
        End Set
    End Property

    ''' <summary>
    ''' Membership Type
    ''' </summary>
    <Field("Membership Type", FieldType.Choice, Id:="f51ebc87-6402-4bb8-a70e-f11ee5428b43")> _
    Public Property MembershipType() As Nullable(Of MembershipType)
        Get
            Return CType(GetValue("MembershipType"), Nullable(Of MembershipType))
        End Get
        Set(ByVal value As Nullable(Of MembershipType))
            SetValue("MembershipType", value)
        End Set
    End Property

    ''' <summary>
    ''' Activities
    ''' </summary>
    <Field("Activities", FieldType.MultiChoice, Id:="c311147f-efec-4938-af6b-374b23706bf9")> _
    Public Property Activities() As Nullable(Of Activities)
        Get
            Return CType(GetValue("Activities"), Nullable(Of Activities))
        End Get
        Set(ByVal value As Nullable(Of Activities))
            SetValue("Activities", value)
        End Set
    End Property

    ''' <summary>
    ''' Favorite food
    ''' </summary>
    <Field("Favorite food", FieldType.Choice, Id:="bdf129e3-b899-4aa0-badb-6529a630a01e", OtherChoice:="FavoriteFoodOther")> _
    Public Property FavoriteFood() As Nullable(Of FavoriteFood)
        Get
            Return CType(GetValue("FavoriteFood"), Nullable(Of FavoriteFood))
        End Get
        Set(ByVal value As Nullable(Of FavoriteFood))
            SetValue("FavoriteFood", value)
        End Set
    End Property

    Private _FavoriteFoodOther As String

    ''' <summary>
    ''' Favorite food 'Fill-in' value
    ''' </summary>
    <Field("Favorite food", FieldType.Text, Id:="bdf129e3-b899-4aa0-badb-6529a630a01e")> _
    Public Property FavoriteFoodOther() As String
        Get
            Return CType(GetValue("FavoriteFoodOther"), String)
        End Get
        Set(ByVal value As String)
            SetValue("FavoriteFoodOther", value)
        End Set
    End Property

    ''' <summary>
    ''' DoubleAge
    ''' </summary>
    <Field("DoubleAge", FieldType.Number, Id:="0acee3ef-a076-44ad-9818-531d6e6b626e", ReadOnly:=True, Calculated:=True)> _
    Public ReadOnly Property DoubleAge() As Nullable(Of Double)
        Get
            Return CType(GetValue("DoubleAge"), Nullable(Of Double))
        End Get
    End Property

    ''' <summary>
    ''' Full name
    ''' </summary>
    <Field("Full name", FieldType.Text, Id:="1f062f0a-9783-4638-ba2b-2400e8a04602", ReadOnly:=True, Calculated:=True)> _
    Public ReadOnly Property FullName() As String
        Get
            Return CType(GetValue("FullName"), String)
        End Get
    End Property

    ''' <summary>
    ''' Test
    ''' </summary>
    <Field("Test", FieldType.MultiChoice, Id:="a65b9acc-2513-4f0c-8198-adf78658a13c", OtherChoice:="TestOther")> _
    Public Property Test() As Nullable(Of Test)
        Get
            Return CType(GetValue("Test"), Nullable(Of Test))
        End Get
        Set(ByVal value As Nullable(Of Test))
            SetValue("Test", value)
        End Set
    End Property

    Private _TestOther As String

    ''' <summary>
    ''' Test 'Fill-in' value
    ''' </summary>
    <Field("Test", FieldType.Text, Id:="a65b9acc-2513-4f0c-8198-adf78658a13c")> _
    Public Property TestOther() As String
        Get
            Return CType(GetValue("TestOther"), String)
        End Get
        Set(ByVal value As String)
            SetValue("TestOther", value)
        End Set
    End Property

    ''' <summary>
    ''' User's picture
    ''' </summary>
    <Field("Image", FieldType.URL, Id:="4f9eed9d-a2b4-4ea0-83c7-c6d8b0680d1c")> _
    Public Property Image() As Url
        Get
            Return CType(GetValue("Image"), Url)
        End Get
        Set(ByVal value As Url)
            SetValue("Image", value)
        End Set
    End Property

    ''' <summary>
    ''' Content Type
    ''' </summary>
    <Field("ContentType", FieldType.Text, Id:="c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly:=True)> _
    Public ReadOnly Property ContentType() As String
        Get
            Return CType(GetValue("ContentType"), String)
        End Get
    End Property

    ''' <summary>
    ''' Modified
    ''' </summary>
    <Field("Modified", FieldType.DateTime, Id:="28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly:=True)> _
    Public ReadOnly Property Modified() As Nullable(Of System.DateTime)
        Get
            Return CType(GetValue("Modified"), Nullable(Of System.DateTime))
        End Get
    End Property

    ''' <summary>
    ''' Created
    ''' </summary>
    <Field("Created", FieldType.DateTime, Id:="8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly:=True)> _
    Public ReadOnly Property Created() As Nullable(Of System.DateTime)
        Get
            Return CType(GetValue("Created"), Nullable(Of System.DateTime))
        End Get
    End Property

    ''' <summary>
    ''' Version
    ''' </summary>
    <Field("_UIVersionString", FieldType.Text, Id:="dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly:=True)> _
    Public ReadOnly Property Version() As String
        Get
            Return CType(GetValue("Version"), String)
        End Get
    End Property

End Class

Enum MembershipType As UInteger
    Platinum
    Gold
    Silver
    Bronze

End Enum

<Flags()> Enum Activities As UInteger
    Quiz = 1
    Adventure = 2
    Culture = 4

End Enum

Enum FavoriteFood As UInteger
    Pizza
    Lasagna
    Hamburger

End Enum

<Flags()> Enum Test As UInteger
    A = 1
    B = 2
    C = 4
    <Choice("Laurel & Hardy")> LaurelHardy = 8

End Enum