using by.Reba.Core.DataTransferObjects.Comment;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static by.Reba.Core.TreeExtensions;

namespace by.Reba.Application.TagHelpers
{
    public class DisplayCommentsTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        [HtmlAttributeName("asp-for")]
        public IEnumerable<ITree<CommentDTO>> Comments { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public DisplayCommentsTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is not null && Comments is not null)
            {
                (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

                output.Content.AppendHtmlLine("<div class=\"card card-body\">");

                foreach (var comment in Comments)
                {
                    await RenderComments(comment, output);
                }

                output.Content.AppendHtmlLine("</div>");
            }
        }

        private async Task RenderComments(ITree<CommentDTO> comment, TagHelperOutput output, int deepLevel = 0)
        {
            var content = await _htmlHelper.PartialAsync("CommentPartial", comment.Data);

            // TODO : Коляска! ПЕРЕДЕЛАТЬ (стили задавать через атрибуты tag хелпера)
            var col = (12 - deepLevel) > 6 ? 12 - deepLevel : 7;
            output.Content.AppendHtmlLine($"<div class=\"col-{col} offset-{deepLevel}\">");
            output.Content.AppendHtml(content);

            foreach (var child in comment.Children)
            {
                await RenderComments(child, output, ++deepLevel);
            }

            output.Content.AppendHtmlLine("</div>");
        }
    }
}
