namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class TypedReferenceInternalToObjectGen
    {
        public static readonly string Name = "System.Object System.TypedReference.InternalToObject(Void*)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new IlCodeBuilder();

            // get TypeCode of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 2);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Conv_I4);

            // switch
            var @switch = codeList.Switch();

            // goto default case
            //var defaultCaseLabel1 = codeList.Branch(Code.Br, Code.Br_S);

            // TODO: do not support Structs for now
            var defaultCaseLabel1 = codeList.CreateLabel();
            codeList.Add(Code.Newobj, 20);
            codeList.Add(Code.Throw);

            // case 0(TypeCode.Empty) -> Default
            @switch.Labels.Add(defaultCaseLabel1);

            // case 1(TypeCode.Object) -> Default
            @switch.Labels.Add(defaultCaseLabel1);

            // case 2(TypeCode.DBNull) -> Default
            @switch.Labels.Add(defaultCaseLabel1);

            // case 3(TypeCode.Boolean)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_U1);
            codeList.Add(Code.Box, 6);
            codeList.Add(Code.Ret);

            // case 4(TypeCode.Char)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_U2);
            codeList.Add(Code.Box, 7);
            codeList.Add(Code.Ret);

            // case 5(TypeCode.SByte)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_I1);
            codeList.Add(Code.Box, 8);
            codeList.Add(Code.Ret);

            // case 6(TypeCode.Byte)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_U1);
            codeList.Add(Code.Box, 9);
            codeList.Add(Code.Ret);

            // case 7(TypeCode.Int16)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_I2);
            codeList.Add(Code.Box, 10);
            codeList.Add(Code.Ret);

            // case 8(TypeCode.UInt16)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_U2);
            codeList.Add(Code.Box, 11);
            codeList.Add(Code.Ret);

            // case 9(TypeCode.Int32)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_I4);
            codeList.Add(Code.Box, 12);
            codeList.Add(Code.Ret);

            // case 10(TypeCode.UInt32)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_U4);
            codeList.Add(Code.Box, 13);
            codeList.Add(Code.Ret);

            // case 11(TypeCode.Int64)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_I8);
            codeList.Add(Code.Box, 14);
            codeList.Add(Code.Ret);

            // case 12(TypeCode.UInt64)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_I8);
            codeList.Add(Code.Box, 15);
            codeList.Add(Code.Ret);

            // case 13(TypeCode.Single)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_R4);
            codeList.Add(Code.Box, 16);
            codeList.Add(Code.Ret);

            // case 14(TypeCode.Double)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Ldind_R8);
            codeList.Add(Code.Box, 17);
            codeList.Add(Code.Ret);

            // case 15(TypeCode.Decimal)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            // to load structure
            codeList.Add(Code.Box, 18);
            codeList.Add(Code.Ret);

            // case 16(TypeCode.DateTime)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3); 
            // to load structure
            codeList.Add(Code.Box, 19);
            codeList.Add(Code.Ret);

            // case 17 -> Default
            @switch.Labels.Add(codeList.CreateLabel());
            // throw NotSupportedException
            codeList.Add(Code.Newobj, 20);
            codeList.Add(Code.Throw);

            // case 18(TypeCode.String) -> Default
            @switch.Labels.Add(defaultCaseLabel1);

            // default:
            codeList.Add(defaultCaseLabel1);

            // get Value of TypedReference (default case)
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldind_Ref);

            codeList.Add(Code.Ret);

            var typedReferenceType = typeResolver.System.System_TypedReference;
            var intPtrType = typeResolver.System.System_IntPtr;

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Value", typeResolver));
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Type", typeResolver));
            tokenResolutions.Add(intPtrType.GetFieldByName("m_value", typeResolver));
            tokenResolutions.Add(typeResolver.System.System_Object.ToPointerType());
            tokenResolutions.Add(typeResolver.System.System_Object);
            tokenResolutions.Add(typeResolver.System.System_Boolean);
            tokenResolutions.Add(typeResolver.System.System_Char);
            tokenResolutions.Add(typeResolver.System.System_SByte);
            tokenResolutions.Add(typeResolver.System.System_Byte);
            tokenResolutions.Add(typeResolver.System.System_Int16);
            tokenResolutions.Add(typeResolver.System.System_UInt16);
            tokenResolutions.Add(typeResolver.System.System_Int32);
            tokenResolutions.Add(typeResolver.System.System_UInt32);
            tokenResolutions.Add(typeResolver.System.System_Int64);
            tokenResolutions.Add(typeResolver.System.System_UInt64);
            tokenResolutions.Add(typeResolver.System.System_Single);
            tokenResolutions.Add(typeResolver.System.System_Double);
            tokenResolutions.Add(typeResolver.System.System_Decimal);
            tokenResolutions.Add(typeResolver.System.System_DateTime);
            tokenResolutions.Add(
                IlReader.Constructors(typeResolver.System.System_NotSupportedException, typeResolver).First(c => !c.GetParameters().Any()));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_Void.ToPointerType().ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}