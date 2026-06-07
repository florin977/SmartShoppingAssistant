namespace SmartShoppingAssistant.DataAccess.Repository.Parameters
{
    public class ProductQueryParameters : PaginationParameters
    {
        public string? Search { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public IEnumerable<int>? CategoryIds { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
