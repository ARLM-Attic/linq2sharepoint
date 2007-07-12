﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1318
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BdsSoft.SharePoint.Linq {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BdsSoft.SharePoint.Linq.Error_handling.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This method is not intended to be called directly; use it in LINQ query predicates only..
        /// </summary>
        internal static string CamlMethodsInvalidUse {
            get {
                return ResourceManager.GetString("CamlMethodsInvalidUse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t negate a {0} query expression..
        /// </summary>
        internal static string CantNegate {
            get {
                return ResourceManager.GetString("CantNegate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error occurred when connecting to the SharePoint site at {0}..
        /// </summary>
        internal static string ConnectionExceptionSp {
            get {
                return ResourceManager.GetString("ConnectionExceptionSp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error occurred when connecting to the SharePoint web service at {0}..
        /// </summary>
        internal static string ConnectionExceptionWs {
            get {
                return ResourceManager.GetString("ConnectionExceptionWs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A call to DateRangesOverlap should have entity property references as its fields arguments, all referring to the same entity type..
        /// </summary>
        internal static string DateRangesOverlapInvalidFieldReferences {
            get {
                return ResourceManager.GetString("DateRangesOverlapInvalidFieldReferences", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A call to DateRangesOverlap should not have an entity property reference as its value argument..
        /// </summary>
        internal static string DateRangesOverlapInvalidValueArgument {
            get {
                return ResourceManager.GetString("DateRangesOverlapInvalidValueArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A call to DateRangesOverlap should have one or more field references in its fields argument..
        /// </summary>
        internal static string DateRangesOverlapMissingFieldReferences {
            get {
                return ResourceManager.GetString("DateRangesOverlapMissingFieldReferences", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Query did not return any results..
        /// </summary>
        internal static string EmptySequence {
            get {
                return ResourceManager.GetString("EmptySequence", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error occurred in the predicate parser..
        /// </summary>
        internal static string FatalError {
            get {
                return ResourceManager.GetString("FatalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.codeplex.com/LINQtoSharePoint/Wiki/View.aspx?title=ErrorCode#{0}.
        /// </summary>
        internal static string HelpLink {
            get {
                return ResourceManager.GetString("HelpLink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Query predicate contains an entity reference that doesn&apos;t belong to a SharePoint list context: {0}..
        /// </summary>
        internal static string InvalidEntityReference {
            get {
                return ResourceManager.GetString("InvalidEntityReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid lookup field type for property {0}. Lookup fields should have a type derived from SharePointListEntity..
        /// </summary>
        internal static string InvalidLookupField {
            get {
                return ResourceManager.GetString("InvalidLookupField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Contains expressions for LookupMulti fields should match the referenced entity type..
        /// </summary>
        internal static string InvalidLookupMultiContainsCall {
            get {
                return ResourceManager.GetString("InvalidLookupMultiContainsCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Null value encountered in query condition. Only equality and non-equality null checks are supported..
        /// </summary>
        internal static string InvalidNullValuedCondition {
            get {
                return ResourceManager.GetString("InvalidNullValuedCondition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid OtherChoice field mapping for MultiChoice field {0}..
        /// </summary>
        internal static string InvalidOtherChoiceFieldMapping {
            get {
                return ResourceManager.GetString("InvalidOtherChoiceFieldMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Storage property on the FieldAttribute for entity property {0}..
        /// </summary>
        internal static string InvalidStoragePropertyFieldReference {
            get {
                return ResourceManager.GetString("InvalidStoragePropertyFieldReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid SharePoint Url field value..
        /// </summary>
        internal static string InvalidUrlParseArgument {
            get {
                return ResourceManager.GetString("InvalidUrlParseArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to List version mismatch between entity type and list definition on the server..
        /// </summary>
        internal static string ListVersionMismatch {
            get {
                return ResourceManager.GetString("ListVersionMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error has occurred in the Lookup field patcher..
        /// </summary>
        internal static string LookupFieldPatchError {
            get {
                return ResourceManager.GetString("LookupFieldPatchError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing field mapping attribute for entity property {0}..
        /// </summary>
        internal static string MissingFieldMappingAttribute {
            get {
                return ResourceManager.GetString("MissingFieldMappingAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing ListAttribute on the entity type..
        /// </summary>
        internal static string MissingListAttribute {
            get {
                return ResourceManager.GetString("MissingListAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Lookup field {0} doesn&apos;t have an associated LookupField attribute property set..
        /// </summary>
        internal static string MissingLookupFieldSetting {
            get {
                return ResourceManager.GetString("MissingLookupFieldSetting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown fill-in choice value was encountered for field {0} but an OtherChoice field mapping is missing. Is the entity mapping for the list outdated?.
        /// </summary>
        internal static string MissingOtherChoiceFieldMapping {
            get {
                return ResourceManager.GetString("MissingOtherChoiceFieldMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No primary key field found on entity type..
        /// </summary>
        internal static string MissingPrimaryKey {
            get {
                return ResourceManager.GetString("MissingPrimaryKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one primary key field found on entity type. There should only be one field marked as primary key on each entity type..
        /// </summary>
        internal static string MoreThanOnePrimaryKey {
            get {
                return ResourceManager.GetString("MoreThanOnePrimaryKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid query condition detected. Make sure references to entity type properties only occur on one side of a condition..
        /// </summary>
        internal static string MultipleEntityReferencesInCondition {
            get {
                return ResourceManager.GetString("MultipleEntityReferencesInCondition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non-boolean constant values are not supported in query predicates..
        /// </summary>
        internal static string NonBoolConstantValueInPredicate {
            get {
                return ResourceManager.GetString("NonBoolConstantValueInPredicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Lookup field {0} links to a non-existing LookupField: {1}..
        /// </summary>
        internal static string NonExistingLookupField {
            get {
                return ResourceManager.GetString("NonExistingLookupField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No property setter found on the non-read-only entity property {0}..
        /// </summary>
        internal static string NonReadOnlyFieldWithoutSetter {
            get {
                return ResourceManager.GetString("NonReadOnlyFieldWithoutSetter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lookup field subqueries are only supported for lookup fields that are unique. Field {0} violates this rule..
        /// </summary>
        internal static string NonUniqueLookupField {
            get {
                return ResourceManager.GetString("NonUniqueLookupField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Lookup field {0} has a null-valued LookupField: {1}. Did you mean a null-check on the Lookup field instead?.
        /// </summary>
        internal static string NullValuedLookupField {
            get {
                return ResourceManager.GetString("NullValuedLookupField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t add another predicate expression to the query..
        /// </summary>
        internal static string PredicateAfterProjection {
            get {
                return ResourceManager.GetString("PredicateAfterProjection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Query predicate contains a method call on a non-entity property: {0}..
        /// </summary>
        internal static string PredicateContainsNonEntityMethodCall {
            get {
                return ResourceManager.GetString("PredicateContainsNonEntityMethodCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Query predicate contains a non-entity property reference: {0}..
        /// </summary>
        internal static string PredicateContainsNonEntityReference {
            get {
                return ResourceManager.GetString("PredicateContainsNonEntityReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Second projection expression encountered during parsing. Queries can only contain one projection expression..
        /// </summary>
        internal static string SecondProjectionExpression {
            get {
                return ResourceManager.GetString("SecondProjectionExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing Storage property on the FieldAttribute for read-only entity property {0}..
        /// </summary>
        internal static string StoragePropertyMissingOnReadOnlyField {
            get {
                return ResourceManager.GetString("StoragePropertyMissingOnReadOnlyField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one unknown choice value encountered for field {0}. Only one fill-in value is supported. Is the entity mapping for the list outdated?.
        /// </summary>
        internal static string TooManyUnknownChoiceValues {
            get {
                return ResourceManager.GetString("TooManyUnknownChoiceValues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unrecognized enumeration flags value..
        /// </summary>
        internal static string UnrecognizedEnumValue {
            get {
                return ResourceManager.GetString("UnrecognizedEnumValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unrecognized mapping type encountered: {0}..
        /// </summary>
        internal static string UnrecognizedMappingType {
            get {
                return ResourceManager.GetString("UnrecognizedMappingType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported binary expression detected in query predicate: {0}..
        /// </summary>
        internal static string UnsupportedBinary {
            get {
                return ResourceManager.GetString("UnsupportedBinary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported method call detected in query predicate: {0}..
        /// </summary>
        internal static string UnsupportedMethodCall {
            get {
                return ResourceManager.GetString("UnsupportedMethodCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported ordering expression detected. Ordering expressions should only contain individual entity property expressions..
        /// </summary>
        internal static string UnsupportedOrdering {
            get {
                return ResourceManager.GetString("UnsupportedOrdering", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported query expression detected..
        /// </summary>
        internal static string UnsupportedQueryExpression {
            get {
                return ResourceManager.GetString("UnsupportedQueryExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Query operator {0} is not supported..
        /// </summary>
        internal static string UnsupportedQueryOperator {
            get {
                return ResourceManager.GetString("UnsupportedQueryOperator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported string filtering query expression detected: {0}. Only the methods Contains and StartsWith are supported..
        /// </summary>
        internal static string UnsupportedStringMethodCall {
            get {
                return ResourceManager.GetString("UnsupportedStringMethodCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported unary expression detected in query predicate: {0}..
        /// </summary>
        internal static string UnsupportedUnary {
            get {
                return ResourceManager.GetString("UnsupportedUnary", resourceCulture);
            }
        }
    }
}
