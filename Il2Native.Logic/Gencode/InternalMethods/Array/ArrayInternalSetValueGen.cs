namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayInternalSetValueGen
    {
        public static readonly string Name = "Void System.Array.InternalSetValue(Void*, System.Object)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
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
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 6);
            codeList.Add(Code.Stind_I1);
            codeList.Add(Code.Ret);

            // case 4(TypeCode.Char)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 7);
            codeList.Add(Code.Stind_I2);
            codeList.Add(Code.Ret);

            // case 5(TypeCode.SByte)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 8);
            codeList.Add(Code.Stind_I1);
            codeList.Add(Code.Ret);

            // case 6(TypeCode.Byte)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 9);
            codeList.Add(Code.Stind_I1);
            codeList.Add(Code.Ret);

            // case 7(TypeCode.Int16)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 10);
            codeList.Add(Code.Stind_I2);
            codeList.Add(Code.Ret);

            // case 8(TypeCode.UInt16)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 11);
            codeList.Add(Code.Stind_I2);
            codeList.Add(Code.Ret);

            // case 9(TypeCode.Int32)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 12);
            codeList.Add(Code.Stind_I4);
            codeList.Add(Code.Ret);

            // case 10(TypeCode.UInt32)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 13);
            codeList.Add(Code.Stind_I4);
            codeList.Add(Code.Ret);

            // case 11(TypeCode.Int64)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 14);
            codeList.Add(Code.Stind_I8);
            codeList.Add(Code.Ret);

            // case 12(TypeCode.UInt64)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 15);
            codeList.Add(Code.Stind_I8);
            codeList.Add(Code.Ret);

            // case 13(TypeCode.Single)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 16);
            codeList.Add(Code.Stind_R4);
            codeList.Add(Code.Ret);

            // case 14(TypeCode.Double)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 17);
            codeList.Add(Code.Stind_R8);
            codeList.Add(Code.Ret);

            // case 15(TypeCode.Decimal)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 18);
            codeList.Add(Code.Ret);

            // case 16(TypeCode.DateTime)
            @switch.Labels.Add(codeList.CreateLabel());
            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            // IntPtr.m_value
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadArgument(1);
            codeList.Add(Code.Unbox, 19);
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
            codeList.LoadArgument(1);
            codeList.Add(Code.Stind_Ref);

            codeList.Add(Code.Ret);

            var typedReferenceType = codeWriter.System.System_TypedReference;
            var intPtrType = codeWriter.System.System_IntPtr;

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(typedReferenceType, "Value", codeWriter));
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(typedReferenceType, "Type", codeWriter));
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(intPtrType, "m_value", codeWriter));
            tokenResolutions.Add(codeWriter.System.System_Object.ToPointerType());
            tokenResolutions.Add(codeWriter.System.System_Object);
            tokenResolutions.Add(codeWriter.System.System_Boolean);
            tokenResolutions.Add(codeWriter.System.System_Char);
            tokenResolutions.Add(codeWriter.System.System_SByte);
            tokenResolutions.Add(codeWriter.System.System_Byte);
            tokenResolutions.Add(codeWriter.System.System_Int16);
            tokenResolutions.Add(codeWriter.System.System_UInt16);
            tokenResolutions.Add(codeWriter.System.System_Int32);
            tokenResolutions.Add(codeWriter.System.System_UInt32);
            tokenResolutions.Add(codeWriter.System.System_Int64);
            tokenResolutions.Add(codeWriter.System.System_UInt64);
            tokenResolutions.Add(codeWriter.System.System_Single);
            tokenResolutions.Add(codeWriter.System.System_Double);
            tokenResolutions.Add(codeWriter.System.System_Decimal);
            tokenResolutions.Add(codeWriter.System.System_DateTime);
            tokenResolutions.Add(
                IlReader.Constructors(codeWriter.System.System_NotSupportedException, codeWriter).First(c => !c.GetParameters().Any()));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_Void.ToPointerType().ToParameter("ref"));
            parameters.Add(codeWriter.System.System_Object.ToParameter("obj"));

            yield return MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}