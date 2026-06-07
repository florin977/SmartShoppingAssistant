namespace SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs
{
    public class ProductQueryDTO : PaginationQueryDTO
    {
        public string? Search { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public IEnumerable<int>? CategoryIds { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
