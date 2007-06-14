﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BdsSoft.SharePoint.Linq;
using Microsoft.SharePoint;
using System.Reflection;

namespace Tests
{
    class SharePointListEntityTest : SharePointListEntity
    {
        public SharePointListEntityTest() : base()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj.GetType() != this.GetType())
                return false;
            else
                return true; //TODO: implement right equality check methodology
        }

        public static void Add(SPList lst, SharePointListEntityTest e)
        {
            SPListItem item = lst.Items.Add();

            foreach (PropertyInfo prop in e.GetType().GetProperties())
            {
                FieldAttribute fa = GetFieldAttribute(prop);
                if (fa != null && !fa.PrimaryKey)
                    item[fa.Field] = prop.GetValue(e, null);
            }

            item.Update();
        }

        public static SPList Create<T>(SPWeb web) where T : SharePointListEntityTest, new()
        {
            T e = new T();

            ListAttribute la = GetListAttribute(typeof(T));
            
            SPList lst;
            try
            {
                lst = web.Lists[la.List];
                if (lst != null)
                    lst.Delete();
            }
            catch { }
            
            web.Lists.Add(la.List, "", SPListTemplateType.GenericList);
            lst = web.Lists[la.List];
            lst.OnQuickLaunch = true;
            lst.Update();

            foreach (PropertyInfo prop in e.GetType().GetProperties())
            {
                FieldAttribute fa = GetFieldAttribute(prop);
                if (fa != null && !fa.PrimaryKey)
                {
                    lst.Fields.Add(fa.Field, (SPFieldType)fa.FieldType, false); //TODO: add extended information
                    lst.Views[0].ViewFields.Add(lst.Fields[fa.Field]);
                }
            }
            
            return lst;
        }

        private static ListAttribute GetListAttribute(Type t)
        {
            ListAttribute[] la = t.GetCustomAttributes(typeof(ListAttribute), false) as ListAttribute[];
            if (la != null && la.Length != 0)
                return la[0];
            else
                throw new InvalidOperationException("Missing ListAttribute on the entity type.");
        }

        private static FieldAttribute GetFieldAttribute(PropertyInfo member)
        {
            FieldAttribute[] fa = member.GetCustomAttributes(typeof(FieldAttribute), false) as FieldAttribute[];
            if (fa != null && fa.Length != 0 && fa[0] != null)
                return fa[0];
            else
                return null;
        }
    }

    class CustomList
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Field> Fields { get; set; }
    }

    class Field
    {
        public string Name { get; set; }
        public SPFieldType Type { get; set; }
        public bool Required { get; set; }
    }
}
