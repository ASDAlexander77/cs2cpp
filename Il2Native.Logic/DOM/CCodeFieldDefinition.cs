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

        public override bool IsGeneric
        {
            get { return this.Field.ContainingType.IsGenericType; }
        }

        public override void WriteTo(IndentedTextWriter itw)
        {
            CCodeSerializer.WriteFieldDefinition(itw, this.Field);
            itw.WriteLine(";");
        }
    }
}
