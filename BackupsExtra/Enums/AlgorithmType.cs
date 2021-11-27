namespace BackupsExtra.Enums
{
    public enum AlgorithmType
    {
        /// <summary>Filter restore points via count algorithm</summary>
        Count,

        /// <summary>Filter restore points via date algorithm</summary>
        Date,

        /// <summary>Filter restore points via hybrid algorithm</summary>
        Hybrid,
    }
}