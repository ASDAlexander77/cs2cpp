// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//============================================================
//
// 
//
//  Purpose: Exception for accessing a drive that is not available.
//
//
//============================================================
using System;

namespace System.IO {

    //Thrown when trying to access a drive that is not availabe.
    [Serializable]
[System.Runtime.InteropServices.ComVisible(true)]
    public class DriveNotFoundException : IOException {
        public DriveNotFoundException() 
            : base(Environment.GetResourceString("Arg_DriveNotFoundException")) {
        }
    
        public DriveNotFoundException(String message) 
            : base(message) {
        }
    
        public DriveNotFoundException(String message, Exception innerException) 
            : base(message, innerException) {
        }
    }
}
