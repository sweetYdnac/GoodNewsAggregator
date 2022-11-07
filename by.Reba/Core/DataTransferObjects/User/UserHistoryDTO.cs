namespace by.Reba.Core.DataTransferObjects.User
{
    public class UserHistoryDTO
    {
        public Guid ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public DateTimeOffset LastVisitTime { get; set; }
    }
}
