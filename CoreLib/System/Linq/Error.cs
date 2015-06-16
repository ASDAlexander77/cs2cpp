using System;

namespace System.Linq
{
  internal static class Error
  {
    internal static Exception ArgumentNotValid(object p0)
    {
      return (Exception) new ArgumentException(Strings.ArgumentNotValid(p0));
    }

    internal static Exception MoreThanOneElement()
    {
      return (Exception) new InvalidOperationException(Strings.MoreThanOneElement);
    }

    internal static Exception MoreThanOneMatch()
    {
      return (Exception) new InvalidOperationException(Strings.MoreThanOneMatch);
    }

    internal static Exception NoElements()
    {
      return (Exception) new InvalidOperationException(Strings.NoElements);
    }

    internal static Exception NoMatch()
    {
      return (Exception) new InvalidOperationException(Strings.NoMatch);
    }

    internal static Exception ArgumentNull(string paramName)
    {
      return (Exception) new ArgumentNullException(paramName);
    }

    internal static Exception ArgumentOutOfRange(string paramName)
    {
      return (Exception) new ArgumentOutOfRangeException(paramName);
    }

    internal static Exception NotSupported()
    {
      return (Exception) new NotSupportedException();
    }

    internal static Exception ArgumentArrayHasTooManyElements(object p0)
    {
      return (Exception) new ArgumentException(Strings.ArgumentArrayHasTooManyElements(p0));
    }

    internal static Exception ArgumentNotIEnumerableGeneric(object p0)
    {
      return (Exception) new ArgumentException(Strings.ArgumentNotIEnumerableGeneric(p0));
    }

    internal static Exception ArgumentNotSequence(object p0)
    {
      return (Exception) new ArgumentException(Strings.ArgumentNotSequence(p0));
    }

    internal static Exception IncompatibleElementTypes()
    {
      return (Exception) new ArgumentException(Strings.IncompatibleElementTypes);
    }

    internal static Exception ArgumentNotLambda(object p0)
    {
      return (Exception) new ArgumentException(Strings.ArgumentNotLambda(p0));
    }

    internal static Exception NoArgumentMatchingMethodsInQueryable(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.NoArgumentMatchingMethodsInQueryable(p0));
    }

    internal static Exception NoMethodOnType(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.NoMethodOnType(p0, p1));
    }

    internal static Exception NoMethodOnTypeMatchingArguments(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.NoMethodOnTypeMatchingArguments(p0, p1));
    }

    internal static Exception NoNameMatchingMethodsInQueryable(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.NoNameMatchingMethodsInQueryable(p0));
    }

    internal static Exception NotImplemented()
    {
      return (Exception) new NotImplementedException();
    }
  }
}
