﻿// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.CSharp.Symbols
{
    /// <summary>
    /// Information decoded from well-known custom attributes applied on a type.
    /// </summary>
    internal sealed class TypeWellKnownAttributeData : CommonTypeWellKnownAttributeData
    {
        #region CoClassAttribute

        private NamedTypeSymbol comImportCoClass;
        public NamedTypeSymbol ComImportCoClass
        {
            get
            {
                return this.comImportCoClass;
            }
            set
            {
                VerifySealed(expected: false);
                Debug.Assert((object)this.comImportCoClass == null);
                Debug.Assert((object)value != null);
                this.comImportCoClass = value;
                SetDataStored();
            }
        }

        #endregion
    }
}
