namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using CodeParts;

    using PEAssemblyReader;

    public static class EnumGen
    {
        public static IEnumerable<IField> GetFields(IType enumType, ITypeResolver typeResolver)
        {
            Debug.Assert(enumType.IsEnum, "This is for enum arrays only");
            yield return enumType.GetEnumUnderlyingType().ToField(enumType, "Value");
        }

        public static void GetEnumGetHashCodeMethod(
            IType enumType,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            Debug.Assert(enumType.ToNormal().IsEnum, "This is for enum only");

            var codeList = new List<object>();

            codeList.Add(Code.Ldarg_0);
            codeList.AppendInt(Code.Ldfld, 1);
            codeList.Add(Code.Conv_I4);

            // return
            codeList.Add(Code.Ret);

            // End of Code
            // tokens
            tokenResolutions = new List<object>();
            // data
            tokenResolutions.Add(enumType.GetFieldByName("Value", typeResolver));            

            // locals
            locals = new List<IType>();

            // code
            code = codeList.ToArray();

            // parameters
            parameters = new List<IParameter>();
        }

        public static void GetEnumToStringMethod(
                    IType enumType,
                    ITypeResolver typeResolver,
                    out object[] code,
                    out IList<object> tokenResolutions,
                    out IList<IType> locals,
                    out IList<IParameter> parameters)
        {
            Debug.Assert(enumType.ToNormal().IsEnum, "This is for enum only");

            var codeList = new List<object>();

            codeList.Add(Code.Ldarg_0);
            codeList.AppendInt(Code.Ldfld, 1);

            var sizeOfEnum = enumType.GetEnumUnderlyingType().IntTypeBitSize() / 8;

            // build cmp/jmp cases
            var stringValues = 1;
            foreach (var enumConstValue in IlReader.Fields(enumType, typeResolver).Where(f => f.IsConst))
            {
                codeList.Add(Code.Dup);
                if (sizeOfEnum == 8)
                {
                    codeList.AppendULong(Code.Ldc_I8, Convert.ToUInt64(enumConstValue.ConstantValue));
                }
                else
                {
                    codeList.AppendUInt(Code.Ldc_I4, unchecked((uint)Convert.ToInt64(enumConstValue.ConstantValue)));
                }

                codeList.Add(Code.Bne_Un_S);
                codeList.Add(6);
                codeList.AppendInt(Code.Ldstr, ++stringValues);
                codeList.Add(Code.Ret);
            }

            codeList.Add(Code.Pop);
            codeList.AppendInt(Code.Ldstr, ++stringValues);
            codeList.Add(Code.Ret);

            // End of Code
            // tokens
            tokenResolutions = new List<object>();
            // data
            tokenResolutions.Add(enumType.GetFieldByName("Value", typeResolver));
            foreach (var enumConstValue in IlReader.Fields(enumType, typeResolver).Where(f => f.IsConst))
            {
                tokenResolutions.Add(enumConstValue.Name);
            }

            // default value
            tokenResolutions.Add(string.Empty);

            // locals
            locals = new List<IType>();

            // code
            code = codeList.ToArray();

            // parameters
            parameters = new List<IParameter>();
        }
    }
}
