﻿using by.Reba.Core.DataTransferObjects.Comment;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static by.Reba.Core.TreeExtensions;

namespace by.Reba.Application.TagHelpers
{
    public class DisplayCommentsTagHelper : TagHelper
    {
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

        private async Task<TagBuilder> CreateComments(ITree<CommentDTO> comment, bool isRoot = true)
        {
            var root = new TagBuilder("div");
            root.Attributes["class"] = $"col-md-{(isRoot ? 12 : 11)} offset-md-{(isRoot ? 0 : 1)}";

            var content = await _htmlHelper.PartialAsync("_CommentPartial", comment.Data);
            root.InnerHtml.AppendHtml(content);

            foreach (var child in comment.Children)
            {
                root.InnerHtml.AppendHtml(await CreateComments(child, false));
            }

            return root;
        }
    }
}
