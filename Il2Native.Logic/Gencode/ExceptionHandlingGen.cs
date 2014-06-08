namespace Il2Native.Logic.Gencode
{
    using Il2Native.Logic.CodeParts;
    using PEAssemblyReader;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// </summary>
    [Flags]
    public enum LandingPadOptions
    {
        /// <summary>
        /// </summary>
        None = 0,

        /// <summary>
        /// </summary>
        Cleanup = 1,

        /// <summary>
        /// </summary>
        EmptyFilter = 2
    }

    public static class ExceptionHandlingGen
    {
        public static void WriteThrow(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine("; Throw");
            llvmWriter.WriteAllocateException(writer, opCode);

            var exceptionPointerType = opCode.OpCodeOperands[0].ResultType;
            writer.Write("invoke void @__cxa_throw(i8* %8, i8* bitcast (");
            exceptionPointerType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*), i8* null)", exceptionPointerType.GetRttiPointerInfoName());
            writer.WriteLine("to label %.unreachable unwind label %{0}", string.Concat(".a", opCode.GroupAddressEnd));

            writer.Indent--;
            writer.Write(string.Concat(".a", opCode.GroupAddressEnd, ':'));
            writer.Indent++;
        }

        public static void WriteAllocateException(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine("; Allocate exception");
            llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.Write("call i8* @__cxa_allocate_exception(i32 {0})", LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);

            var newExceptionResult = opCode.OpCodeOperands[0].ResultNumber;
            var newExceptionResultType = opCode.OpCodeOperands[0].ResultType;

            opCode.OpCodeOperands[0].ResultNumber = opCode.ResultNumber;

            llvmWriter.WriteBitcast(writer, opCode, opCode.OpCodeOperands[0].ResultType, options: LlvmWriter.OperandOptions.GenerateResult);
            writer.WriteLine("*");

            opCode.OpCodeOperands[0].ResultNumber = newExceptionResult;

            llvmWriter.UnaryOper(writer, opCode, "store");
            writer.Write(", ");
            llvmWriter.WriteTypePrefix(writer, opCode.OpCodeOperands[0].ResultType);
            writer.Write("* ");
            llvmWriter.WriteResultNumber(opCode.ResultNumber ?? -1);
            writer.WriteLine(string.Empty);
        }

        public static void WriteUnexpectedCall(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            var result = llvmWriter.WriteSetResultNumber(writer, null);
            writer.Write("load i8** %2");
            writer.WriteLine("call void @__cxa_call_unexpected(i8* {0})", llvmWriter.GetResultNumber(result));
            writer.WriteLine("unreachable");
        }

        public static void WriteLandingPadVariables(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            if (llvmWriter.landingPadVariablesAreWritten)
            {
                return;
            }

            llvmWriter.landingPadVariablesAreWritten = true;

            writer.Write("%error_object = ");
            writer.Write("alloca i8*, align " + LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);

            writer.Write("%error_typeid = ");
            writer.Write("alloca i32, align " + LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);
        }

        private static void WriteLandingPad(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, LandingPadOptions options, IType[] @catch = null, int[] filter = null, int? exceptionAllocationResultNumber = null)
        {
            llvmWriter.WriteLandingPadVariables(writer);

            var landingPadResult = llvmWriter.WriteSetResultNumber(writer, opCode);

            writer.WriteLine("landingpad { i8*, i32 } personality i8* bitcast (i32 (...)* @__gxx_personality_v0 to i8*)");
            if (options.HasFlag(LandingPadOptions.Cleanup))
            {
                writer.Indent++;
                writer.WriteLine("cleanup");
                writer.Indent--;
            }

            if (options.HasFlag(LandingPadOptions.EmptyFilter))
            {
                writer.Indent++;
                writer.WriteLine("filter [0 x i8*] zeroinitializer");
                writer.Indent--;
            }

            var getErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.WriteLine("extractvalue { i8*, i32 } {0}, 0", llvmWriter.GetResultNumber(landingPadResult));
            writer.WriteLine("store i8* {0}, i8** %.error_object", llvmWriter.GetResultNumber(getErrorObjectResultNumber));
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.WriteLine("extractvalue { i8*, i32 } {0}, 1", llvmWriter.GetResultNumber(landingPadResult));
            writer.WriteLine("store i32 {0}, i32* %.error_typeid", llvmWriter.GetResultNumber(getErrorTypeIdResultNumber));

            if (exceptionAllocationResultNumber.HasValue)
            {
                writer.WriteLine("call void @__cxa_free_exception(i8* {0})", exceptionAllocationResultNumber.Value);
            }

            opCode.ResultNumber = landingPadResult;
            opCode.ResultType = null;
        }
    }
}
