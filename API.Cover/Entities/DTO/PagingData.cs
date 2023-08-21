namespace API.Cover.Entities.DTO
{
    public class PagingData<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// 
        /// </summary>
        public long TotalCount { get; set; }
    }
}
