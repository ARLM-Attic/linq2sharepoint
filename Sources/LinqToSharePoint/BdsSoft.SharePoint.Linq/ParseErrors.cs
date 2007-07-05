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
 * 0.2.1 - Introduction of ParseErrors, ParseError and ParseErrorCollection.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Helper class to handle parse errors. Methods will either throw an exception or log the errors on the query object.
    /// </summary>
    internal static class ParseErrors
    {
        public static XmlElement UnsupportedQueryOperator(this CamlQuery query, string queryOperator, int start, int end)
        {
            return KeepOrThrow(1, String.Format(Errors.UnsupportedQueryOperator, queryOperator), query, start, end);
        }

        public static XmlElement NonBoolConstantValueInPredicate(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(2, String.Format(Errors.NonBoolConstantValueInPredicate), query, start, end);
        }

        public static XmlElement PredicateAfterProjection(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(3, String.Format(Errors.PredicateAfterProjection), query, start, end);
        }

        public static XmlElement DateRangesOverlapInvalidValueArgument(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(4, String.Format(Errors.DateRangesOverlapInvalidValueArgument), query, start, end);
        }

        public static XmlElement DateRangesOverlapMissingFieldReferences(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(5, String.Format(Errors.DateRangesOverlapMissingFieldReferences), query, start, end);
        }

        public static XmlElement PredicateContainsNonEntityReference(this CamlQuery query, string member, int start, int end)
        {
            return KeepOrThrow(6, String.Format(Errors.PredicateContainsNonEntityReference, member), query, start, end);
        }

        public static XmlElement PredicateContainsNonEntityMethodCall(this CamlQuery query, string method, int start, int end)
        {
            return KeepOrThrow(7, String.Format(Errors.PredicateContainsNonEntityMethodCall, method), query, start, end);
        }

        public static XmlElement InvalidEntityReference(this CamlQuery query, string member, int start, int end)
        {
            return KeepOrThrow(8, String.Format(Errors.InvalidEntityReference, member), query, start, end);
        }

        public static XmlElement UnsupportedStringMethodCall(this CamlQuery query, string method, int start, int end)
        {
            return KeepOrThrow(9, String.Format(Errors.UnsupportedStringMethodCall, method), query, start, end);
        }

        public static XmlElement UnsupportedMethodCall(this CamlQuery query, string method, int start, int end)
        {
            return KeepOrThrow(10, String.Format(Errors.UnsupportedMethodCall, method), query, start, end);
        }

        public static XmlElement CantNegate(this CamlQuery query, string expression, int start, int end)
        {
            return KeepOrThrow(11, String.Format(Errors.CantNegate, expression), query, start, end);
        }

        public static XmlElement UnsupportedUnary(this CamlQuery query, string type, int start, int end)
        {
            return KeepOrThrow(12, String.Format(Errors.UnsupportedUnary, type), query, start, end);
        }

        public static XmlElement UnsupportedBinary(this CamlQuery query, string type, int start, int end)
        {
            return KeepOrThrow(13, String.Format(Errors.UnsupportedBinary, type), query, start, end);
        }

        public static XmlElement InvalidNullValuedCondition(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(14, String.Format(Errors.InvalidNullValuedCondition), query, start, end);
        }

        public static XmlElement UnrecognizedEnumValue(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(15, String.Format(Errors.UnrecognizedEnumValue), query, start, end);
        }

        public static XmlElement UnsupportedOrdering(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(16, String.Format(Errors.UnsupportedOrdering), query, start, end);
        }

        public static XmlElement InvalidLookupMultiContainsCall(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(17, String.Format(Errors.InvalidLookupMultiContainsCall), query, start, end);
        }

        public static XmlElement SecondProjectionExpression(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(18, String.Format(Errors.SecondProjectionExpression), query, start, end);
        }

        public static XmlElement UnsupportedQueryExpression(this CamlQuery query, int start, int end)
        {
            return KeepOrThrow(99, String.Format(Errors.UnsupportedQueryExpression), query, start, end);
        }

        public static XmlElement GeneralError(this CamlQuery query, string message, int start, int end)
        {
            return KeepOrThrow(9999, message, query, start, end);
        }

        public static void FatalError(int start, int end)
        {
            throw new InvalidOperationException(Errors.FatalError);
        }

        private static XmlElement KeepOrThrow(int errorCode, string message, CamlQuery query, int start, int end)
        {
            if (query._errors != null)
            {
                int id = query._errors.Add(new ParseError(errorCode, message, start, end));

                XmlElement error = query._doc.CreateElement("ParseError");
                XmlAttribute idAttribute = query._doc.CreateAttribute("ID");
                idAttribute.Value = id.ToString();
                error.Attributes.Append(idAttribute);
                return error;
            }
            else
                throw new NotSupportedException(message);
        }
    }

    /// <summary>
    /// Represents a parse error with detailed error information.
    /// </summary>
    [Serializable]
    public class ParseError
    {
        private int _errorCode;

        /// <summary>
        /// Error code.
        /// </summary>
        public string ErrorCode
        {
            get
            {
                return String.Format("SP{0}", _errorCode.ToString().PadLeft(4, '0'));
            }
        }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Start position of the faulting expression in the original LINQ expression textual representation.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// End position of the faulting expression in the original LINQ expression textual representation.
        /// </summary>
        public int EndIndex { get; private set; }

        /// <summary>
        /// Creates a new parse error.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <param name="startIndex">Start position of the faulting expression in the original LINQ expression textual representation.</param>
        /// <param name="endIndex">End position of the faulting expression in the original LINQ expression textual representation.</param>
        public ParseError(int errorCode, string message, int startIndex, int endIndex)
        {
            _errorCode = errorCode;
            Message = message;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }

    /// <summary>
    /// Numbered dictionary collection of parse errors.
    /// </summary>
    [Serializable]
    public class ParseErrorCollection : Dictionary<int, ParseError>, ISerializable
    {
        public ParseErrorCollection()
        {
        }

        private ParseErrorCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Expression = info.GetString("Expression");
        }

        private int n = 0;

        /// <summary>
        /// Adds a new parse error to the collection.
        /// </summary>
        /// <param name="error">Parse error to add.</param>
        /// <returns>Unique identification code assigned to the parse error.</returns>
        public int Add(ParseError error)
        {
            int i = ++n;
            this.Add(i, error);
            return i;
        }

        /// <summary>
        /// Original complete LINQ expression textual representation.
        /// </summary>
        public string Expression { get; set; }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Expression", Expression);
        }
    }
}
