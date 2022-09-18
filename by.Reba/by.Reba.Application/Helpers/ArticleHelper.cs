namespace by.Reba.Application.Helpers
{
    public static class ArticleHelper
    {
        public static string GetPublicationDateFormatted(DateTime publicationDate)
        {
            var time = DateTime.Now - publicationDate;

            return time.Days switch
            {
                > 2 => publicationDate.ToString("f"),
                > 1 => $"Вчера в {publicationDate.ToString("t")}",
                < 1 => $"Сегодня в {publicationDate.ToString("t")}",
                _ => time.Hours < 10 ? $"{time.Hours} час. назад" : $"{time.Minutes} мин. назад",
            };
        }
    }
}
