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
 * 0.2.1 - Introduction of RuntimeErrors.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Helper class to handle runtime errors, occurring during query execution and while retrieving results. Methods will throw an exception.
    /// </summary>
    internal static class RuntimeErrors
    {
        public static void MissingFieldMappingAttribute(string property)
        {
            throw new InvalidOperationException(String.Format(Errors.MissingFieldMappingAttribute, property));
        }

        public static void ListVersionMismatch()
        {
            throw new InvalidOperationException(String.Format(Errors.ListVersionMismatch));
        }

        public static void ConnectionExceptionSp(string url, Exception innerException)
        {
            throw new SharePointConnectionException(String.Format(Errors.ConnectionExceptionSp, url), innerException);
        }

        public static void ConnectionExceptionWs(string url, Exception innerException)
        {
            throw new SharePointConnectionException(String.Format(Errors.ConnectionExceptionWs, url), innerException);
        }

        public static void InvalidLookupField(string property)
        {
            throw new NotSupportedException(String.Format(Errors.InvalidLookupField, property));
        }

        public static void LookupFieldPatchError()
        {
            throw new InvalidOperationException(Errors.LookupFieldPatchError);
        }

        public static void UnrecognizedMappingType(string fieldType)
        {
            throw new InvalidOperationException(String.Format(Errors.UnrecognizedMappingType, fieldType));
        }

        public static void TooManyUnknownChoiceValues(string property)
        {
            throw new InvalidOperationException(String.Format(Errors.TooManyUnknownChoiceValues, property));
        }

        public static void InvalidOtherChoiceFieldMapping(string property)
        {
            throw new InvalidOperationException(String.Format(Errors.InvalidOtherChoiceFieldMapping, property));
        }

        public static void MissingOtherChoiceFieldMapping(string property)
        {
            throw new InvalidOperationException(String.Format(Errors.MissingOtherChoiceFieldMapping, property));
        }

        public static void UnsupportedQueryOperator(string queryOperator)
        {
            throw new InvalidOperationException(String.Format(Errors.UnsupportedQueryOperator, queryOperator));
        }

        public static void EmptySequence()
        {
            throw new InvalidOperationException(String.Format(Errors.EmptySequence));
        }

        public static void FatalError()
        {
            throw new InvalidOperationException(Errors.FatalError);
        }
    }
}
