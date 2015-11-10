// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringGen.cs" company="">
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
        private static string _stringPrefixConstData;

        public static void StringAllocationSizeMethodBody(
            IlCodeBuilder codeList,
            ICodeWriter codeWriter,
            IType stringType,
            IType charType,
            bool increaseSizeByOne = false)
        {
            // add element size
            var elementSize = TypeGen.SystemTypeSizes[charType.Name];
            codeList.SizeOf(charType);

            // load length
            codeList.LoadArgument(0);

            if (increaseSizeByOne)
            {
                codeList.LoadConstant(1);
                codeList.Add(Code.Add);
            }

            codeList.Add(Code.Mul);

            codeList.SizeOf(stringType);
            codeList.Add(Code.Add);

            // calculate alignment
            codeList.Add(Code.Dup);

            var alignForType = Math.Max(CWriter.PointerSize, !charType.IsValueType() ? elementSize : CWriter.PointerSize);
            codeList.LoadConstant(alignForType - 1);
            codeList.Add(Code.Add);

            codeList.LoadConstant(~(alignForType - 1));
            codeList.Add(Code.And);

            // parameters
            codeList.Parameters.AddRange(GetParameters(codeWriter));
        }

        public static IList<IParameter> GetParameters(ICodeWriter codeWriter)
        {
            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_Int32.ToParameter("size"));
            return parameters;
        }

        /// <summary>
        /// resetting cahced values to force calling AddRequiredVirtualTablesDeclaration(stringType) and AddRequiredRttiDeclaration(stringType)
        /// </summary>
        public static void ResetClass()
        {
            _stringPrefixDataType = null;
            _stringPrefixConstData = null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static string GetStringPrefixDataType(ICodeWriter codeWriter)
        {
            if (_stringPrefixDataType != null)
            {
                return _stringPrefixDataType;
            }

            var sb = new StringBuilder();

            if (codeWriter.MultiThreadingSupport)
            {
                sb.Append("Byte* cond; ");
                sb.Append("Byte* lock; ");
            }

            sb.Append("Byte* vtable;");
            sb.Append("Int32 len");

            _stringPrefixDataType = sb.ToString();
            return _stringPrefixDataType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static string GetStringPrefixConstData(CWriter cWriter)
        {
            if (_stringPrefixConstData != null)
            {
                return _stringPrefixConstData;
            }

            ICodeWriter codeWriter = cWriter;

            var stringSystemType = codeWriter.System.System_String;

            var sb = new StringBuilder();

            if (codeWriter.MultiThreadingSupport)
            {
                sb.Append("(Byte*) -1, ");
                sb.Append("(Byte*) -1, ");
            }

            sb.Append(stringSystemType.GetVirtualTableNameReference(cWriter));

            _stringPrefixConstData = sb.ToString();
            return _stringPrefixConstData;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="charType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetStringTypeHeader(this CWriter cWriter, int length)
        {
            var charType = cWriter.System.System_Char;
            var typeString = cWriter.WriteToString(() => charType.WriteTypePrefix(cWriter));

            return "{ " + GetStringPrefixDataType(cWriter) + "; " + typeString + " data[" + length + "]; }";
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
            this CWriter cWriter,
            int length,
            int storeLength)
        {
            return GetStringPrefixConstData(cWriter) + ", " + storeLength + ", ";
        }

        public static IMethod GetCtorMethodByParameters(IType stringType, IEnumerable<IParameter> getParameters, ICodeWriter codeWriter)
        {
            var parameters = getParameters.ToArray();
            var method =
                IlReader.Methods(stringType, codeWriter)
                        .FirstOrDefault(m => m.Name.StartsWith("Ctor") && m.GetParameters().ToArray().IsMatchingParams(parameters));

            Debug.Assert(method != null, "String corresponding Ctor can't be found");

            return method;
        }

        public static void GetCtorSBytePtrStartLengthEncoding(ICodeWriter codeWriter, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            IType systemString = codeWriter.System.System_String;

            var codeBuilder = new IlCodeBuilder();

            codeBuilder.LoadArgument(0);
            codeBuilder.LoadArgument(1);
            codeBuilder.Add(Code.Add);
            codeBuilder.Add(Code.Conv_I);

            codeBuilder.LoadArgument(2);
            codeBuilder.LoadArgument(3);
            codeBuilder.Add(Code.Call, 1);
            codeBuilder.Add(Code.Ret);

            code = codeBuilder.GetCode();

            locals = new List<IType>();

            tokenResolutions = new List<object>();
            tokenResolutions.Add(systemString.GetFirstMethodByName("CreateStringFromEncoding", codeWriter));

            parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_SByte.ToPointerType().ToParameter("str"));
            parameters.Add(codeWriter.System.System_Int32.ToParameter("index"));
            parameters.Add(codeWriter.System.System_Int32.ToParameter("size"));
            parameters.Add(codeWriter.ResolveType("System.Text.Encoding").ToParameter("enc"));
        }

        public static void GetCtorSBytePtrStartLength(ICodeWriter codeWriter, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            IType systemString = codeWriter.System.System_String;

            var codeBuilder = new IlCodeBuilder();

            codeBuilder.LoadArgument(0);
            codeBuilder.LoadArgument(1);
            codeBuilder.Add(Code.Add);
            codeBuilder.Add(Code.Conv_I);

            codeBuilder.LoadArgument(2);
            codeBuilder.Add(Code.Call, 2);
            codeBuilder.Add(Code.Call, 1);
            codeBuilder.Add(Code.Ret);

            code = codeBuilder.GetCode();

            locals = new List<IType>();

            tokenResolutions = new List<object>();
            tokenResolutions.Add(systemString.GetFirstMethodByName("CreateStringFromEncoding", codeWriter));
            tokenResolutions.Add(OpCodeExtensions.GetFirstMethodByName(codeWriter.ResolveType("System.Text.Encoding"), "get_ASCII", codeWriter));

            parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_SByte.ToPointerType().ToParameter("str"));
            parameters.Add(codeWriter.System.System_Int32.ToParameter("index"));
            parameters.Add(codeWriter.System.System_Int32.ToParameter("size"));
        }

        public static void GetCtorSBytePtr(ICodeWriter codeWriter, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            IType systemString = codeWriter.System.System_String;

            var codeBuilder = new IlCodeBuilder();

            codeBuilder.LoadArgument(0);
            
            // calculate length
            codeBuilder.LoadArgument(0);
            codeBuilder.Add(Code.Call, 2);

            codeBuilder.Add(Code.Call, 3);
            codeBuilder.Add(Code.Call, 1);
            codeBuilder.Add(Code.Ret);

            code = codeBuilder.GetCode();

            locals = new List<IType>();

            tokenResolutions = new List<object>();
            tokenResolutions.Add(systemString.GetFirstMethodByName("CreateStringFromEncoding", codeWriter));
            tokenResolutions.Add(systemString.GetFirstMethodByName("strlen", codeWriter));
            tokenResolutions.Add(OpCodeExtensions.GetFirstMethodByName(codeWriter.ResolveType("System.Text.Encoding"), "get_ASCII", codeWriter));

            parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_SByte.ToPointerType().ToParameter("str"));
        }

        public static void GetStrLen(ICodeWriter codeWriter, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            var codeBuilder = new IlCodeBuilder();

            codeBuilder.LoadArgument(0);
            codeBuilder.SaveLocal(0);
            var initialJumpLabel = codeBuilder.Branch(Code.Br, Code.Br_S);

            var jumpBack = codeBuilder.CreateLabel();

            codeBuilder.LoadLocal(0);
            codeBuilder.LoadConstant(1);
            codeBuilder.Add(Code.Conv_I);
            codeBuilder.Add(Code.Add);
            codeBuilder.SaveLocal(0);

            codeBuilder.Add(initialJumpLabel);

            codeBuilder.LoadLocal(0);
            codeBuilder.Add(Code.Ldind_I1);
            codeBuilder.LoadConstant(0);

            codeBuilder.Branch(Code.Bgt, Code.Bgt_S, jumpBack);

            codeBuilder.LoadLocal(0);
            codeBuilder.LoadArgument(0);
            codeBuilder.Add(Code.Sub);

            // and if element size is bugger 1, you need to devide it by element size
            // codeBuilder.LoadConstant(<size>);
            // codeBuilder.Add(Code.Div);

            codeBuilder.Add(Code.Ret);

            code = codeBuilder.GetCode();

            locals = new List<IType>();
            locals.Add(codeWriter.System.System_SByte.ToPointerType());

            tokenResolutions = new List<object>();

            parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_SByte.ToPointerType().ToParameter("str"));
        }
    }
}