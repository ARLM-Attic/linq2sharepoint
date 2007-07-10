/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

using System;
using BdsSoft.SharePoint.Linq;

namespace Demo
{
    /// <summary>
    /// Demo
    /// </summary>
    [List("Demo", Id = "34c90895-fbf3-4da7-a260-4b3ddc67146d", Version = 59, Path = "/Lists/Demo")]
    class Demo : SharePointListEntity
    {
        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get { return (string)GetValue("Title"); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// First name
        /// </summary>
        [Field("First name", FieldType.Text, Id = "f5164cc0-8a1b-423d-8669-e86bb65a3118")]
        public string FirstName
        {
            get { return (string)GetValue("FirstName"); }
            set { SetValue("FirstName", value); }
        }

        /// <summary>
        /// Last name
        /// </summary>
        [Field("Last name", FieldType.Text, Id = "42de3b10-2186-4db1-9345-0ec91fb8a62d")]
        public string LastName
        {
            get { return (string)GetValue("LastName"); }
            set { SetValue("LastName", value); }
        }

        /// <summary>
        /// Age
        /// </summary>
        [Field("Age", FieldType.Number, Id = "95feebc1-5719-440d-8c95-eccfc2c88f4d")]
        public double? Age
        {
            get { return (double?)GetValue("Age"); }
            set { SetValue("Age", value); }
        }

        /// <summary>
        /// Member
        /// </summary>
        [Field("Member", FieldType.Boolean, Id = "32c5c166-6890-4685-acd4-c8d3ad78031c")]
        public bool? Member
        {
            get { return (bool?)GetValue("Member"); }
            set { SetValue("Member", value); }
        }

        /// <summary>
        /// Member since
        /// </summary>
        [Field("Member since", FieldType.DateTime, Id = "3320e4c8-b7d2-4a7f-b828-d995167bbecc")]
        public System.DateTime? MemberSince
        {
            get { return (System.DateTime?)GetValue("MemberSince"); }
            set { SetValue("MemberSince", value); }
        }

        /// <summary>
        /// Homepage
        /// </summary>
        [Field("Homepage", FieldType.URL, Id = "72e5a0be-72cf-4be1-8e38-f234e25a05c5")]
        public Url Homepage
        {
            get { return (Url)GetValue("Homepage"); }
            set { SetValue("Homepage", value); }
        }

        /// <summary>
        /// Account balance
        /// </summary>
        [Field("Account balance", FieldType.Currency, Id = "f8a1efb7-3bac-444a-a122-6245754b47da")]
        public double? AccountBalance
        {
            get { return (double?)GetValue("AccountBalance"); }
            set { SetValue("AccountBalance", value); }
        }

        /// <summary>
        /// Short biography
        /// </summary>
        [Field("Short Bio", FieldType.Note, Id = "0460ce4a-1282-4ba7-8235-30cf74cb80ab")]
        public string ShortBio
        {
            get { return (string)GetValue("ShortBio"); }
            set { SetValue("ShortBio", value); }
        }

        /// <summary>
        /// Membership Type
        /// </summary>
        [Field("Membership Type", FieldType.Choice, Id = "f51ebc87-6402-4bb8-a70e-f11ee5428b43")]
        public MembershipType? MembershipType
        {
            get { return (MembershipType?)GetValue("MembershipType"); }
            set { SetValue("MembershipType", value); }
        }

        /// <summary>
        /// Activities
        /// </summary>
        [Field("Activities", FieldType.MultiChoice, Id = "c311147f-efec-4938-af6b-374b23706bf9")]
        public Activities? Activities
        {
            get { return (Activities?)GetValue("Activities"); }
            set { SetValue("Activities", value); }
        }

        /// <summary>
        /// Favorite food
        /// </summary>
        [Field("Favorite food", FieldType.Choice, Id = "bdf129e3-b899-4aa0-badb-6529a630a01e", OtherChoice = "FavoriteFoodOther")]
        public FavoriteFood? FavoriteFood
        {
            get { return (FavoriteFood?)GetValue("FavoriteFood"); }
            set { SetValue("FavoriteFood", value); }
        }

        /// <summary>
        /// Favorite food 'Fill-in' value
        /// </summary>
        [Field("Favorite food", FieldType.Text, Id = "bdf129e3-b899-4aa0-badb-6529a630a01e")]
        public string FavoriteFoodOther
        {
            get { return (string)GetValue("FavoriteFoodOther"); }
            set { SetValue("FavoriteFoodOther", value); }
        }

        /// <summary>
        /// DoubleAge
        /// </summary>
        [Field("DoubleAge", FieldType.Number, Id = "0acee3ef-a076-44ad-9818-531d6e6b626e", ReadOnly = true, Calculated = true)]
        public double? DoubleAge
        {
            get { return (double?)GetValue("DoubleAge"); }
        }

        /// <summary>
        /// Full name
        /// </summary>
        [Field("Full name", FieldType.Text, Id = "1f062f0a-9783-4638-ba2b-2400e8a04602", ReadOnly = true, Calculated = true)]
        public string FullName
        {
            get { return (string)GetValue("FullName"); }
        }

        /// <summary>
        /// Test
        /// </summary>
        [Field("Test", FieldType.MultiChoice, Id = "a65b9acc-2513-4f0c-8198-adf78658a13c", OtherChoice = "TestOther")]
        public Test? Test
        {
            get { return (Test?)GetValue("Test"); }
            set { SetValue("Test", value); }
        }

        /// <summary>
        /// Test 'Fill-in' value
        /// </summary>
        [Field("Test", FieldType.Text, Id = "a65b9acc-2513-4f0c-8198-adf78658a13c")]
        public string TestOther
        {
            get { return (string)GetValue("TestOther"); }
            set { SetValue("TestOther", value); }
        }

        /// <summary>
        /// User's picture
        /// </summary>
        [Field("Image", FieldType.URL, Id = "4f9eed9d-a2b4-4ea0-83c7-c6d8b0680d1c")]
        public Url Image
        {
            get { return (Url)GetValue("Image"); }
            set { SetValue("Image", value); }
        }

        /// <summary>
        /// Profile
        /// </summary>
        [Field("Profile", FieldType.Lookup, Id = "74f7ac1b-5f0f-4a4a-82dc-50ed6cf89f4c", LookupDisplayField = "LinkTitleNoMenu")]
        public Users Profile
        {
            get { return (Users)GetValue("Profile"); }
            set { SetValue("Profile", value); }
        }

        /// <summary>
        /// ID
        /// </summary>
        [Field("ID", FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [Field("ContentType", FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get { return (string)GetValue("ContentType"); }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [Field("Modified", FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public System.DateTime? Modified
        {
            get { return (System.DateTime?)GetValue("Modified"); }
        }

        /// <summary>
        /// Created
        /// </summary>
        [Field("Created", FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public System.DateTime? Created
        {
            get { return (System.DateTime?)GetValue("Created"); }
        }

        /// <summary>
        /// Version
        /// </summary>
        [Field("_UIVersionString", FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get { return (string)GetValue("Version"); }
        }
    }

    /// <summary>
    /// Users
    /// </summary>
    [List("Users", Id = "e2abfbd2-f198-4fea-8a41-68eb23c8b220", Version = 12, Path = "/Lists/Users")]
    class Users : SharePointListEntity
    {
        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get { return (string)GetValue("Title"); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// First name
        /// </summary>
        [Field("First name", FieldType.Text, Id = "aa224957-7c27-4995-938e-95864c47e632")]
        public string FirstName
        {
            get { return (string)GetValue("FirstName"); }
            set { SetValue("FirstName", value); }
        }

        /// <summary>
        /// Last name
        /// </summary>
        [Field("Last name", FieldType.Text, Id = "1c3df335-b743-4ad8-993e-2319a522d1ad")]
        public string LastName
        {
            get { return (string)GetValue("LastName"); }
            set { SetValue("LastName", value); }
        }

        /// <summary>
        /// Age
        /// </summary>
        [Field("Age", FieldType.Number, Id = "80faaac7-9f73-4cdf-ba4a-d5cccfa7da71")]
        public double Age
        {
            get { return (double)GetValue("Age"); }
            set { SetValue("Age", value); }
        }

        /// <summary>
        /// Account balance
        /// </summary>
        [Field("Account balance", FieldType.Currency, Id = "d4311ef9-bf76-405f-85b2-154269c5d483")]
        public double? AccountBalance
        {
            get { return (double?)GetValue("AccountBalance"); }
            set { SetValue("AccountBalance", value); }
        }

        /// <summary>
        /// Favorite food
        /// </summary>
        [Field("Favorite food", FieldType.MultiChoice, Id = "c48610a1-098e-438c-9f77-6e65c6a392cb", OtherChoice = "FavoriteFoodOther")]
        public FavoriteFood1? FavoriteFood
        {
            get { return (FavoriteFood1?)GetValue("FavoriteFood"); }
            set { SetValue("FavoriteFood", value); }
        }

        /// <summary>
        /// Favorite food 'Fill-in' value
        /// </summary>
        [Field("Favorite food", FieldType.Text, Id = "c48610a1-098e-438c-9f77-6e65c6a392cb")]
        public string FavoriteFoodOther
        {
            get { return (string)GetValue("FavoriteFoodOther"); }
            set { SetValue("FavoriteFoodOther", value); }
        }

        /// <summary>
        /// Birthdate
        /// </summary>
        [Field("Birthdate", FieldType.DateTime, Id = "c6950879-064d-42c6-813e-f93a2c6fa06c")]
        public System.DateTime? Birthdate
        {
            get { return (System.DateTime?)GetValue("Birthdate"); }
            set { SetValue("Birthdate", value); }
        }

        /// <summary>
        /// Sportive
        /// </summary>
        [Field("Sportive", FieldType.Boolean, Id = "a2ce8327-41e1-4531-b30c-e3c797126ccd")]
        public bool? Sportive
        {
            get { return (bool?)GetValue("Sportive"); }
            set { SetValue("Sportive", value); }
        }

        /// <summary>
        /// Homepage
        /// </summary>
        [Field("Homepage", FieldType.URL, Id = "e6efe0b7-cfe9-4ec3-af79-852138133efe")]
        public Url Homepage
        {
            get { return (Url)GetValue("Homepage"); }
            set { SetValue("Homepage", value); }
        }

        /// <summary>
        /// CatYears
        /// </summary>
        [Field("CatYears", FieldType.Number, Id = "c46c4f9e-c7f2-4c09-9d5a-c021cb4fc9c0", ReadOnly = true, Calculated = true)]
        public double? CatYears
        {
            get { return (double?)GetValue("CatYears"); }
        }

        /// <summary>
        /// Full name
        /// </summary>
        [Field("Full name", FieldType.Text, Id = "6a0470eb-58c0-4f3f-92f7-9e0c3acf24b7", ReadOnly = true, Calculated = true)]
        public string FullName
        {
            get { return (string)GetValue("FullName"); }
        }

        /// <summary>
        /// ID
        /// </summary>
        [Field("ID", FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [Field("ContentType", FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get { return (string)GetValue("ContentType"); }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [Field("Modified", FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public System.DateTime? Modified
        {
            get { return (System.DateTime?)GetValue("Modified"); }
        }

        /// <summary>
        /// Created
        /// </summary>
        [Field("Created", FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public System.DateTime? Created
        {
            get { return (System.DateTime?)GetValue("Created"); }
        }

        /// <summary>
        /// Version
        /// </summary>
        [Field("_UIVersionString", FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get { return (string)GetValue("Version"); }
        }
    }

    enum MembershipType : uint { Platinum, Gold, Silver, Bronze }

    [Flags]
    enum Activities : uint { Quiz = 1, Adventure = 2, Culture = 4 }

    enum FavoriteFood : uint { Pizza, Lasagna, Hamburger }

    [Flags]
    enum Test : uint { A = 1, B = 2, C = 4, [Choice("Laurel & Hardy")] LaurelHardy = 8 }

    [Flags]
    enum FavoriteFood1 : uint { Pizza = 1, Lasagne = 2, Hamburger = 4 }
}