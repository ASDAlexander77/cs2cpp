namespace System.Linq.Expressions {

    public abstract class LambdaExpression : Expression {
    }

    public sealed class Expression<TDelegate> : LambdaExpression {
    }

    public partial class Expression {
    }
}
