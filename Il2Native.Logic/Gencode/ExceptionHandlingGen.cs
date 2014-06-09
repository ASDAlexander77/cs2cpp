namespace Il2Native.Logic.Gencode
{
    using System;
    using System.CodeDom.Compiler;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

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
            writer.WriteLine("to label %.unreachable unwind label %.unwind_exception");

            llvmWriter.needToWriteUnwindException = true;
        }

        public static void WriteCatchTest(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, IType catchType)
        {
            var opCodeNone = OpCodePart.Nop;
            writer.WriteLine("; Test Exception type");
            var errorTypeIdOfCatchResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("load i32* %error_typeid");
            var errorTypeIdOfExceptionResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.Write("call i32 @llvm.eh.typeid.for(i8* bitcast (");
            catchType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @{0} to i8*))", catchType.GetRttiPointerInfoName());
            var compareResultResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("icmp eq i32 {0}, {1}", llvmWriter.GetResultNumber(errorTypeIdOfCatchResultNumber), llvmWriter.GetResultNumber(errorTypeIdOfExceptionResultNumber));
            writer.WriteLine("br i1 {0}, label %41, label %47", llvmWriter.GetResultNumber(compareResultResultNumber));
        }

        public static void WriteCatchBegin(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, IType catchType)
        {
            writer.WriteLine("; Begin of Catch or Finally");
            writer.WriteLine("%42 = load i8** %2");
            writer.WriteLine("%43 = call i8* @__cxa_begin_catch(i8* %42)");
            writer.WriteLine("%44 = bitcast i8* %43 to %class.Exception*");
            writer.WriteLine("store %class.Exception* %44, %class.Exception** %e1");
        }

        public static void WriteCatchEnd(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.WriteLine("; End of Catch or Finally");
            writer.WriteLine("store i32 0, i32* %1");
            writer.WriteLine("store i32 1, i32* %4");
            writer.WriteLine("call void @__cxa_end_catch() #3");
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

        public static void WriteUnreachable(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine(".unreachable:");
            writer.Indent++;
            writer.WriteLine("unreachable");
        }

        public static void WriteUnwindException(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine(".unwind_exception:");
            writer.Indent++;
            llvmWriter.WriteLandingPad(writer, OpCodePart.Nop, LandingPadOptions.EmptyFilter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("br label %.unexpected");
            llvmWriter.WriteUnexpectedCall(writer);
        }

        public static void WriteUnexpectedCall(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine(".unexpected:");
            writer.Indent++;
            var result = llvmWriter.WriteSetResultNumber(writer, null);
            writer.WriteLine("load i8** %error_object");
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

        public static void WriteLandingPad(
            this LlvmWriter llvmWriter, 
            LlvmIndentedTextWriter writer, 
            OpCodePart opCode, 
            LandingPadOptions options, 
            IType[] @catch = null, 
            int[] filter = null, 
            int? exceptionAllocationResultNumber = null)
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
            writer.WriteLine("extractvalue {1} {0}, 0", llvmWriter.GetResultNumber(landingPadResult), "{ i8*, i32 }");
            writer.WriteLine("store i8* {0}, i8** %.error_object", llvmWriter.GetResultNumber(getErrorObjectResultNumber));
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.WriteLine("extractvalue {1} {0}, 1", llvmWriter.GetResultNumber(landingPadResult), "{ i8*, i32 }");
            writer.Write("store i32 {0}, i32* %.error_typeid", llvmWriter.GetResultNumber(getErrorTypeIdResultNumber));

            if (exceptionAllocationResultNumber.HasValue)
            {
                writer.WriteLine(string.Empty);
                writer.Write("call void @__cxa_free_exception(i8* {0})", exceptionAllocationResultNumber.Value);
            }

            opCode.ResultNumber = landingPadResult;
            opCode.ResultType = null;
        }
    }
}