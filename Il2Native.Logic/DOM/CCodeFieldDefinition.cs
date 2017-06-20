// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Diagnostics;

    using Microsoft.CodeAnalysis;

    public class CCodeFieldDefinition : CCodeDefinition
    {
        public CCodeFieldDefinition(IFieldSymbol field)
        {
            this.Field = field;
        }

        public bool DoNotWrapStatic { get; set; }

        public IFieldSymbol Field { get; set; }

        public override bool IsTemplate
        {
            get { return this.Field.ContainingType.IsGenericType; }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteFieldDefinition(this.Field, this.DoNotWrapStatic);
            c.EndStatement();
        }
    }
}
