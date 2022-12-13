using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;

namespace by.Reba.Application.TagHelpers
{
    public class DateFormatterTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public DateTime Date { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            var time = DateTime.Now - Date;

            var formated = Date.Date.AddDays(1) == DateTime.Now.Date
                ? $"Вчера в {Date:HH:mm}"
                : Date.Date == DateTime.Now.Date
                    ? time.Hours > 10
                        ? $"Сегодня в {Date:HH:mm}"
                        : time.Hours >= 1 
                            ? $"{time.Hours} час. назад" 
                            : time.Minutes >= 1 
                                ? $"{time.Minutes} мин. назад" 
                                : $"Только что"
                    : Date.ToString("dd MMMM, HH:mm", new CultureInfo("ru-RU"));

            output.Content.AppendHtml(formated);
        }
    }
}
