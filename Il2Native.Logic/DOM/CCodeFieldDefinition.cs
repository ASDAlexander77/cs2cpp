namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public class CCodeFieldDefinition : CCodeDefinition
    {
        public CCodeFieldDefinition(IFieldSymbol field)
        {
            this.Field = field;
        }

        public IFieldSymbol Field { get; set; }

        public bool DoNotWrapStatic { get; set; }

        public override bool IsGeneric
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
