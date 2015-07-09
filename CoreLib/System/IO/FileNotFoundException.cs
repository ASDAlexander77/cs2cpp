// Licensed under the MIT license.

/*============================================================
**
** 
** 
** 
**
**
** Purpose: Exception for accessing a file that doesn't exist.
**
**
===========================================================*/

using System;
using System.Security.Permissions;
using SecurityException = System.Security.SecurityException;
using System.Globalization;

namespace System.IO {
    // Thrown when trying to access a file that doesn't exist on disk.
    [Serializable]
[System.Runtime.InteropServices.ComVisible(true)]
    public class FileNotFoundException : IOException {

        private String _fileName;  // The name of the file that isn't found.
        private String _fusionLog;  // fusion log (when applicable)
        
        public FileNotFoundException() 
            : base(Environment.GetResourceString("IO.FileNotFound")) {
        }
    
        public FileNotFoundException(String message) 
            : base(message) {
        }
    
        public FileNotFoundException(String message, Exception innerException) 
            : base(message, innerException) {
        }

        public FileNotFoundException(String message, String fileName) : base(message)
        {
            _fileName = fileName;
        }

        public FileNotFoundException(String message, String fileName, Exception innerException) 
            : base(message, innerException) {
            _fileName = fileName;
        }

        public override String Message
        {
            get {
                SetMessageField();
                return _message;
            }
        }

        private void SetMessageField()
        {
            _message = Environment.GetResourceString("IO.FileNotFound");
        }

        public String FileName {
            get { return _fileName; }
        }

        public override String ToString()
        {
            String s = GetType().FullName + ": " + Message;

            if (_fileName != null && _fileName.Length != 0)
                s += Environment.NewLine + Environment.GetResourceString("IO.FileName_Name", _fileName);
            
            if (InnerException != null)
                s = s + " ---> " + InnerException.ToString();

            if (StackTrace != null)
                s += Environment.NewLine + StackTrace;

#if FEATURE_FUSION            
            try
            {
                if(FusionLog!=null)
                {
                    if (s==null)
                        s=" ";
                    s+=Environment.NewLine;
                    s+=Environment.NewLine;
                    s+=FusionLog;
                }
            }
            catch(SecurityException)
            {
            
            }
#endif            
            return s;
            
        }

        private FileNotFoundException(String fileName, String fusionLog,int hResult)
            : base(null)
        {
            _fileName = fileName;
            _fusionLog=fusionLog;
            SetMessageField();
        }

#if FEATURE_FUSION
        public String FusionLog {
            [System.Security.SecuritySafeCritical]  // auto-generated
            #if PROTECTION
[SecurityPermissionAttribute( SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
#endif
            get { return _fusionLog; }
        }
#endif

#if FEATURE_SERIALIZATION
        [System.Security.SecurityCritical]  // auto-generated_required
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            // Serialize data for our base classes.  base will verify info != null.
            base.GetObjectData(info, context);

            // Serialize data for this class
            info.AddValue("FileNotFound_FileName", _fileName, typeof(String));

            try
            {
                info.AddValue("FileNotFound_FusionLog", FusionLog, typeof(String));
            }
            catch (SecurityException)
            {
            }
        }
#endif
    }
}

