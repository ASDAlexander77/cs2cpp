// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArraySingleDimensionGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using CodeParts;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class StringGen
    {
        /// <summary>
        /// </summary>
        private static string _stringPrefixDataType;

        /// <summary>
        /// </summary>
        private static string _stringPrefixNullConstData;

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static string GetStringPrefixDataType(ITypeResolver typeResolver)
        {
            //return "i8*, i8*, i8*, i32, i32";
            if (_stringPrefixDataType != null)
            {
                return _stringPrefixDataType;
            }

            var stringSystemType = typeResolver.System.System_String;

            var sb = new StringBuilder();

            sb.Append("i8*");
            foreach (var @interface in stringSystemType.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct())
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                sb.Append("i8*");
            }

            sb.Append(", i32");

            _stringPrefixDataType = sb.ToString();
            return _stringPrefixDataType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static string GetStringPrefixConstData(LlvmWriter llvmWriter)
        {
            if (_stringPrefixNullConstData != null)
            {
                return _stringPrefixNullConstData;
            }

            ITypeResolver typeResolver = llvmWriter;

            var stringSystemType = typeResolver.System.System_String;

            llvmWriter.AddRequiredVirtualTablesDeclaration(stringSystemType);

            var sb = new StringBuilder();

            sb.Append("i8* bitcast (i8** ");
            sb.Append(stringSystemType.GetVirtualTableReference(typeResolver));
            sb.AppendLine(" to i8*)");

            foreach (var @interface in stringSystemType.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct())
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                sb.Append("i8* bitcast (i8** ");
                sb.Append(stringSystemType.GetVirtualTableReference(@interface, typeResolver));
                sb.AppendLine(" to i8*)");                
            }

            _stringPrefixNullConstData = sb.ToString();
            return _stringPrefixNullConstData;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="charType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetStringTypeHeader(this LlvmWriter llvmWriter, int length)
        {
            var charType = llvmWriter.System.System_Char;
            var typeString = llvmWriter.WriteToString(
                () =>
                {
                    charType.WriteTypePrefix(llvmWriter);
                });

            return "{ " + GetStringPrefixDataType(llvmWriter) + ", [" + length + " x " + typeString + "] }";
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="name">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetStringTypeReference(
            this LlvmWriter llvmWriter,
            string name,
            int length)
        {
            var convertString = llvmWriter.WriteToString(
                () =>
                {
                    var writer = llvmWriter.Output;

                    var charType = llvmWriter.System.System_Char;
                    var stringType = llvmWriter.System.System_String;
                    writer.Write("bitcast (");
                    writer.Write("{1}* {0} to ", name, llvmWriter.GetStringTypeHeader(length));
                    stringType.WriteTypePrefix(llvmWriter);
                    writer.Write(")");
                });

            return convertString;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <param name="storeLength">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetStringValuesHeader(
            this LlvmWriter llvmWriter,
            int length,
            int storeLength)
        {
            var charType = llvmWriter.System.System_Char;
            var typeString = llvmWriter.WriteToString(
                () =>
                {
                    charType.WriteTypePrefix(llvmWriter);
                });

            return GetStringPrefixConstData(llvmWriter) + ", i32 " + storeLength + ", [" + length + " x " + typeString + "]";
        }
    }
}