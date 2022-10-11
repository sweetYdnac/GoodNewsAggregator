using Microsoft.AspNetCore.Razor.TagHelpers;

namespace by.Reba.Application.TagHelpers
{
    // todo : fix this
    public class DateFormatterTagHelper : TagHelper
    {
        public DateTime Date { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.SelfClosing;
            output.TagName = "p";

            var time = DateTime.Now - Date;
            var formated = time.Days switch
            {
                >= 2 => Date.ToString("f"),
                1 => $"Вчера в {Date.ToString("t")}",
                0 => $"Сегодня в {Date.ToString("t")}",
                _ => time.Hours < 10 ? $"{time.Hours} час. назад" : $"{time.Minutes} мин. назад",
            };

            output.Content.SetContent(formated);
        }
    }
}
