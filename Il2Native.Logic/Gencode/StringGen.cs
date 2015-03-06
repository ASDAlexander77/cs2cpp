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

    using Il2Native.Logic.CodeParts;

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


        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        public static FullyDefinedReference WriteStringAllocationSize(
            this CWriter cWriter,
            OpCodePart opCode,
            IType stringType,
            IType charType)
        {
            Debug.Assert(stringType.IsString, "This is for string only");

            var writer = cWriter.Output;

            writer.WriteLine("; Calculate String allocation size");

            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            GetCalculationPartOfStringAllocationSizeMethodBody(
                cWriter,
                stringType,
                charType,
                out code,
                out tokenResolutions,
                out locals,
                out parameters);

            var constructedMethod = MethodBodyBank.GetMethodDecorator(null, code, tokenResolutions, locals, parameters);

            // actual write
            var opCodes = cWriter.WriteCustomMethodPart(constructedMethod, null);
            return opCodes.Last().Result;
        }

        private static void GetCalculationPartOfStringAllocationSizeMethodBody(
            ITypeResolver typeResolver,
            IType stringType,
            IType charType,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            var codeList = new List<object>();

            // add element size
            var elementSize = charType.GetTypeSize(typeResolver, true);
            codeList.AppendLoadInt(elementSize);

            // load length
            codeList.AppendLoadArgument(0);
            codeList.Add(Code.Mul);

            var arrayTypeSizeWithoutArrayData = stringType.GetTypeSize(typeResolver);
            codeList.AppendLoadInt(arrayTypeSizeWithoutArrayData);
            codeList.Add(Code.Add);

            // calculate alignment
            codeList.Add(Code.Dup);

            var alignForType = Math.Max(CWriter.PointerSize, !charType.IsStructureType() ? elementSize : CWriter.PointerSize);
            codeList.AppendLoadInt(alignForType - 1);
            codeList.Add(Code.Add);

            codeList.AppendLoadInt(~(alignForType - 1));
            codeList.Add(Code.And);

            // locals
            locals = new List<IType>();

            // tokens
            tokenResolutions = new List<object>();

            // parameters
            parameters = GetParameters(typeResolver);

            code = codeList.ToArray();
        }

        public static IList<IParameter> GetParameters(ITypeResolver typeResolver)
        {
            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_Int32.ToParameter());
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
        public static string GetStringPrefixConstData(CWriter cWriter)
        {
            if (_stringPrefixConstData != null)
            {
                return _stringPrefixConstData;
            }

            ITypeResolver typeResolver = cWriter;

            var stringSystemType = typeResolver.System.System_String;

            cWriter.AddRequiredVirtualTablesDeclaration(stringSystemType);
            cWriter.AddRequiredRttiDeclaration(stringSystemType);

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
            var typeString = cWriter.WriteToString(
                () =>
                {
                    charType.WriteTypePrefix(cWriter);
                });

            return "{ " + GetStringPrefixDataType(cWriter) + ", [" + length + " x " + typeString + "] }";
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
            this CWriter cWriter,
            string name,
            int length)
        {
            var convertString = cWriter.WriteToString(
                () =>
                {
                    var writer = cWriter.Output;

                    var charType = cWriter.System.System_Char;
                    var stringType = cWriter.System.System_String;
                    writer.Write("bitcast (");
                    writer.Write("{1}* {0} to ", name, cWriter.GetStringTypeHeader(length));
                    stringType.WriteTypePrefix(cWriter);
                    writer.Write(")");
                });

            return convertString;
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
            var charType = cWriter.System.System_Char;
            var typeString = cWriter.WriteToString(
                () =>
                {
                    charType.WriteTypePrefix(cWriter);
                });

            return GetStringPrefixConstData(cWriter) + ", i32 " + storeLength + ", [" + length + " x " + typeString + "]";
        }

        public static IMethod GetCtorMethodByParameters(IType stringType, IEnumerable<IParameter> getParameters, ITypeResolver typeResolver)
        {
            var parameters = getParameters.ToArray();
            var method =
                IlReader.Methods(stringType, typeResolver)
                        .FirstOrDefault(m => m.Name.StartsWith("Ctor") && m.GetParameters().ToArray().IsMatchingParams(parameters));

            Debug.Assert(method != null, "String corresponding Ctor can't be found");

            return method;
        }

        public static void GetCtorSBytePtrStartLengthEncoding(ITypeResolver typeResolver, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            IType systemString = typeResolver.System.System_String;

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
            tokenResolutions.Add(systemString.GetMethodByName("CreateStringFromEncoding", typeResolver));

            parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_SByte.ToPointerType().ToParameter());
            parameters.Add(typeResolver.System.System_Int32.ToParameter());
            parameters.Add(typeResolver.System.System_Int32.ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Text.Encoding").ToParameter());
        }

        public static void GetCtorSBytePtrStartLength(ITypeResolver typeResolver, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            IType systemString = typeResolver.System.System_String;

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
            tokenResolutions.Add(systemString.GetMethodByName("CreateStringFromEncoding", typeResolver));
            tokenResolutions.Add(typeResolver.ResolveType("System.Text.Encoding").GetMethodByName("get_ASCII", typeResolver));

            parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_SByte.ToPointerType().ToParameter());
            parameters.Add(typeResolver.System.System_Int32.ToParameter());
            parameters.Add(typeResolver.System.System_Int32.ToParameter());
        }

        public static void GetCtorSBytePtr(ITypeResolver typeResolver, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            IType systemString = typeResolver.System.System_String;

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
            tokenResolutions.Add(systemString.GetMethodByName("CreateStringFromEncoding", typeResolver));
            tokenResolutions.Add(systemString.GetMethodByName("strlen", typeResolver));
            tokenResolutions.Add(typeResolver.ResolveType("System.Text.Encoding").GetMethodByName("get_ASCII", typeResolver));

            parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_SByte.ToPointerType().ToParameter());
        }

        public static void GetStrLen(ITypeResolver typeResolver, out byte[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
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
            locals.Add(typeResolver.System.System_SByte.ToPointerType());

            tokenResolutions = new List<object>();

            parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_SByte.ToPointerType().ToParameter());
        }
    }
}