// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic
{
    using DOM;

    public abstract class CCodeDefinition : CCodeBase
    {
        public abstract bool IsGeneric
        {
            get;
        }

        public bool IsStub { get; set; }
    }
}
