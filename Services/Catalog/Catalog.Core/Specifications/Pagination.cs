namespace Catalog.Core.Specifications
{
    public class Pagination<T> where T : class // chỉ reference type (các class còn value type như int, double, bool, struct thì không được)
    {
        public Pagination() { }
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyCollection<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int Count { get; private set; }
        public IReadOnlyCollection<T> Data { get; private set; }

        public void SetPageIndex(int pageIndex)
        {
            PageIndex = pageIndex;
        }

        public void SetPageSize(int pageSize)
        {
            PageSize = pageSize;
        }

        public void SetCount(int count)
        {
            Count = count;
        }

        public void SetData(IReadOnlyCollection<T> data)
        {
            Data = data;
        }
    }
}
