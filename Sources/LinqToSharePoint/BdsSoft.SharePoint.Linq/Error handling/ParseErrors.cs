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

#region Namespace imports

using System;
using System.Xml;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Helper class to handle parse errors. Methods will either throw an exception or log the errors on the query object.
    /// </summary>
    internal static class ParseErrors
    {
        public static XmlElement UnsupportedQueryOperator(this QueryParser parser, string queryOperator, int start, int end)
        {
            return KeepOrThrow(parser, 1, String.Format(Errors.UnsupportedQueryOperator, queryOperator), start, end);
        }

        public static XmlElement NonBoolConstantValueInPredicate(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 2, Errors.NonBoolConstantValueInPredicate, start, end);
        }

        public static XmlElement PredicateAfterProjection(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 3, Errors.PredicateAfterProjection, start, end);
        }

        public static XmlElement DateRangesOverlapInvalidValueArgument(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 4, Errors.DateRangesOverlapInvalidValueArgument, start, end);
        }

        public static XmlElement DateRangesOverlapMissingFieldReferences(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 5, Errors.DateRangesOverlapMissingFieldReferences, start, end);
        }

        public static XmlElement PredicateContainsNonEntityReference(this QueryParser parser, string member, int start, int end)
        {
            return KeepOrThrow(parser, 6, String.Format(Errors.PredicateContainsNonEntityReference, member), start, end);
        }

        public static XmlElement PredicateContainsNonEntityMethodCall(this QueryParser parser, string method, int start, int end)
        {
            return KeepOrThrow(parser, 7, String.Format(Errors.PredicateContainsNonEntityMethodCall, method), start, end);
        }

        public static XmlElement InvalidEntityReference(this QueryParser parser, string member, int start, int end)
        {
            return KeepOrThrow(parser, 8, String.Format(Errors.InvalidEntityReference, member), start, end);
        }

        public static XmlElement UnsupportedStringMethodCall(this QueryParser parser, string method, int start, int end)
        {
            return KeepOrThrow(parser, 9, String.Format(Errors.UnsupportedStringMethodCall, method), start, end);
        }

        public static XmlElement UnsupportedMethodCall(this QueryParser parser, string method, int start, int end)
        {
            return KeepOrThrow(parser, 10, String.Format(Errors.UnsupportedMethodCall, method), start, end);
        }

        public static XmlElement CantNegate(this QueryParser parser, string expression, int start, int end)
        {
            return KeepOrThrow(parser, 11, String.Format(Errors.CantNegate, expression), start, end);
        }

        public static XmlElement UnsupportedUnary(this QueryParser parser, string type, int start, int end)
        {
            return KeepOrThrow(parser, 12, String.Format(Errors.UnsupportedUnary, type), start, end);
        }

        public static XmlElement UnsupportedBinary(this QueryParser parser, string type, int start, int end)
        {
            return KeepOrThrow(parser, 13, String.Format(Errors.UnsupportedBinary, type), start, end);
        }

        public static XmlElement InvalidNullValuedCondition(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 14, Errors.InvalidNullValuedCondition, start, end);
        }

        public static XmlElement UnrecognizedEnumValue(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 15, Errors.UnrecognizedEnumValue, start, end);
        }

        public static XmlElement UnsupportedOrdering(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 16, Errors.UnsupportedOrdering, start, end);
        }

        public static XmlElement InvalidLookupMultiContainsCall(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 17, Errors.InvalidLookupMultiContainsCall, start, end);
        }

        public static XmlElement SecondProjectionExpression(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 18, Errors.SecondProjectionExpression, start, end);
        }

        public static XmlElement DateRangesOverlapInvalidFieldReferences(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 19, Errors.DateRangesOverlapInvalidFieldReferences, start, end);
        }

        public static XmlElement NonUniqueLookupField(this QueryParser parser, string property, int start, int end)
        {
            return KeepOrThrow(parser, 20, String.Format(Errors.NonUniqueLookupField, property), start, end);
        }

        public static XmlElement MissingLookupFieldSetting(this QueryParser parser, string field, int start, int end)
        {
            return KeepOrThrow(parser, 21, String.Format(Errors.MissingLookupFieldSetting, field), start, end);
        }

        public static XmlElement NonExistingLookupField(this QueryParser parser, string field, string lookup, int start, int end)
        {
            return KeepOrThrow(parser, 22, String.Format(Errors.NonExistingLookupField, field, lookup), start, end);
        }

        public static XmlElement NullValuedLookupField(this QueryParser parser, string field, string lookup, int start, int end)
        {
            return KeepOrThrow(parser, 23, String.Format(Errors.NullValuedLookupField, field, lookup), start, end);
        }

        public static XmlElement MultipleEntityReferencesInCondition(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 24, Errors.MultipleEntityReferencesInCondition, start, end);
        }

        public static XmlElement UnsupportedQueryExpression(this QueryParser parser, int start, int end)
        {
            return KeepOrThrow(parser, 99, Errors.UnsupportedQueryExpression, start, end);
        }

        public static XmlElement MissingFieldMappingAttribute(this QueryParser parser, string property)
        {
            return KeepOrThrow(parser, 101, String.Format(Errors.MissingFieldMappingAttribute, property), 0, 0);
        }

        public static XmlElement GeneralError(this QueryParser parser, string message, int start, int end)
        {
            return KeepOrThrow(parser, 9999, message, start, end);
        }

        public static void LookupFieldPatchError()
        {
            throw new InvalidOperationException(Errors.LookupFieldPatchError);
        }

        public static void FatalError(int start, int end)
        {
            throw new InvalidOperationException(Errors.FatalError);
        }

        private static XmlElement KeepOrThrow(QueryParser parser, int errorCode, string message, int start, int end)
        {
            //
            // Error object.
            //
            ParseError error = new ParseError(errorCode, message, start, end);

            //
            // Check run mode.
            //
            if (parser._errors != null)
            {
                //
                // Create error message and add it to the errors collection of the query object.
                //
                int id = parser._errors.Add(error);

                //
                // Generate an error reference tag that will be inserted in the generated CAML on the faulting position.
                //
                return parser._factory.ParseError(id);
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
