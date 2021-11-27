namespace BackupsExtra.Enums
{
    public enum HybridType
    {
        /// <summary>Select point if it doesn't fit at least one limit</summary>
        OneLimit,

        /// <summary>Select point if it doesn't fit all the limits</summary>
        AllLimits,
    }
}