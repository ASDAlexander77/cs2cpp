using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Linq
{
  internal sealed class SR
  {
    private static System.Linq.SR loader;
    private ResourceManager resources;
    internal const string OwningTeam = "OwningTeam";
    internal const string ArgumentArrayHasTooManyElements = "ArgumentArrayHasTooManyElements";
    internal const string ArgumentNotIEnumerableGeneric = "ArgumentNotIEnumerableGeneric";
    internal const string ArgumentNotSequence = "ArgumentNotSequence";
    internal const string ArgumentNotValid = "ArgumentNotValid";
    internal const string IncompatibleElementTypes = "IncompatibleElementTypes";
    internal const string ArgumentNotLambda = "ArgumentNotLambda";
    internal const string MoreThanOneElement = "MoreThanOneElement";
    internal const string MoreThanOneMatch = "MoreThanOneMatch";
    internal const string NoArgumentMatchingMethodsInQueryable = "NoArgumentMatchingMethodsInQueryable";
    internal const string NoElements = "NoElements";
    internal const string NoMatch = "NoMatch";
    internal const string NoMethodOnType = "NoMethodOnType";
    internal const string NoMethodOnTypeMatchingArguments = "NoMethodOnTypeMatchingArguments";
    internal const string NoNameMatchingMethodsInQueryable = "NoNameMatchingMethodsInQueryable";
    internal const string EmptyEnumerable = "EmptyEnumerable";
    internal const string Argument_AdjustmentRulesNoNulls = "Argument_AdjustmentRulesNoNulls";
    internal const string Argument_AdjustmentRulesOutOfOrder = "Argument_AdjustmentRulesOutOfOrder";
    internal const string Argument_AdjustmentRulesAmbiguousOverlap = "Argument_AdjustmentRulesAmbiguousOverlap";
    internal const string Argument_AdjustmentRulesrDaylightSavingTimeOverlap = "Argument_AdjustmentRulesrDaylightSavingTimeOverlap";
    internal const string Argument_AdjustmentRulesrDaylightSavingTimeOverlapNonRuleRange = "Argument_AdjustmentRulesrDaylightSavingTimeOverlapNonRuleRange";
    internal const string Argument_AdjustmentRulesInvalidOverlap = "Argument_AdjustmentRulesInvalidOverlap";
    internal const string Argument_ConvertMismatch = "Argument_ConvertMismatch";
    internal const string Argument_DateTimeHasTimeOfDay = "Argument_DateTimeHasTimeOfDay";
    internal const string Argument_DateTimeIsInvalid = "Argument_DateTimeIsInvalid";
    internal const string Argument_DateTimeIsNotAmbiguous = "Argument_DateTimeIsNotAmbiguous";
    internal const string Argument_DateTimeOffsetIsNotAmbiguous = "Argument_DateTimeOffsetIsNotAmbiguous";
    internal const string Argument_DateTimeKindMustBeUnspecified = "Argument_DateTimeKindMustBeUnspecified";
    internal const string Argument_DateTimeHasTicks = "Argument_DateTimeHasTicks";
    internal const string Argument_InvalidId = "Argument_InvalidId";
    internal const string Argument_InvalidSerializedString = "Argument_InvalidSerializedString";
    internal const string Argument_InvalidREG_TZI_FORMAT = "Argument_InvalidREG_TZI_FORMAT";
    internal const string Argument_OutOfOrderDateTimes = "Argument_OutOfOrderDateTimes";
    internal const string Argument_TimeSpanHasSeconds = "Argument_TimeSpanHasSeconds";
    internal const string Argument_TimeZoneInfoBadTZif = "Argument_TimeZoneInfoBadTZif";
    internal const string Argument_TimeZoneInfoInvalidTZif = "Argument_TimeZoneInfoInvalidTZif";
    internal const string Argument_TransitionTimesAreIdentical = "Argument_TransitionTimesAreIdentical";
    internal const string ArgumentOutOfRange_DayParam = "ArgumentOutOfRange_DayParam";
    internal const string ArgumentOutOfRange_DayOfWeek = "ArgumentOutOfRange_DayOfWeek";
    internal const string ArgumentOutOfRange_MonthParam = "ArgumentOutOfRange_MonthParam";
    internal const string ArgumentOutOfRange_UtcOffset = "ArgumentOutOfRange_UtcOffset";
    internal const string ArgumentOutOfRange_UtcOffsetAndDaylightDelta = "ArgumentOutOfRange_UtcOffsetAndDaylightDelta";
    internal const string ArgumentOutOfRange_Week = "ArgumentOutOfRange_Week";
    internal const string InvalidTimeZone_InvalidRegistryData = "InvalidTimeZone_InvalidRegistryData";
    internal const string InvalidTimeZone_InvalidWin32APIData = "InvalidTimeZone_InvalidWin32APIData";
    internal const string Security_CannotReadRegistryData = "Security_CannotReadRegistryData";
    internal const string Serialization_CorruptField = "Serialization_CorruptField";
    internal const string Serialization_InvalidEscapeSequence = "Serialization_InvalidEscapeSequence";
    internal const string TimeZoneNotFound_MissingRegistryData = "TimeZoneNotFound_MissingRegistryData";
    internal const string ArgumentOutOfRange_DateTimeBadTicks = "ArgumentOutOfRange_DateTimeBadTicks";
    internal const string PLINQ_CommonEnumerator_Current_NotStarted = "PLINQ_CommonEnumerator_Current_NotStarted";
    internal const string PLINQ_ExternalCancellationRequested = "PLINQ_ExternalCancellationRequested";
    internal const string PLINQ_DisposeRequested = "PLINQ_DisposeRequested";
    internal const string PLINQ_EnumerationPreviouslyFailed = "PLINQ_EnumerationPreviouslyFailed";
    internal const string ParallelPartitionable_NullReturn = "ParallelPartitionable_NullReturn";
    internal const string ParallelPartitionable_NullElement = "ParallelPartitionable_NullElement";
    internal const string ParallelPartitionable_IncorretElementCount = "ParallelPartitionable_IncorretElementCount";
    internal const string ParallelEnumerable_ToArray_DimensionRequired = "ParallelEnumerable_ToArray_DimensionRequired";
    internal const string ParallelEnumerable_WithQueryExecutionMode_InvalidMode = "ParallelEnumerable_WithQueryExecutionMode_InvalidMode";
    internal const string ParallelEnumerable_WithMergeOptions_InvalidOptions = "ParallelEnumerable_WithMergeOptions_InvalidOptions";
    internal const string ParallelEnumerable_BinaryOpMustUseAsParallel = "ParallelEnumerable_BinaryOpMustUseAsParallel";
    internal const string ParallelEnumerable_WithCancellation_TokenSourceDisposed = "ParallelEnumerable_WithCancellation_TokenSourceDisposed";
    internal const string ParallelQuery_InvalidAsOrderedCall = "ParallelQuery_InvalidAsOrderedCall";
    internal const string ParallelQuery_InvalidNonGenericAsOrderedCall = "ParallelQuery_InvalidNonGenericAsOrderedCall";
    internal const string ParallelQuery_PartitionerNotOrderable = "ParallelQuery_PartitionerNotOrderable";
    internal const string ParallelQuery_DuplicateTaskScheduler = "ParallelQuery_DuplicateTaskScheduler";
    internal const string ParallelQuery_DuplicateDOP = "ParallelQuery_DuplicateDOP";
    internal const string ParallelQuery_DuplicateWithCancellation = "ParallelQuery_DuplicateWithCancellation";
    internal const string ParallelQuery_DuplicateExecutionMode = "ParallelQuery_DuplicateExecutionMode";
    internal const string ParallelQuery_DuplicateMergeOptions = "ParallelQuery_DuplicateMergeOptions";
    internal const string PartitionerQueryOperator_NullPartitionList = "PartitionerQueryOperator_NullPartitionList";
    internal const string PartitionerQueryOperator_WrongNumberOfPartitions = "PartitionerQueryOperator_WrongNumberOfPartitions";
    internal const string PartitionerQueryOperator_NullPartition = "PartitionerQueryOperator_NullPartition";
    internal const string event_ParallelQueryBegin = "event_ParallelQueryBegin";
    internal const string event_ParallelQueryEnd = "event_ParallelQueryEnd";
    internal const string event_ParallelQueryFork = "event_ParallelQueryFork";
    internal const string event_ParallelQueryJoin = "event_ParallelQueryJoin";

    static CultureInfo Culture
    {
      get
      {
        return (CultureInfo) null;
      }
    }

    public static ResourceManager Resources
    {
      get
      {
        return System.Linq.SR.GetLoader().resources;
      }
    }

    static SR()
    {
    }

    internal SR()
    {
      this.resources = new ResourceManager("System.Linq", this.GetType().Assembly);
    }

    public static string GetString(string name)
    {
      System.Linq.SR loader = System.Linq.SR.GetLoader();
      if (loader == null)
        return (string) null;
      else
        return loader.resources.GetString(name, System.Linq.SR.Culture);
    }

    private static System.Linq.SR GetLoader()
    {
      if (System.Linq.SR.loader == null)
      {
        System.Linq.SR sr = new System.Linq.SR();
        Interlocked.CompareExchange<System.Linq.SR>(ref System.Linq.SR.loader, sr, (System.Linq.SR) null);
      }
      return System.Linq.SR.loader;
    }

    public static string GetString(string name, params object[] args)
    {
      System.Linq.SR loader = System.Linq.SR.GetLoader();
      if (loader == null)
        return (string) null;
      string @string = loader.resources.GetString(name, System.Linq.SR.Culture);
      if (args == null || args.Length <= 0)
        return @string;
      for (int index = 0; index < args.Length; ++index)
      {
        string str = args[index] as string;
        if (str != null && str.Length > 1024)
          args[index] = (object) (str.Substring(0, 1021) + "...");
      }
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, @string, args);
    }

    public static string GetString(string name, out bool usedFallback)
    {
      usedFallback = false;
      return System.Linq.SR.GetString(name);
    }

    public static object GetObject(string name)
    {
      System.Linq.SR loader = System.Linq.SR.GetLoader();
      if (loader == null)
        return (object) null;
      else
        return loader.resources.GetObject(name, System.Linq.SR.Culture);
    }
  }
}
