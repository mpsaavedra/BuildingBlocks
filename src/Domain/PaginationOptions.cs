namespace Orun.BuildingBlocks.Domain
{
    /// <summary>
    /// Options for ordering and pagination of the query result
    /// </summary>
    public class PaginationOptions
    {
        /// <summary>
        /// Ordering direction, default is Ascending,
        /// Default: true
        /// </summary>
        public bool Ascending { get; set; } = true;

        /// <summary>
        /// Index of page to display
        /// Default: 1
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// Size of the result page
        /// Default: 20
        /// </summary>
        public int Size { get; set; } = 20;
    }
}