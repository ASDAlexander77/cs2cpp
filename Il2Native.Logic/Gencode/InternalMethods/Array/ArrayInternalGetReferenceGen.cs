namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayInternalGetReferenceGen
    {
        public static readonly string Name = "Void System.Array.InternalGetReference(Void*, Int32, Int32*)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new IlCodeBuilder();

            // set Type of TypedReference
            codeList.LoadArgument(1);
            codeList.Add(Code.Ldflda, 2);

            // Load element typeCode
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldfld, 5);

            // Save typeCode into TypedReference TypeCode
            codeList.Add(Code.Conv_I);
            codeList.Add(Code.Stfld, 3);

            // Calculate data index
            // check if it 1-dim array
            codeList.LoadArgument(2);
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);
            var labelGotoMultiDimArray = codeList.Branch(Code.Brtrue, Code.Brtrue_S);

            // set Value of TypedReference
            codeList.LoadArgument(1);
            codeList.Add(Code.Ldflda, 1);

            // Load reference to an array (do not load reference to field data)
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 4);

            // Load elementSize
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldfld, 6);

            // Load index
            codeList.LoadArgument(3);
            codeList.Add(Code.Ldind_I4);

            // multiply it
            codeList.Add(Code.Mul);

            // load address of an element
            codeList.Add(Code.Ldelema, 7);

            // Save address into TypedReference Value
            codeList.Add(Code.Conv_I);
            codeList.Add(Code.Stfld, 3);

            codeList.Add(labelGotoMultiDimArray);
            codeList.Add(Code.Ret);

            var typedReferenceType = typeResolver.ResolveType("System.TypedReference");
            var intPtrType = typeResolver.ResolveType("System.IntPtr");
            var byteType = typeResolver.ResolveType("System.Byte");
            var arrayType = byteType.ToArrayType(1);

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Value", typeResolver));
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Type", typeResolver));
            tokenResolutions.Add(intPtrType.GetFieldByName("m_value", typeResolver));
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(arrayType.GetFieldByName("typeCode", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("elementSize", typeResolver));
            tokenResolutions.Add(byteType);

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.ResolveType("System.Void").ToPointerType().ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Int32").ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Int32").ToPointerType().ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}