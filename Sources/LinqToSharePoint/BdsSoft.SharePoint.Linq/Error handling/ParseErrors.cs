/*
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
 * 0.2.1 - Introduction of ParseErrors.
 */

using System;
using System.Xml;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Helper class to handle parse errors. Methods will either throw an exception or log the errors on the query object.
    /// </summary>
    internal static class ParseErrors
    {
        public static XmlElement UnsupportedQueryOperator(this CamlQuery query, string queryOperator, int start, int end)
        {
            return KeepOrThrow(query, 1, String.Format(Errors.UnsupportedQueryOperator, queryOperator), start, end);
        }

        public static XmlElement NonBoolConstantValueInPredicate(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 2, String.Format(Errors.NonBoolConstantValueInPredicate), start, end);
        }

        public static XmlElement PredicateAfterProjection(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 3, String.Format(Errors.PredicateAfterProjection), start, end);
        }

        public static XmlElement DateRangesOverlapInvalidValueArgument(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 4, String.Format(Errors.DateRangesOverlapInvalidValueArgument), start, end);
        }

        public static XmlElement DateRangesOverlapMissingFieldReferences(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 5, String.Format(Errors.DateRangesOverlapMissingFieldReferences), start, end);
        }

        public static XmlElement PredicateContainsNonEntityReference(this CamlQuery query, string member, int start, int end)
        {
            return KeepOrThrow(query, 6, String.Format(Errors.PredicateContainsNonEntityReference, member), start, end);
        }

        public static XmlElement PredicateContainsNonEntityMethodCall(this CamlQuery query, string method, int start, int end)
        {
            return KeepOrThrow(query, 7, String.Format(Errors.PredicateContainsNonEntityMethodCall, method), start, end);
        }

        public static XmlElement InvalidEntityReference(this CamlQuery query, string member, int start, int end)
        {
            return KeepOrThrow(query, 8, String.Format(Errors.InvalidEntityReference, member), start, end);
        }

        public static XmlElement UnsupportedStringMethodCall(this CamlQuery query, string method, int start, int end)
        {
            return KeepOrThrow(query, 9, String.Format(Errors.UnsupportedStringMethodCall, method), start, end);
        }

        public static XmlElement UnsupportedMethodCall(this CamlQuery query, string method, int start, int end)
        {
            return KeepOrThrow(query, 10, String.Format(Errors.UnsupportedMethodCall, method), start, end);
        }

        public static XmlElement CantNegate(this CamlQuery query, string expression, int start, int end)
        {
            return KeepOrThrow(query, 11, String.Format(Errors.CantNegate, expression), start, end);
        }

        public static XmlElement UnsupportedUnary(this CamlQuery query, string type, int start, int end)
        {
            return KeepOrThrow(query, 12, String.Format(Errors.UnsupportedUnary, type), start, end);
        }

        public static XmlElement UnsupportedBinary(this CamlQuery query, string type, int start, int end)
        {
            return KeepOrThrow(query, 13, String.Format(Errors.UnsupportedBinary, type), start, end);
        }

        public static XmlElement InvalidNullValuedCondition(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 14, String.Format(Errors.InvalidNullValuedCondition), start, end);
        }

        public static XmlElement UnrecognizedEnumValue(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 15, String.Format(Errors.UnrecognizedEnumValue), start, end);
        }

        public static XmlElement UnsupportedOrdering(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 16, String.Format(Errors.UnsupportedOrdering), start, end);
        }

        public static XmlElement InvalidLookupMultiContainsCall(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 17, String.Format(Errors.InvalidLookupMultiContainsCall), start, end);
        }

        public static XmlElement SecondProjectionExpression(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 18, String.Format(Errors.SecondProjectionExpression), start, end);
        }

        public static XmlElement UnsupportedQueryExpression(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(query, 99, String.Format(Errors.UnsupportedQueryExpression), start, end);
        }

        public static XmlElement MissingFieldMappingAttribute(this CamlQuery query, string property)
        {
            return KeepOrThrow(query, 101, String.Format(Errors.MissingFieldMappingAttribute, property), 0, 0);
        }

        public static XmlElement GeneralError(this CamlQuery query, string message, int start, int end)
        {
            return KeepOrThrow(query, 9999, message, start, end);
        }

        public static void FatalError(int start, int end)
        {
            throw new InvalidOperationException(Errors.FatalError);
        }

        private static XmlElement KeepOrThrow(CamlQuery query, int errorCode, string message, int start, int end)
        {
            //
            // Error object.
            //
            ParseError error = new ParseError(errorCode, message, start, end);

            //
            // Check run mode.
            //
            if (query._errors != null)
            {
                //
                // Create error message and add it to the errors collection of the query object.
                //
                int id = query._errors.Add(error);

                //
                // Generate an error reference tag that will be inserted in the generated CAML on the faulting position.
                //
                XmlElement errorElement = query._doc.CreateElement("ParseError");
                XmlAttribute idAttribute = query._doc.CreateAttribute("ID");
                idAttribute.Value = id.ToString();
                errorElement.Attributes.Append(idAttribute);
                return errorElement;
            }
            else
            {
                //
                // Runtime mode: throw an exception.
                //
                NotSupportedException ex = new NotSupportedException(error.ToString());
                ex.HelpLink = error.HelpLink;
                throw ex;
            }
        }
    }
}
