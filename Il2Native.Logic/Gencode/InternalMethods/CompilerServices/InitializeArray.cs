namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class InitializeArrayGen
    {
        public static readonly string Name = "Void System.Runtime.CompilerServices.RuntimeHelpers.InitializeArray(System.Array, System.RuntimeFieldHandle)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(typeResolver.System.System_Int32);
            ilCodeBuilder.Locals.Add(typeResolver.System.System_Void.ToPointerType());

            var arrayType = typeResolver.System.System_Byte.ToArrayType(1);
            var multiArrayType = typeResolver.System.System_Byte.ToArrayType(2);

            // check rank
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(arrayType);
            ilCodeBuilder.LoadField(arrayType.GetFieldByName("rank", typeResolver));
            ilCodeBuilder.LoadConstant(1);
            ilCodeBuilder.Add(Code.Sub);

            var multiArrayJumpLabel = ilCodeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);
            
            // single-dim array
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(arrayType);
            ilCodeBuilder.LoadFieldAddress(arrayType.GetFieldByName("data", typeResolver));
            ilCodeBuilder.Add(Code.Conv_I);
            ilCodeBuilder.SaveLocal(1);
            
            var skipMultiArray = ilCodeBuilder.Branch(Code.Br, Code.Br_S);

            ilCodeBuilder.Add(multiArrayJumpLabel);

            // multi-dim array
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(multiArrayType);
            ilCodeBuilder.LoadFieldAddress(multiArrayType.GetFieldByName("data", typeResolver));
            ilCodeBuilder.Add(Code.Conv_I);
            ilCodeBuilder.SaveLocal(1);

            ilCodeBuilder.Add(skipMultiArray);           

            ilCodeBuilder.LoadLocal(1);
            ilCodeBuilder.Add(Code.Conv_I);

#if ADD_ARRAY_ALIGMENT
            // align pointer
            // Load ElementSize
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(arrayType);
            ilCodeBuilder.LoadField(arrayType.GetFieldByName("elementSize", typeResolver));

            // elementSize - 1
            ilCodeBuilder.LoadConstant(1);
            ilCodeBuilder.Add(Code.Sub);
            ilCodeBuilder.Add(Code.Dup);
            ilCodeBuilder.SaveLocal(0);

            // size + align - 1
            ilCodeBuilder.Add(Code.Add);

            // to allow arithmetic operation on address
            ilCodeBuilder.Add(Code.Conv_I);

            ilCodeBuilder.Add(Code.Conv_I8);

            // size &= ~(align - 1)
            ilCodeBuilder.LoadLocal(0);

            // to allow arithmetic operation on address
            ilCodeBuilder.Add(Code.Conv_I);
            ilCodeBuilder.LoadConstant(-1);
            ilCodeBuilder.Add(Code.Xor);
            ilCodeBuilder.Add(Code.And);
            ilCodeBuilder.Add(Code.Conv_I);
#endif 

            // load field pointer and size
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.LoadField(typeResolver.System.System_RuntimeFieldHandle.GetFieldByName("fieldAddress", typeResolver));
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.LoadField(typeResolver.System.System_RuntimeFieldHandle.GetFieldByName("fieldSize", typeResolver));
            ilCodeBuilder.Add(Code.Cpblk);

            ilCodeBuilder.Add(Code.Ret);

            yield return ilCodeBuilder.Register(Name, typeResolver);
        }
    }
}