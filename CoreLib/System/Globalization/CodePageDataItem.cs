namespace System.Globalization
{
    using System;

    //
    // Data item for EncodingTable.  Along with EncodingTable, they are used by 
    // System.Text.Encoding.
    // 
    // This class stores a pointer to the internal data and the index into that data
    // where our required information is found.  We load the code page, flags and uiFamilyCodePage
    // immediately because they don't require creating an object.  Creating any of the string
    // names is delayed until somebody actually asks for them and the names are then cached.

    [Serializable]
    internal class CodePageDataItem
    {
        internal int m_dataIndex;
        internal int m_uiFamilyCodePage;
        internal String m_webName;
        internal String m_headerName;
        internal String m_bodyName;
        internal uint m_flags;

        unsafe internal CodePageDataItem(int dataIndex)
        {
            m_dataIndex = dataIndex;
            m_uiFamilyCodePage = EncodingTable.codePageDataPtr[dataIndex].uiFamilyCodePage;
            m_flags = EncodingTable.codePageDataPtr[dataIndex].flags;
        }

        unsafe public String WebName
        {
            get
            {
                if (m_webName == null)
                {
                    m_webName = EncodingTable.codePageDataPtr[m_dataIndex].Names[0];
                }
                return m_webName;
            }
        }

        public virtual int UIFamilyCodePage
        {
            get
            {
                return m_uiFamilyCodePage;
            }
        }

        unsafe public String HeaderName
        {
            get
            {
                if (m_headerName == null)
                {
                    m_headerName = EncodingTable.codePageDataPtr[m_dataIndex].Names[1];
                }
                return m_headerName;
            }
        }

        unsafe public String BodyName
        {
            get
            {
                if (m_bodyName == null)
                {
                    m_bodyName = EncodingTable.codePageDataPtr[m_dataIndex].Names[2];
                }
                return m_bodyName;
            }
        }

        unsafe public uint Flags
        {
            get
            {
                return (m_flags);
            }
        }
    }
}
