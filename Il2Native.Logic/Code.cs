// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Code.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    /// <summary>
    /// </summary>
    public enum Code
    {
        /// <summary>
        /// </summary>
        Nop = 0, // 0
        /// <summary>
        /// </summary>
        Break, 

        /// <summary>
        /// </summary>
        Ldarg_0, 

        /// <summary>
        /// </summary>
        Ldarg_1, 

        /// <summary>
        /// </summary>
        Ldarg_2, 

        /// <summary>
        /// </summary>
        Ldarg_3, 

        /// <summary>
        /// </summary>
        Ldloc_0, 

        /// <summary>
        /// </summary>
        Ldloc_1, 

        /// <summary>
        /// </summary>
        Ldloc_2, 

        /// <summary>
        /// </summary>
        Ldloc_3, 

        /// <summary>
        /// </summary>
        Stloc_0, // 10
        /// <summary>
        /// </summary>
        Stloc_1, 

        /// <summary>
        /// </summary>
        Stloc_2, 

        /// <summary>
        /// </summary>
        Stloc_3, 

        /// <summary>
        /// </summary>
        Ldarg_S, 

        /// <summary>
        /// </summary>
        Ldarga_S, 

        /// <summary>
        /// </summary>
        Starg_S, 

        /// <summary>
        /// </summary>
        Ldloc_S, 

        /// <summary>
        /// </summary>
        Ldloca_S, 

        /// <summary>
        /// </summary>
        Stloc_S, 

        /// <summary>
        /// </summary>
        Ldnull, // 20
        /// <summary>
        /// </summary>
        Ldc_I4_M1, 

        /// <summary>
        /// </summary>
        Ldc_I4_0, 

        /// <summary>
        /// </summary>
        Ldc_I4_1, 

        /// <summary>
        /// </summary>
        Ldc_I4_2, 

        /// <summary>
        /// </summary>
        Ldc_I4_3, 

        /// <summary>
        /// </summary>
        Ldc_I4_4, 

        /// <summary>
        /// </summary>
        Ldc_I4_5, 

        /// <summary>
        /// </summary>
        Ldc_I4_6, 

        /// <summary>
        /// </summary>
        Ldc_I4_7, 

        /// <summary>
        /// </summary>
        Ldc_I4_8, // 30
        /// <summary>
        /// </summary>
        Ldc_I4_S, 

        /// <summary>
        /// </summary>
        Ldc_I4, 

        /// <summary>
        /// </summary>
        Ldc_I8, 

        /// <summary>
        /// </summary>
        Ldc_R4, 

        /// <summary>
        /// </summary>
        Ldc_R8, 

        /// <summary>
        /// </summary>
        UNKNOWN, // TODO: 35, here should be something different
        /// <summary>
        /// </summary>
        Dup, // 36
        /// <summary>
        /// </summary>
        Pop, 

        /// <summary>
        /// </summary>
        Jmp, 

        /// <summary>
        /// </summary>
        Call, 

        /// <summary>
        /// </summary>
        Calli, // 40
        /// <summary>
        /// </summary>
        Ret, 

        /// <summary>
        /// </summary>
        Br_S, 

        /// <summary>
        /// </summary>
        Brfalse_S, 

        /// <summary>
        /// </summary>
        Brtrue_S, 

        /// <summary>
        /// </summary>
        Beq_S, 

        /// <summary>
        /// </summary>
        Bge_S, 

        /// <summary>
        /// </summary>
        Bgt_S, 

        /// <summary>
        /// </summary>
        Ble_S, 

        /// <summary>
        /// </summary>
        Blt_S, 

        /// <summary>
        /// </summary>
        Bne_Un_S, 

        /// <summary>
        /// </summary>
        Bge_Un_S, 

        /// <summary>
        /// </summary>
        Bgt_Un_S, 

        /// <summary>
        /// </summary>
        Ble_Un_S, 

        /// <summary>
        /// </summary>
        Blt_Un_S, 

        /// <summary>
        /// </summary>
        Br, 

        /// <summary>
        /// </summary>
        Brfalse, 

        /// <summary>
        /// </summary>
        Brtrue, 

        /// <summary>
        /// </summary>
        Beq, 

        /// <summary>
        /// </summary>
        Bge, 

        /// <summary>
        /// </summary>
        Bgt, 

        /// <summary>
        /// </summary>
        Ble, 

        /// <summary>
        /// </summary>
        Blt, 

        /// <summary>
        /// </summary>
        Bne_Un, 

        /// <summary>
        /// </summary>
        Bge_Un, 

        /// <summary>
        /// </summary>
        Bgt_Un, 

        /// <summary>
        /// </summary>
        Ble_Un, 

        /// <summary>
        /// </summary>
        Blt_Un, 

        /// <summary>
        /// </summary>
        Switch, 

        /// <summary>
        /// </summary>
        Ldind_I1, 

        /// <summary>
        /// </summary>
        Ldind_U1, 

        /// <summary>
        /// </summary>
        Ldind_I2, 

        /// <summary>
        /// </summary>
        Ldind_U2, 

        /// <summary>
        /// </summary>
        Ldind_I4, 

        /// <summary>
        /// </summary>
        Ldind_U4, 

        /// <summary>
        /// </summary>
        Ldind_I8, 

        /// <summary>
        /// </summary>
        Ldind_I, 

        /// <summary>
        /// </summary>
        Ldind_R4, 

        /// <summary>
        /// </summary>
        Ldind_R8, 

        /// <summary>
        /// </summary>
        Ldind_Ref, 

        /// <summary>
        /// </summary>
        Stind_Ref, 

        /// <summary>
        /// </summary>
        Stind_I1, 

        /// <summary>
        /// </summary>
        Stind_I2, 

        /// <summary>
        /// </summary>
        Stind_I4, 

        /// <summary>
        /// </summary>
        Stind_I8, 

        /// <summary>
        /// </summary>
        Stind_R4, 

        /// <summary>
        /// </summary>
        Stind_R8, 

        /// <summary>
        /// </summary>
        Add, 

        /// <summary>
        /// </summary>
        Sub, 

        /// <summary>
        /// </summary>
        Mul, 

        /// <summary>
        /// </summary>
        Div, 

        /// <summary>
        /// </summary>
        Div_Un, 

        /// <summary>
        /// </summary>
        Rem, 

        /// <summary>
        /// </summary>
        Rem_Un, 

        /// <summary>
        /// </summary>
        And, 

        /// <summary>
        /// </summary>
        Or, 

        /// <summary>
        /// </summary>
        Xor, 

        /// <summary>
        /// </summary>
        Shl, 

        /// <summary>
        /// </summary>
        Shr, 

        /// <summary>
        /// </summary>
        Shr_Un, 

        /// <summary>
        /// </summary>
        Neg, 

        /// <summary>
        /// </summary>
        Not, 

        /// <summary>
        /// </summary>
        Conv_I1, 

        /// <summary>
        /// </summary>
        Conv_I2, 

        /// <summary>
        /// </summary>
        Conv_I4, 

        /// <summary>
        /// </summary>
        Conv_I8, 

        /// <summary>
        /// </summary>
        Conv_R4, 

        /// <summary>
        /// </summary>
        Conv_R8, 

        /// <summary>
        /// </summary>
        Conv_U4, 

        /// <summary>
        /// </summary>
        Conv_U8, 

        /// <summary>
        /// </summary>
        Callvirt = 0x6F, 

        /// <summary>
        /// </summary>
        Cpobj, 

        /// <summary>
        /// </summary>
        Ldobj, 

        /// <summary>
        /// </summary>
        Ldstr, 

        /// <summary>
        /// </summary>
        Newobj, 

        /// <summary>
        /// </summary>
        Castclass = 0x74, 

        /// <summary>
        /// </summary>
        Isinst, 

        /// <summary>
        /// </summary>
        Conv_R_Un, 

        /// <summary>
        /// </summary>
        Unbox = 0x79, 

        /// <summary>
        /// </summary>
        Throw = 0x7A, 

        /// <summary>
        /// </summary>
        Ldfld = 0x7B, 

        /// <summary>
        /// </summary>
        Ldflda = 0x7C, 

        /// <summary>
        /// </summary>
        Stfld = 0x7D, 

        /// <summary>
        /// </summary>
        Ldsfld = 0x7E, 

        /// <summary>
        /// </summary>
        Ldsflda = 0x7F, 

        /// <summary>
        /// </summary>
        Stsfld = 0x80, 

        /// <summary>
        /// </summary>
        Stobj, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I1_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I2_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I4_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I8_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U1_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U2_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U4_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U8_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I_Un, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U_Un, 

        /// <summary>
        /// </summary>
        Box = 0x8C, 

        /// <summary>
        /// </summary>
        Newarr, 

        /// <summary>
        /// </summary>
        Ldlen, 

        /// <summary>
        /// </summary>
        Ldelema, 

        /// <summary>
        /// </summary>
        Ldelem_I1, 

        /// <summary>
        /// </summary>
        Ldelem_U1, 

        /// <summary>
        /// </summary>
        Ldelem_I2, 

        /// <summary>
        /// </summary>
        Ldelem_U2, 

        /// <summary>
        /// </summary>
        Ldelem_I4, 

        /// <summary>
        /// </summary>
        Ldelem_U4, 

        /// <summary>
        /// </summary>
        Ldelem_I8, 

        /// <summary>
        /// </summary>
        Ldelem_I, 

        /// <summary>
        /// </summary>
        Ldelem_R4, 

        /// <summary>
        /// </summary>
        Ldelem_R8, 

        /// <summary>
        /// </summary>
        Ldelem_Ref, 

        /// <summary>
        /// </summary>
        Stelem_I, 

        /// <summary>
        /// </summary>
        Stelem_I1, 

        /// <summary>
        /// </summary>
        Stelem_I2, 

        /// <summary>
        /// </summary>
        Stelem_I4, 

        /// <summary>
        /// </summary>
        Stelem_I8, 

        /// <summary>
        /// </summary>
        Stelem_R4, 

        /// <summary>
        /// </summary>
        Stelem_R8, 

        /// <summary>
        /// </summary>
        Stelem_Ref = 0xA2, 

        /// <summary>
        /// </summary>
        Ldelem, 

        /// <summary>
        /// </summary>
        Stelem, 

        /// <summary>
        /// </summary>
        Unbox_Any = 0xA5, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I1 = 0xB3, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U1, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I2, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U2, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I4, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U4, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I8, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U8, 

        /// <summary>
        /// </summary>
        Refanyval = 0xC2, 

        /// <summary>
        /// </summary>
        Ckfinite, 

        /// <summary>
        /// </summary>
        Mkrefany = 0xC6, 

        /// <summary>
        /// </summary>
        Ldtoken = 0xD0, 

        /// <summary>
        /// </summary>
        Conv_U2, 

        /// <summary>
        /// </summary>
        Conv_U1, 

        /// <summary>
        /// </summary>
        Conv_I, 

        /// <summary>
        /// </summary>
        Conv_Ovf_I, 

        /// <summary>
        /// </summary>
        Conv_Ovf_U, 

        /// <summary>
        /// </summary>
        Add_Ovf, 

        /// <summary>
        /// </summary>
        Add_Ovf_Un, 

        /// <summary>
        /// </summary>
        Mul_Ovf, 

        /// <summary>
        /// </summary>
        Mul_Ovf_Un, 

        /// <summary>
        /// </summary>
        Sub_Ovf, 

        /// <summary>
        /// </summary>
        Sub_Ovf_Un, 

        /// <summary>
        /// </summary>
        Endfinally, 

        /// <summary>
        /// </summary>
        Leave, 

        /// <summary>
        /// </summary>
        Leave_S, 

        /// <summary>
        /// </summary>
        Stind_I, 

        /// <summary>
        /// </summary>
        Conv_U, 

        /// <summary>
        /// </summary>
        Arglist = 0xE1, 

        /// <summary>
        /// </summary>
        Ceq, 

        /// <summary>
        /// </summary>
        Cgt, 

        /// <summary>
        /// </summary>
        Cgt_Un, 

        /// <summary>
        /// </summary>
        Clt, 

        /// <summary>
        /// </summary>
        Clt_Un, 

        /// <summary>
        /// </summary>
        Ldftn, 

        /// <summary>
        /// </summary>
        Ldvirtftn, 

        /// <summary>
        /// </summary>
        Ldarg, 

        /// <summary>
        /// </summary>
        Ldarga, 

        /// <summary>
        /// </summary>
        Starg, 

        /// <summary>
        /// </summary>
        Ldloc, 

        /// <summary>
        /// </summary>
        Ldloca, 

        /// <summary>
        /// </summary>
        Stloc, 

        /// <summary>
        /// </summary>
        Localloc = 0xF0, 

        /// <summary>
        /// </summary>
        Endfilter = 0xF2, 

        /// <summary>
        /// </summary>
        Unaligned = 0xF3, 

        /// <summary>
        /// </summary>
        Volatile = 0xF4, 

        /// <summary>
        /// </summary>
        Tail, 

        /// <summary>
        /// </summary>
        Initobj = 0xF6, 

        /// <summary>
        /// </summary>
        Constrained, 

        /// <summary>
        /// </summary>
        Cpblk, 

        /// <summary>
        /// </summary>
        Initblk, 

        /// <summary>
        /// </summary>
        No, 

        /// <summary>
        /// </summary>
        Rethrow, 

        /// <summary>
        /// </summary>
        Sizeof = 0xFD, 

        /// <summary>
        /// </summary>
        Refanytype = 0xFE, 

        /// <summary>
        /// </summary>
        Readonly = 0xFF, 
    }
}