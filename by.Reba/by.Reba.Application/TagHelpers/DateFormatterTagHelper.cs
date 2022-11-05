using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;

namespace by.Reba.Application.TagHelpers
{
    // todo : fix this
    public class DateFormatterTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public DateTime Date { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            var time = DateTime.Now - Date;
            var formated = time.Days switch
            {
                >= 2 => Date.ToString("dd MMMM, HH:mm", new CultureInfo("ru-RU")),
                1 => $"Вчера в {Date.ToString("HH:mm")}",
                0 => $"Сегодня в {Date.ToString("hh:mm")}",
                _ => time.Hours < 10 ? $"{time.Hours} час. назад" : $"{time.Minutes} мин. назад",
            };

            output.Content.AppendHtml(formated);
        }
    }
}
