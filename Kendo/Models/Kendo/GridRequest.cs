namespace Kendo.Models.Kendo
{
    public class DataSourceRequest
    {
        public int take     { get; set; }
        public int skip     { get; set; }
        public int pageSize { get; set; }
        public int page     { get; set; }
        public KendoSort[] sort { get; set; }

    }
    public class KendoSort
    {
        public string field { get; set; }
        public string dir { get; set; }
    }
}