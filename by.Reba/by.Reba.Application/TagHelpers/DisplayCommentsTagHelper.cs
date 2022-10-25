using by.Reba.Core.DataTransferObjects.Comment;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static by.Reba.Core.TreeExtensions;

namespace by.Reba.Application.TagHelpers
{
    public class DisplayCommentsTagHelper : TagHelper
    {
        private const int MAX_DEEP_LEVEL = 6;
        private readonly IHtmlHelper _htmlHelper;

        public DisplayCommentsTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        [HtmlAttributeName("asp-for")]
        public IEnumerable<ITree<CommentDTO>>? Comments { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is not null && Comments is not null)
            {
                (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

                foreach (var comment in Comments)
                {
                    output.Content.AppendHtml(await CreateComments(comment));
                }
            }
        }

        private async Task<TagBuilder> CreateComments(ITree<CommentDTO> comment, int deepLevel = 0)
        {
            var root = new TagBuilder("div");

            var col = Math.Max(12 - deepLevel, MAX_DEEP_LEVEL);
            root.Attributes["class"] = $"col-{col} offset-{deepLevel}";

            var content = await _htmlHelper.PartialAsync("_CommentPartial", comment.Data);
            root.InnerHtml.AppendHtml(content);

            foreach (var child in comment.Children)
            {
                root.InnerHtml.AppendHtml(await CreateComments(child, ++deepLevel));
            }

            return root;
        }
    }
}
