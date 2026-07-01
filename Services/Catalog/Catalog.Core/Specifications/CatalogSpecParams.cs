namespace Catalog.Core.Specifications
{
    public class CatalogSpecParams
    {
        private const int MaxPageSize = 70;
        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // tập hợp các điều kiện filter/search/sort/pagination. Ko nhất thiết mỗi Id
        public string? BrandId { get; set; }
        public string? TypeId { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
    }
}
