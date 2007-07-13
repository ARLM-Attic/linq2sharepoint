﻿/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

/*
 * Version history:
 *
 * 0.2.1 - Introduction of CamlQuery.
 *         Refactoring of PatchPredicate into separate methods.
 *         Patch for negated DateRangesOverlap expressions.
 *         Error handling with position tracking in the parser.
 *         Improvement to Lookup field subqueries; now using a foreign key concept.
 *         Refactoring of parser functionality into QueryParser.
 * 0.2.2 - New entity model; changes to property assignment and lazy loading.
 */

#region Namespace imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Web.Services.Protocols;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// CAML query class, used to represent, parse and execute CAML queries.
    /// </summary>
    internal class CamlQuery
    {
        #region Private members

        /// <summary>
        /// Factory for CAML fragments.
        /// </summary>
        internal CamlFactory _factory = new CamlFactory();

        /// <summary>
        /// Parse error collection, used when running the parser in validation mode.
        /// </summary>
        internal ParseErrorCollection _errors;

        /// <summary>
        /// Parse results.
        /// </summary>
        internal ParseResults _results;

        #region Connection

        /// <summary>
        /// SPList object for object model connections.
        /// </summary>
        private SPList _list;

        /// <summary>
        /// Web service proxy to Lists service for web service connections.
        /// </summary>
        private string _wsList;

        #endregion

        #endregion

        #region Factory methods

        /// <summary>
        /// Parses a CAML query based on an expression tree.
        /// </summary>
        /// <param name="expression">Expression tree to generate the CAML query object for.</param>
        /// <param name="validate">Used to turn on parse validation.</param>
        /// <returns>CAML query object for the specified expression tree.</returns>
        public static CamlQuery Parse(Expression expression, bool validate)
        {
            //
            // Create a query object and enable validation (optional).
            //
            CamlQuery query = new CamlQuery();

            //
            // Do the real parsing.
            //
            QueryParser parser = new QueryParser(expression, validate);
            parser._factory = query._factory;

            query._results = parser.Parse();
            query._errors = parser._errors;

            //
            // Return constructed query object.
            //
            return query;
        }

        #endregion

        #region Query execution and enumeration

        /// <summary>
        /// Triggers the query and fetches results.
        /// </summary>
        /// <returns>Query results.</returns>
        /// <typeparam name="T">Type of the result objects.</typeparam>
        public IEnumerator<T> Execute<T>()
        {
            //
            // Get list information.
            //
            ListAttribute la = Helpers.GetListAttribute(_results.EntityType, true);
            if (_results.Context._site != null)
                _list = _results.Context._site.RootWeb.GetList(la.Path);
            else
                _wsList = la.List;

            //
            // Perform version check; the _CheckListVersion method will figure out whether or not such a check is required.
            //
            _CheckListVersion();

            //
            // We don't want the default view, so we'll make an exhaustive list of all the properties to retrieve.
            //
            if (_results.Projection == null)
            {
                _results.Projection = _factory.ViewFields();

                HashSet<string> fields = new HashSet<string>();
                foreach (PropertyInfo property in _results.EntityType.GetProperties())
                {
                    if (Helpers.GetFieldAttribute(property) != null)
                    {
                        //
                        // Get the field and field name corresponding to the current entity property.
                        //
                        XmlElement field = GetFieldRef(property);
                        string fieldName = field.Attributes["Name"].Value;

                        //
                        // Filter for duplicates that can occur because of multi-choice values with fill-in choices.
                        //
                        if (field != null && !fields.Contains(fieldName))
                        {
                            fields.Add(fieldName);
                            _results.Projection.AppendChild(field);
                        }
                    }
                }
            }

            //
            // Patch the query for possible Lookup field references.
            //
            bool? optimized = PatchQueryPredicate();
            if (optimized != null)
            {
                //
                // Predicate evaluates to true. Remove the query predicate.
                //
                if (optimized.Value)
                    _results.Where = null;
                //
                // Predicate evaluates to false. No results will be fetched.
                //
                else
                    return VoidResult<T>();
            }

            //
            // Perform query via the SharePoint Object Model or via SharePoint web services.
            //
            if (_results.Context._site != null)
                return GetEnumeratorSp<T>();
            else
                return GetEnumeratorWs<T>();
        }

        /// <summary>
        /// Helper method to return a void result if the query predicate evaluates to a constant false value.
        /// </summary>
        /// <returns>Empty sequence.</returns>
        private IEnumerator<T> VoidResult<T>()
        {
            yield break;
        }

        #region Enumeration helpers

        /// <summary>
        /// Performs a list version check to make sure that the exported list definition matches the online list version.
        /// </summary>
        private void _CheckListVersion()
        {
            bool check;
            int version;

            //
            // Find the list attribute, which is required to perform a list version match check.
            //
            ListAttribute la = Helpers.GetListAttribute(_results.EntityType, true);

            //
            // Check on context level.
            //
            bool? checkContext = _results.Context.CheckListVersion;
            if (checkContext == null)
            {
                object list = _results.Context.GetList(_results.EntityType);

                //
                // Check on list level.
                //
                bool? checkList = (bool?)list.GetType().GetProperty("CheckVersion").GetValue(list, null);
                if (checkList == null)
                    check = la.CheckVersion;
                else
                    check = checkList.Value;
            }
            else
                check = checkContext.Value;

            //
            // Should check?
            //
            if (!check)
                return;

            //
            // Check version via the SharePoint Object Model or via SharePoint web services.
            //
            if (_results.Context._site != null)
                version = _list.Version;
            else
            {
                XmlNode lst = _results.Context._wsProxy.GetList(_wsList);
                version = int.Parse(lst.Attributes["Version"].Value, CultureInfo.InvariantCulture.NumberFormat);
            }

            //
            // Check the version.
            //
            if (la.Version != version)
                throw RuntimeErrors.ListVersionMismatch();
        }

        /// <summary>
        /// Patches the query predicate to eliminate Lookup field references by subqueries.
        /// </summary>
        /// <returns>Null if the query predicate wasn't optimized away; a Boolean value with the constant query predicate value if the whole query was optimized to one single constant.</returns>
        private bool? PatchQueryPredicate()
        {
            //
            // Patch Lookup field references.
            //
            Patch(_results.Where);

            //
            // Eliminate Boolean patches by tree pruning.
            //
            return Prune(_results.Where);
        }

        /// <summary>
        /// Patches the query predicate represented by the given node to eliminate Lookup field references by subqueries.
        /// </summary>
        /// <param name="node">Query predicate node to be patched.</param>
        private void Patch(XmlNode node)
        {
            List<Patch> patches = new List<Patch>();
            GetPatches(node, ref patches);

            foreach (Patch p in patches)
                p.Parent.ReplaceChild(p.NewChild, p.OldChild);
        }

        private void GetPatches(XmlNode node, ref List<Patch> patches)
        {
            //
            // Any work to do?
            //
            if (node == null)
                return;

            foreach (XmlNode child in node.ChildNodes)
            {
                XmlElement e = child as XmlElement;
                if (e != null)
                {
                    //
                    // Find <Patch> tags.
                    //
                    if (e.Name == "Patch")
                    {
                        //
                        // Get information about the Lookup field.
                        //
                        string field = e.Attributes["Field"].Value;
                        PropertyInfo lookupField = _results.EntityType.GetProperty(field);
                        FieldAttribute lookup = Helpers.GetFieldAttribute(lookupField);
                        if (lookup.FieldType != FieldType.Lookup)
                            throw RuntimeErrors.LookupFieldPatchError();

                        //
                        // Get information about the Lookup list.
                        //
                        ListAttribute innerListAttribute = Helpers.GetListAttribute(lookupField.PropertyType, true);
                        string innerList = innerListAttribute.List;

                        //
                        // Get the CAML from the patch.
                        //
                        string caml = e.InnerXml;

                        //
                        // Only retrieve the lookup column value (display column).
                        // 0.2.1 - only the ID is required
                        //
                        XmlElement viewFields = _factory.ViewFields();
                        viewFields.AppendChild(_factory.FieldRef("ID"));

                        //
                        // Prepare list of subquery results.
                        //
                        List<int> ids = new List<int>();

                        //
                        // Use SharePoint object model.
                        //
                        if (_results.Context._site != null)
                        {
                            //
                            // Build the query.
                            //
                            SPQuery query = new SPQuery();
                            XmlElement q = _factory.CreateElement("Where");
                            q.InnerXml = caml;
                            query.Query = q.OuterXml;
                            query.ViewFields = viewFields.InnerXml;
                            query.IncludeMandatoryColumns = false;
                            query.IncludePermissions = false;
                            query.IncludeAttachmentVersion = false;
                            query.IncludeAttachmentUrls = false;
                            query.IncludeAllUserPermissions = false;

                            //
                            // Log it.
                            //
                            DoLogging(innerList, q, null, viewFields);

                            //
                            // Get subquery results.
                            //
                            foreach (SPListItem item in _results.Context._site.RootWeb.Lists[innerList].GetItems(query))
                            {
                                //fks.Add(item[lookup.LookupField]);
                                ids.Add((int)item["ID"]);
                            }
                        }
                        //
                        // Use SharePoint web services.
                        //
                        else
                        {
                            //
                            // Build the query.
                            //
                            XmlElement query = _factory.CreateElement("Query");
                            XmlElement where = _factory.CreateElement("Where");
                            where.InnerXml = caml;
                            query.AppendChild(where);

                            XmlNode queryOptions = _factory.CreateElement("QueryOptions");

                            XmlNode includeMandatoryColumns = _factory.CreateElement("IncludeMandatoryColumns");
                            includeMandatoryColumns.InnerText = "FALSE";
                            queryOptions.AppendChild(includeMandatoryColumns);

                            //
                            // Log it.
                            //
                            DoLogging(innerList, where, null, viewFields);

                            //
                            // Get results.
                            //
                            XmlNode res = _results.Context._wsProxy.GetListItems(innerList, null, query, viewFields, null, queryOptions, null);
                            DataSet ds = new DataSet();
                            ds.Locale = CultureInfo.InvariantCulture;
                            ds.ReadXml(new StringReader(res.OuterXml));
                            DataTable tbl = ds.Tables["row"];

                            //
                            // Get subquery results.
                            //
                            foreach (DataRow row in tbl.Rows)
                            {
                                //fks.Add(row["ows_" + lookup.LookupField]);
                                ids.Add(int.Parse((string)row["ows_ID"], CultureInfo.InvariantCulture.NumberFormat));
                            }
                        }

                        //
                        // Create patch.
                        //
                        XmlElement patch = null;
                        //foreach (object o in fks)
                        foreach (int id in ids)
                        {
                            //XmlElement val = GetValue(o, lookup, 0, 0);
                            //patch = CreatePatch(lookupField, val, patch);
                            patch = CreatePatch(lookupField, id, patch);
                        }

                        //
                        // Apply patch. If no Lookup field reference patch is found, a Boolean false-valued patch will be inserted to allow for subsequent pruning.
                        //
                        if (patch != null) //FIX
                            patches.Add(new Patch() { Parent = e.ParentNode, NewChild = patch, OldChild = e });
                        else
                            patches.Add(new Patch() { Parent = e.ParentNode, NewChild = _factory.BooleanPatch(false), OldChild = e });
                    }
                    else
                        GetPatches(e, ref patches);
                }
            }
        }

        /// <summary>
        /// Prunes Boolean patches from the predicate tree.
        /// </summary>
        /// <param name="node">Predicate tree to be pruned.</param>
        /// <returns></returns>
        private bool? Prune(XmlNode node)
        {
            //
            // Any work to do?
            //
            if (node == null)
                return null;

            XmlElement e = node as XmlElement;
            if (e != null)
            {
                bool? b1, b2;

                //
                // Find binary nodes and Boolean patch tags.
                //
                switch (e.Name)
                {
                    case "Where":
                        if (e.ChildNodes.Count == 1)
                        {
                            b1 = Prune(e.ChildNodes[0]);
                            if (b1 == null)
                                return null;
                            else
                                return b1.Value;
                        }
                        else
                            return true;
                    case "And":
                        b1 = Prune(e.ChildNodes[0]);
                        b2 = Prune(e.ChildNodes[1]);

                        if (b1 != null && b2 != null)
                            return b1.Value && b2.Value;
                        else if (b1 != null)
                        {
                            //
                            // (false && x) == false
                            //
                            if (!b1.Value)
                                return false;
                            //
                            // (true && x) == x
                            //
                            else
                                e.ParentNode.ReplaceChild(e.ChildNodes[1], e);
                        }
                        else if (b2 != null)
                        {
                            //
                            // (x && false) == false
                            //
                            if (!b2.Value)
                                return false;
                            //
                            // (x && true) == x
                            //
                            else
                                e.ParentNode.ReplaceChild(e.ChildNodes[0], e);
                        }
                        break;
                    case "Or":
                        b1 = Prune(e.ChildNodes[0]);
                        b2 = Prune(e.ChildNodes[1]);

                        if (b1 != null && b2 != null)
                            return b1.Value || b2.Value;
                        else if (b1 != null)
                        {
                            //
                            // (true || x) == true
                            //
                            if (b1.Value)
                                return true;
                            //
                            // (false || x) == x
                            //
                            else
                                e.ParentNode.ReplaceChild(e.ChildNodes[1], e);
                        }
                        else if (b2 != null)
                        {
                            //
                            // (x || true) == true
                            //
                            if (b2.Value)
                                return true;
                            //
                            // (x || false) == x
                            //
                            else
                                e.ParentNode.ReplaceChild(e.ChildNodes[0], e);
                        }
                        break;
                    case "TRUE":
                        return true;
                    case "FALSE":
                        return false;
                }
            }

            return null;
        }

        /// <summary>
        /// Helper method to create a Lookup field patch by building a tree of Or CAML elements.
        /// </summary>
        /// <param name="field">Entity property to construct the Lookup patch for.</param>
        /// <param name="value">Value for the Lookup condition.</param>
        /// <param name="parent">Current tree of the Lookup patch to add the new condition node to. Should be null to start creating a condition tree.</param>
        /// <returns></returns>
        /// <example>
        /// If parent == null:
        /// <![CDATA[
        /// <condition>
        ///    value
        ///    <FieldRef Name="field" />
        /// </condition>
        /// ]]>
        /// 
        /// If parent != null:
        /// <![CDATA[
        /// <Or>
        ///    <condition>
        ///       value
        ///       <FieldRef Name="field" />
        ///    </condition>
        ///    parent
        /// </Or>
        /// ]]>
        /// </example>
        private XmlElement CreatePatch(PropertyInfo field, int value, XmlElement parent)
        {
            //
            // Create condition element with the child tree and the FieldRef element.
            //
            XmlElement cond = _factory.Eq();

            XmlElement val = _factory.Value("Lookup");
            val.InnerText = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
            cond.AppendChild(val);

            XmlElement fieldRef = GetFieldRef(field);
            fieldRef.Attributes.Append(_factory.LookupAttribute());
            cond.AppendChild(fieldRef);

            //
            // If no parent is present yet, we'll just return the condition element.
            //
            if (parent == null)
                return cond;
            //
            // If we're deeper in the tree, we'll take the current parent and lift it to a new Or element together with the newly created condition.
            //
            else
                return _factory.Or(cond, parent);
        }

        /// <summary>
        /// Helper method to execute a query and fetch results using the SharePoint Object Model.
        /// </summary>
        /// <returns>Query results.</returns>
        private IEnumerator<T> GetEnumeratorSp<T>()
        {
            //
            // Construct the query ready for consumption by the SharePoint Object Model, without <Query> root element.
            //
            StringBuilder queryBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            using (XmlWriter xw = XmlWriter.Create(queryBuilder, settings))
            {
                if (this._results.Where != null && this._results.Where.ChildNodes.Count != 0)
                    this._results.Where.WriteTo(xw);
                if (_results.Order != null)
                    _results.Order.WriteTo(xw);
                xw.Flush();
            }

            //
            // Make the SharePoint SPQuery object.
            //
            SPQuery query = new SPQuery();
            query.Query = queryBuilder.ToString();
            query.IncludeMandatoryColumns = false;

            //
            // Include projection fields if a projection clause has been parsed.
            //
            if (_results.Projection != null)
            {
                query.ViewFields = _results.Projection.InnerXml;
            }

            //
            // In case a row limit (Take) was found, set the limit on the query.
            //
            if (_results.Top != null)
            {
                uint top = (uint)_results.Top.Value;
                if (top == 0) //FIX: query.RowLimit = 0 seems to be ineffective.
                    yield break;
                else
                    query.RowLimit = top;
            }

            //
            // Perform logging of the gathered information.
            //
            DoLogging(_list.Title, _results.Where, _results.Order, _results.Projection);

            //
            // Execute the query via the SPList object.
            //
            SPListItemCollection items;
            try
            {
                items = _list.GetItems(query);
            }
            catch (Exception ex)
            {
                throw RuntimeErrors.ConnectionExceptionSp(_results.Context._site.Url, ex);
            }

            //
            // Still an entity?
            //
            object lst = null;
            MethodInfo fromCache = null;
            MethodInfo toCache = null;
            GetEntityAccessors<T>(out lst, out fromCache, out toCache);

            //
            // Fetch results using an iterator.
            //
            foreach (SPListItem item in items)
            {
                yield return GetItem<T>(item, null, lst, fromCache, toCache);
            }
        }

        /// <summary>
        /// Helper method to execute a query and fetch results using SharePoint web services.
        /// </summary>
        /// <returns>Query results.</returns>
        private IEnumerator<T> GetEnumeratorWs<T>()
        {
            //
            // Construct the query ready for consumption by the SharePoint web services, including a <Query> root element.
            //
            XmlNode query = _factory.CreateElement("Query");
            if (this._results.Where != null && this._results.Where.ChildNodes.Count != 0)
                query.AppendChild(this._results.Where);
            if (_results.Order != null)
                query.AppendChild(_results.Order);

            //
            // Set query options.
            //
            XmlNode queryOptions = _factory.CreateElement("QueryOptions");

            //
            // Perform logging of the gathered information.
            //
            DoLogging(_wsList, _results.Where, _results.Order, _results.Projection);

            //
            // Retrieve the results of the query via a web service call, using the projection and a row limit (if set).
            //
            uint top = 0;
            if (_results.Top != null)
            {
                top = (uint)_results.Top.Value;
                if (top == 0) //FIX: don't roundtrip to server if no results are requested.
                    yield break;
            }

            XmlNode res;
            try
            {
                res = _results.Context._wsProxy.GetListItems(_wsList, null, query, _results.Projection, _results.Top == null ? null : top.ToString(), queryOptions, null);
            }
            catch (SoapException ex)
            {
                throw RuntimeErrors.ConnectionExceptionWs(_results.Context._wsProxy.Url, ex);
            }

            //
            // Store results in a DataSet for easy iteration.
            // TODO: the DataSet approach could be replaced by raw XML parsing.
            //
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(res.OuterXml));
            DataTable tbl = ds.Tables["row"];

            //
            // Make sure results are available. If not, return nothing.
            //
            if (tbl == null)
                yield break;

            //
            // Still an entity?
            //
            object lst = null;
            MethodInfo fromCache = null;
            MethodInfo toCache = null;
            GetEntityAccessors<T>(out lst, out fromCache, out toCache);

            //
            // Fetch results using an iterator.
            //
            foreach (DataRow row in tbl.Rows)
            {
                yield return GetItem<T>(null, row, lst, fromCache, toCache);
            }
        }

        /// <summary>
        /// Retrieves the entity accessors on the list type. These allow retrieval from and storage to the entity cache that's stored with the list.
        /// </summary>
        /// <typeparam name="T">Type of the entity to retrieve accessors for.</typeparam>
        /// <param name="lst">List object corresponding to the entity type.</param>
        /// <param name="fromCache">Cache retrieval accessor.</param>
        /// <param name="toCache">Cache storage accessor.</param>
        private void GetEntityAccessors<T>(out object lst, out MethodInfo fromCache, out MethodInfo toCache)
        {
            lst = null;
            fromCache = null;
            toCache = null;

            //
            // Get accessors only when the resulting objects are of the entity type.
            // Otherwise, the temporary entity objects will be incomplete due to projections and are therefore not suitable for storage in the cache.
            //
            if (typeof(T) == _results.EntityType)
            {
                lst = _results.Context.GetList(_results.EntityType);
                Type listType = lst.GetType();
                fromCache = listType.GetMethod("FromCache", BindingFlags.NonPublic | BindingFlags.Instance);
                toCache = listType.GetMethod("ToCache", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            Debug.Assert(
                   (lst == null && fromCache == null && toCache == null)
                || (lst != null && fromCache != null && toCache != null)
            );
        }

        /// <summary>
        /// Helper method to log query information before fetching results.
        /// </summary>
        /// <param name="list">List that's being queried.</param>
        /// <param name="where">Query predicate.</param>
        /// <param name="order">Ordering clause.</param>
        /// <param name="projection">Projection clause.</param>
        private void DoLogging(string list, XmlElement where, XmlElement order, XmlElement projection)
        {
            //
            // Check whether logging is enabled or not.
            //
            if (_results.Context.Log != null)
            {
                //
                // List general info.
                //
                if (_results.Context._site != null)
                    _results.Context.Log.WriteLine("Query for " + list + " through object model...");
                else
                    _results.Context.Log.WriteLine("Query for " + list + " through web services...");

                //
                // Do the remainder of the logging (CAML).
                //
                Helpers.LogTo(_results.Context.Log, where, order, projection);

                //
                // Spacing to distinguish between subsequent queries.
                //
                _results.Context.Log.WriteLine();
                _results.Context.Log.Flush();
            }
        }

        /// <summary>
        /// Constructs a query result object based on the given item that was retrieved either via the SharePoint object model or via the SharePoint list web service.
        /// </summary>
        /// <param name="item">Item retrieved via the SharePoint object model.</param>
        /// <param name="row">Item retrieved via the SharePoint list web service.</param>
        /// <param name="lst">List that will hold the entity (if result still is an entity).</param>
        /// <param name="fromCache">Accessor method to get entity from list entity cache.</param>
        /// <param name="toCache">Accessor method to store entity in list entity cache.</param>
        /// <returns>Query result object for the query, reflecting the final result (possibly after projection).</returns>
        /// <remarks>Either item or row should be null.</remarks>
        private T GetItem<T>(SPListItem item, DataRow row, object lst, MethodInfo fromCache, MethodInfo toCache)
        {
            Debug.Assert(item != null || row != null);

            bool isEntity = lst != null;
            int? id = null;

            //
            // Still an entity?
            //
            if (isEntity)
            {
                //
                // Get the id.
                //
                if (item != null)
                    id = (int)item["ID"];
                else
                    id = int.Parse((string)row["ows_ID"], CultureInfo.InvariantCulture.NumberFormat);

                //
                // Already in list?
                //
                object o = fromCache.Invoke(lst, new object[] { id.Value });
                if (o != null)
                    return (T)o;
            }

            //
            // Create an instance of the entity type. This instance will be used to perform the projection operation on (if any).
            //
            object result = Activator.CreateInstance(_results.EntityType);

            //
            // Get the collection of properties that have to be set on the entity.
            // Only in case the result type is still an entity, we'll set all properties; otherwise, we'll set the properties from the projection.
            //
            IEnumerable<PropertyInfo> props = (!isEntity ? (IEnumerable<PropertyInfo>)_results.ProjectionProperties : typeof(T).GetProperties());

            //
            // Data comes from the SharePoint Object Model.
            //
            if (item != null)
            {
                foreach (PropertyInfo p in props)
                    AssignResultProperty<T>(item, null, p, result);
            }
            //
            // Data comes from the SharePoint list web service.
            //
            else
            {
                foreach (PropertyInfo p in props)
                    AssignResultProperty<T>(null, row, p, result);
            }

            //
            // Perform projection if required.
            //
            if (isEntity)
            {
                T res = (T)result;
                toCache.Invoke(lst, new object[] { id.Value, res });
                return res;
            }
            else
                return (T)_results.Project.DynamicInvoke(result);
        }

        /// <summary>
        /// Assigns a value from the query result to a given property of the entity object.
        /// </summary>
        /// <param name="item">Query result item retrieved using the SharePoint object model.</param>
        /// <param name="row">Query result item retrieved using the SharePoint lists web service.</param>
        /// <param name="property">Property to set on the entity object.</param>
        /// <param name="target">Entity object to set the property for.</param>
        private void AssignResultProperty<T>(SPListItem item, DataRow row, PropertyInfo property, object target)
        {
            //
            // Get the field mapping attribute for the given property.
            //
            FieldAttribute field = Helpers.GetFieldAttribute(property);
            if (field == null)
                throw RuntimeErrors.MissingFieldMappingAttribute(property.Name);

            //
            // Get the value of the property either using the SharePoint list object or using the current DataRow.
            //
            object val;
            string valueAsString;
            if (item != null)
                //
                // Results have a field name which is XML encoded.
                //
                val = item[XmlConvert.EncodeName(field.Field)];
            else
            {
                //
                // Results from the web service have columns prefixed by ows_.
                //
                string col = "ows_" + field.Field;

                //
                // If no results were found with a specific column, it won't be present in the DataRow. We can ignore this property then.
                //
                if (!row.Table.Columns.Contains(col))
                    return;
                val = row[col];
            }

            //
            // If no value has been set, ignore this property.
            //
            if (val == null || val is DBNull)
                return;

            //
            // Get the property type in order to do subsequent value parsing. If the type is Nullable<X>, return typeof(X).
            //
            Type propertyType = property.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                propertyType = Nullable.GetUnderlyingType(propertyType);

            //
            // Enums require special treatment in order to set flags and/or fill-in choices.
            //
            if (propertyType.IsSubclassOf(typeof(Enum)))
            {
                val = AssignResultPropertyAsEnum(property, target, field, val, propertyType);
            }
            //
            // If the value is of type string, we'll do additional parsing to assign the value to the property.
            //
            else if ((valueAsString = val as string) != null)
            {
                //
                // Calculated fields are prefixed by a type indicator, followed by a ;# separator. We'll trim this off and keep the remainder which represents the underlying value.
                //
                if (field.Calculated)
                    valueAsString = valueAsString.Substring(valueAsString.IndexOf(";#", StringComparison.Ordinal) + 2);

                //
                // Parse the value based on the type set on the field mapping attribute.
                //
                switch (field.FieldType)
                {
                    //
                    // Booleans are represented as 0, 1 or some string representation compatible with System.Boolean.Parse.
                    //
                    case FieldType.Boolean:
                        bool bb = (valueAsString == "1" ? true : (valueAsString == "0" ? false : bool.Parse(valueAsString)));
                        AssignValue(target, property, field, bb);
                        break;
                    //
                    // DateTime values can be parsed using System.DateTime.Parse.
                    //
                    case FieldType.DateTime:
                        DateTime dt = DateTime.ParseExact(valueAsString, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-us")); //FIX hh to HH (check!)
                        AssignValue(target, property, field, dt);
                        break;
                    //
                    // Counter field represents an integer and is used in primary key values. (v0.2.0.0)
                    //
                    case FieldType.Counter:
                        int pk = int.Parse(valueAsString, new CultureInfo("en-us"));
                        AssignValue(target, property, field, pk);
                        break;
                    //
                    // Number and Currency values are represented as floats that can be parsed using System.Double.Parse.
                    //
                    case FieldType.Number:
                    case FieldType.Currency:
                        double dd = double.Parse(valueAsString, new CultureInfo("en-us"));
                        AssignValue(target, property, field, dd);
                        break;
                    //
                    // Integer values are 32-bit signed numbers that can be parsed using System.Int32.Parse.
                    //
                    case FieldType.Integer:
                        int ii = int.Parse(valueAsString, new CultureInfo("en-us"));
                        AssignValue(target, property, field, ii);
                        break;
                    //
                    // For URL values, a custom Url class has been defined that knows how to parse a SharePoint URL value to a Uri and a friendly name.
                    //
                    case FieldType.URL:
                        Url url = Url.Parse(valueAsString);
                        AssignValue(target, property, field, url);
                        break;
                    //
                    // Text and Note values are plain simple strings.
                    //
                    case FieldType.Text:
                    case FieldType.Note:
                        AssignValue(target, property, field, valueAsString);
                        break;
                    //
                    // Lookup fields represent n-to-1 mappings and require lazy loading of the referenced list entity.
                    //
                    case FieldType.Lookup:
                        //
                        // Structure will be key;#display where key represents the foreign key and display the display field.
                        //
                        string[] fk = valueAsString.Split(new string[] { ";#" }, StringSplitOptions.None);
                        if (fk.Length != 2)
                            break;
                        int fkey = int.Parse(fk[0], CultureInfo.InvariantCulture.NumberFormat);

                        //
                        // We'll only support lazy loading on entity types.
                        //
                        //if (entity == null)
                        if (Helpers.GetListAttribute(target.GetType(), false) == null)
                            throw RuntimeErrors.InvalidLookupField(property.Name);
                        else
                        {
                            //
                            // Create EntityRef<T>.
                            //
                            Type t = typeof(EntityRef<>).MakeGenericType(property.PropertyType);
                            object entityRef = Activator.CreateInstance(t, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { _results.Context, fkey }, null);
                            AssignValue(target, property, field, entityRef);

                            //
                            // Load if deferred loading is disabled.
                            //
                            if (!_results.Context.DeferredLoadingEnabled)
                                t.GetMethod("Load", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(entityRef, new object[] { });
                        }
                        break;
                    //
                    // LookupMulti fields represent n-to-m mappings and require lazy loading of the referenced list entities.
                    //
                    case FieldType.LookupMulti:
                        //
                        // Structure will be [key;#display]* where key represents the foreign key and display the display field.
                        //
                        string[] fks = valueAsString.Split(new string[] { ";#" }, StringSplitOptions.None);
                        if (fks.Length % 2 != 0)
                            break;
                        List<int> lstFkeys = new List<int>();
                        for (int i = 0; i < fks.Length; i += 2)
                            lstFkeys.Add(int.Parse(fks[i], CultureInfo.InvariantCulture.NumberFormat));
                        int[] fkeys = lstFkeys.ToArray();

                        //
                        // We'll only support lazy loading on entity types.
                        //
                        //if (entity == null)
                        if (Helpers.GetListAttribute(target.GetType(), false) == null)
                            throw RuntimeErrors.InvalidLookupField(property.Name);
                        else
                        {
                            //
                            // Create EntitySef<T>. TODO: hook up event handlers for Add and Remove actions.
                            //
                            Type t = typeof(EntitySet<>).MakeGenericType(property.PropertyType);
                            object entitySet = Activator.CreateInstance(t, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { _results.Context, fkeys, null, null }, null);
                            AssignValue(target, property, field, entitySet);

                            //
                            // Load if deferred loading is disabled.
                            //
                            if (!_results.Context.DeferredLoadingEnabled)
                                t.GetMethod("Load", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(entitySet, new object[] { });
                        }
                        break;
                    default:
                        throw RuntimeErrors.UnrecognizedMappingType(field.FieldType.ToString());
                }
            }
            //
            // If the type is not an enum or a string, we can assume it has the right type for direct assignment to the property.
            //
            else
            {
                //if (entity == null)
                //    property.SetValue(target, val, null);
                //else
                //    entity.SetValue(property.Name, val);
                AssignValue(target, property, field, val);
            }
        }

        /// <summary>
        /// Helper method to assign a value to a target object's specified property.
        /// </summary>
        /// <param name="target">Target object to assign a property value to.</param>
        /// <param name="property">Property on the target object to assign a value to.</param>
        /// <param name="field">Field corresponding with the property to be set.</param>
        /// <param name="value">Value to assign to the specified property on the specified object.</param>
        private static void AssignValue(object target, PropertyInfo property, FieldAttribute field, object value)
        {
            Debug.Assert(target != null);
            Debug.Assert(property != null);
            Debug.Assert(field != null);

            //
            // Read-only fields require 
            //
            if (field.ReadOnly && field.Storage == null)
                throw RuntimeErrors.StoragePropertyMissingOnReadOnlyField(property.Name);

            //
            // Can bypass?
            //
            if (field.Storage != null)
            {
                FieldInfo fi = target.GetType().GetField(field.Storage, BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi == null)
                    throw RuntimeErrors.InvalidStoragePropertyFieldReference(property.Name);

                fi.SetValue(target, value);
            }
            //
            // Go through property.
            //
            else
            {
                if (!property.CanWrite)
                    throw RuntimeErrors.NonReadOnlyFieldWithoutSetter(property.Name);

                property.SetValue(target, value, null);
            }
        }

        /// <summary>
        /// Helper method to assign an enum property on a result object.
        /// </summary>
        /// <param name="property">Property to assign a value to.</param>
        /// <param name="target">Target object to assign a property value to.</param>
        /// <param name="field">Field attribute of the entity type field to assign to. Used to determine the "other choice" field.</param>
        /// <param name="val">Value to be assigned as an enum with possible "other choice".</param>
        /// <param name="propertyType">Type of the underlying enumeration.</param>
        /// <returns>Value of the enum corresponding to the val parameter.</returns>
        private static object AssignResultPropertyAsEnum(PropertyInfo property, object target, FieldAttribute field, object val, Type propertyType)
        {
            //
            // Find all of the choices of the enum type. A reverse mapping from SharePoint CHOICE names to enum field names is maintained, which will be used to allow Enum.Parse calls further on.
            //
            HashSet<string> choices = new HashSet<string>();
            Dictionary<string, string> reverseMapping = new Dictionary<string, string>();
            foreach (string f in Enum.GetNames(propertyType))
            {
                //
                // Custom mapping of enum field to CHOICE value?
                //
                ChoiceAttribute[] ca = propertyType.GetField(f).GetCustomAttributes(typeof(ChoiceAttribute), false) as ChoiceAttribute[];
                if (ca != null && ca.Length != 0 && ca[0] != null)
                {
                    choices.Add(ca[0].Choice);

                    //
                    // Maintain the reverse mapping from the SharePoint CHOICE name to the enum field name.
                    //
                    reverseMapping.Add(ca[0].Choice, f);
                }
                else
                    choices.Add(f);
            }

            //
            // The value can be converted to a string in case of (Multi)Choice results. Each choice value is separated by ;#, so we'll split the set of choices.
            // From this set of individual choices, we can filter out the known values, which will leave us with a possible fill-in choice.
            //
            string[] vs = ((string)val).Replace(";#", ",").Trim(',', ' ').Split(',');
            HashSet<string> knownVals = new HashSet<string>(vs);
            knownVals.IntersectWith(choices);

            //
            // In order to get the value that needs to be assigned to the property we'll take all of the known values and map them back to enum field names.
            // The resulting comma-separated string with known choices can be parsed using Enum.Parse to get the final result.
            //
            StringBuilder sb = new StringBuilder();
            foreach (string s in knownVals)
                sb.AppendFormat("{0}, ", reverseMapping.ContainsKey(s) ? reverseMapping[s] : s);

            string v = sb.ToString().TrimEnd(',', ' ');
            if (v.Length != 0)
            {
                val = Enum.Parse(propertyType, v);
                AssignValue(target, property, field, val);
            }

            //
            // Now a set of remaining values is constructed by taking the original set of values and removing all known values.
            //
            HashSet<string> otherVals = new HashSet<string>(vs);
            foreach (string s in knownVals)
                otherVals.Remove(s);

            //
            // We expect zero or one other value. In the latter case, this will serve as the input for the "other field".
            //
            string other = null;
            if (otherVals.Count == 1)
                other = otherVals.ToArray()[0];
            else if (otherVals.Count > 1)
                throw RuntimeErrors.TooManyUnknownChoiceValues(property.Name);

            //
            // If an other value is found, process it as a fill-in choice for the MultiChoice field.
            //
            if (other != null)
            {
                //
                // Find the field used for the fill-in choice.
                //
                string otherField = field.OtherChoice;
                if (otherField != null)
                {
                    PropertyInfo pOther = property.DeclaringType.GetProperty(otherField);
                    //
                    // Assign the fill-in choice value to the OtherChoice field.
                    //
                    if (pOther != null)
                    {
                        FieldAttribute otherFieldAttr = Helpers.GetFieldAttribute(pOther);
                        if (otherFieldAttr == null)
                            throw RuntimeErrors.MissingFieldMappingAttribute(pOther.Name);

                        AssignValue(target, pOther, otherFieldAttr, other);
                    }
                    else
                        throw RuntimeErrors.InvalidOtherChoiceFieldMapping(property.Name);
                }
                else
                    throw RuntimeErrors.MissingOtherChoiceFieldMapping(property.Name);
            }
            return val;
        }

        #endregion

        #endregion

        #region Helper methods for FieldRef element creation

        /// <summary>
        /// Gets a CAML FieldRef element for the specified entity property.
        /// </summary>
        /// <param name="property">Entity property to get the FieldRef element for.</param>
        /// <returns>FieldRef element for the specified entity property.</returns>
        private XmlElement GetFieldRef(PropertyInfo property)
        {
            //
            // Get mapping attribute and make sure it has been set.
            //
            FieldAttribute fld = Helpers.GetFieldAttribute(property);
            if (fld == null)
                throw RuntimeErrors.MissingFieldMappingAttribute(property.Name);

            return _factory.FieldRef(fld.Field);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the CAML query represented as a string.
        /// </summary>
        /// <returns>String representation of the query.</returns>
        public override string ToString()
        {
            StringBuilder caml = new StringBuilder();
            StringWriter sw = new StringWriter(caml, CultureInfo.InvariantCulture);
            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.Formatting = Formatting.Indented;

            Helpers.LogTo(sw, _results.Where, _results.Order, _results.Projection);
            return caml.ToString();
        }

        #endregion
    }

    /// <summary>
    /// Internal helper class for patches in a CAML query.
    /// </summary>
    internal class Patch
    {
        /// <summary>
        /// Parent of the element to be patched.
        /// </summary>
        public XmlNode Parent;

        /// <summary>
        /// Element to be patched.
        /// </summary>
        public XmlNode OldChild;

        /// <summary>
        /// Patch value to be put in place of the patch.
        /// </summary>
        public XmlNode NewChild;
    }
}