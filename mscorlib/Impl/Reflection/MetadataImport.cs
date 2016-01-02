namespace System.Reflection
{
    internal partial struct MetadataImport
    {
        private static void _GetMarshalAs(
            IntPtr pNativeType,
            int cNativeType,
            out int unmanagedType,
            out int safeArraySubType,
            out string safeArrayUserDefinedSubType,
            out int arraySubType,
            out int sizeParamIndex,
            out int sizeConst,
            out string marshalType,
            out string marshalCookie,
            out int iidParamIndex)
        {
            unmanagedType = 0;
            safeArraySubType = 0;
            safeArrayUserDefinedSubType = null;
            arraySubType = 0;
            sizeParamIndex = 0;
            sizeConst = 0;
            marshalType = null;
            marshalCookie = null;
            iidParamIndex = 0;
        }

        private unsafe static void _Enum(IntPtr scope, int type, int parent, out MetadataEnumResult result)
        {
            result = new MetadataEnumResult();
        }

        private static String _GetDefaultValue(
            IntPtr scope,
            int mdToken,
            out long value,
            out int length,
            out int corElementType)
        {
            value = 0;
            length = 0;
            corElementType = 0;
            return null;
        }

        private static unsafe void _GetUserString(IntPtr scope, int mdToken, void** name, out int length)
        {
            length = 0;
        }

        private static unsafe void _GetName(IntPtr scope, int mdToken, void** name)
        {
        }

        private static unsafe void _GetNamespace(IntPtr scope, int mdToken, void** namesp)
        {
        }

        private unsafe static void _GetEventProps(
            IntPtr scope,
            int mdToken,
            void** name,
            out int eventAttributes)
        {
            eventAttributes = 0;
        }

        private static void _GetFieldDefProps(IntPtr scope, int mdToken, out int fieldAttributes)
        {
            fieldAttributes = 0;
        }

        private unsafe static void _GetPropertyProps(
            IntPtr scope,
            int mdToken,
            void** name,
            out int propertyAttributes,
            out ConstArray signature)
        {
            propertyAttributes = 0;
            signature = new ConstArray();
        }

        private static void _GetParentToken(
            IntPtr scope,
            int mdToken,
            out int tkParent)
        {
            tkParent = 0;
        }

        private static void _GetParamDefProps(
            IntPtr scope,
            int parameterToken,
            out int sequence,
            out int attributes)
        {
            sequence = 0;
            attributes = 0;
        }

        private static void _GetGenericParamProps(
            IntPtr scope,
            int genericParameter,
            out int flags)
        {
            flags = 0;
        }

        private static void _GetScopeProps(
            IntPtr scope,
            out Guid mvid)
        {
            mvid = Guid.Empty;
        }

        private static void _GetSigOfMethodDef(
            IntPtr scope,
            int methodToken,
            ref ConstArray signature)
        {
        }

        private static void _GetSignatureFromToken(
            IntPtr scope,
            int methodToken,
            ref ConstArray signature)
        {
        }

        private static void _GetMemberRefProps(
            IntPtr scope,
            int memberTokenRef,
            out ConstArray signature)
        {
            signature = new ConstArray();
        }

        private static void _GetCustomAttributeProps(
            IntPtr scope,
            int customAttributeToken,
            out int constructorToken,
            out ConstArray signature)
        {
            constructorToken = 0;
            signature = new ConstArray();
        }

        private static void _GetClassLayout(
            IntPtr scope,
            int typeTokenDef,
            out int packSize,
            out int classSize)
        {
            packSize = 0;
            classSize = 0;
        }

        private static bool _GetFieldOffset(
            IntPtr scope,
            int typeTokenDef,
            int fieldTokenDef,
            out int offset)
        {
            offset = 0;
            return false;
        }

        private static void _GetSigOfFieldDef(
            IntPtr scope,
            int fieldToken,
            ref ConstArray fieldMarshal)
        {
        }

        private static void _GetFieldMarshal(
            IntPtr scope,
            int fieldToken,
            ref ConstArray fieldMarshal)
        {
        }

        private unsafe static void _GetPInvokeMap(
            IntPtr scope,
            int token,
            out int attributes,
            void** importName,
            void** importDll)
        {
            attributes = 0;
        }

        private static bool _IsValidToken(IntPtr scope, int token)
        {
            return false;
        }
    }
}


