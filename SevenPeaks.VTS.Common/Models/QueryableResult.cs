namespace SevenPeaks.VTS.Common.Models
{
    public class QueryableResult
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Route { get; set; }
        public string OtherQueryStrings { get; set; }
    }
}
