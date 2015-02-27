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
            codeList.Add(Code.Dup);
            codeList.SaveLocal(0);

            // Load index
            codeList.LoadArgument(3);
            codeList.Add(Code.Ldind_I4);

            // multiply it
            codeList.Add(Code.Mul);

            // load address of an element
            codeList.Add(Code.Ldelema, 7);

            // align index
            codeList.LoadLocal(0);
            // elementSize - 1
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);
            codeList.Add(Code.Dup);
            codeList.SaveLocal(0);

            // size + align - 1
            codeList.Add(Code.Add);

            // size &= ~(align - 1)
            codeList.LoadLocal(0);
            codeList.LoadConstant(-1);
            codeList.Add(Code.Xor);
            codeList.Add(Code.And);

            // Save address into TypedReference Value
            codeList.Add(Code.Conv_I);
            codeList.Add(Code.Stfld, 3);
            codeList.Add(Code.Ret);

            // for multiarray
            codeList.Add(labelGotoMultiDimArray);

            // *indeces += count;
            codeList.LoadArgument(3);
            codeList.LoadArgument(2);
            codeList.Add(Code.Conv_I);
            codeList.LoadConstant(4);
            codeList.Add(Code.Mul);
            codeList.Add(Code.Add);
            codeList.SaveArgument(3);

            // init multiplier
            // Load elementSize
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldfld, 6);
            codeList.SaveLocal(0);

            // calculate first index
            codeList.LoadArgument(3);
            codeList.LoadConstant(4);
            codeList.Add(Code.Sub);
            codeList.Add(Code.Dup);
            codeList.SaveArgument(3);
            codeList.Add(Code.Ldind_I4);
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 9);
            codeList.Add(Code.Ldfld, 10);
            codeList.LoadConstant(0);
            codeList.Add(Code.Ldelem_I4);
            codeList.Add(Code.Sub);
            codeList.LoadLocal(0);
            codeList.Add(Code.Mul);
            codeList.SaveLocal(1);           
            
            // init 'i' (index)
            codeList.LoadConstant(1);
            codeList.SaveLocal(2);

            var labelLoopStart = codeList.Branch(Code.Br, Code.Br_S);
            // loop start

            var labelLoopBack = codeList.CreateLabel();

            codeList.LoadLocal(0);
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 9);
            codeList.Add(Code.Ldfld, 11);
            codeList.LoadLocal(2);
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);
            codeList.Add(Code.Ldelem_I4);
            codeList.Add(Code.Mul);
            codeList.SaveLocal(0);
            codeList.LoadLocal(1);
            codeList.LoadArgument(3);
            codeList.LoadConstant(4);
            codeList.Add(Code.Sub);
            codeList.Add(Code.Dup);
            codeList.SaveArgument(3);
            codeList.Add(Code.Ldind_I4);
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 9);
            codeList.Add(Code.Ldfld, 10);
            codeList.LoadLocal(2);
            codeList.Add(Code.Ldelem_I4);
            codeList.Add(Code.Sub);
            codeList.LoadLocal(0);
            codeList.Add(Code.Mul);
            codeList.Add(Code.Add);
            codeList.SaveLocal(1);
            codeList.LoadLocal(2);
            codeList.LoadConstant(1);
            codeList.Add(Code.Add);
            codeList.SaveLocal(2);

            codeList.Add(labelLoopStart);

            codeList.LoadLocal(2);
            codeList.LoadArgument(2);

            codeList.Branch(Code.Blt, Code.Blt_S, labelLoopBack);

            // set Value of TypedReference
            codeList.LoadArgument(1);
            codeList.Add(Code.Ldflda, 1);

            // align index (array offset) (Local.0) and save it to TypedReferenece
            // Load reference to an array (do not load reference to field data)
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 9);
            codeList.LoadLocal(1);
            // load address of an element
            codeList.Add(Code.Ldelema, 7);

            // elementSize 
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldfld, 6);

            // elementSize - 1
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);
            codeList.Add(Code.Dup);
            codeList.SaveLocal(2);

            // size + align - 1
            codeList.Add(Code.Add);

            // size &= ~(align - 1)
            codeList.LoadLocal(2);
            codeList.LoadConstant(-1);
            codeList.Add(Code.Xor);
            codeList.Add(Code.And);

            // Save address into TypedReference Value
            codeList.Add(Code.Conv_I);
            codeList.Add(Code.Stfld, 3);

            codeList.Add(Code.Ret);

            var typedReferenceType = typeResolver.System.System_TypedReference;
            var intPtrType = typeResolver.System.System_IntPtr;
            var byteType = typeResolver.System.System_Byte;
            var arrayType = byteType.ToArrayType(1);
            var multiArrayType = byteType.ToArrayType(2);

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Value", typeResolver));
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Type", typeResolver));
            tokenResolutions.Add(intPtrType.GetFieldByName("m_value", typeResolver));
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(arrayType.GetFieldByName("typeCode", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("elementSize", typeResolver));
            tokenResolutions.Add(byteType);
            tokenResolutions.Add(arrayType.GetFieldByName("rank", typeResolver));
            tokenResolutions.Add(multiArrayType);
            tokenResolutions.Add(multiArrayType.GetFieldByName("lowerBounds", typeResolver));
            tokenResolutions.Add(multiArrayType.GetFieldByName("lengths", typeResolver));

            var locals = new List<IType>();
            locals.Add(typeResolver.System.System_Int32);
            locals.Add(typeResolver.System.System_Int32);
            locals.Add(typeResolver.System.System_Int32);

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_Void.ToPointerType().ToParameter());
            parameters.Add(typeResolver.System.System_Int32.ToParameter());
            parameters.Add(typeResolver.System.System_Int32.ToPointerType().ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}