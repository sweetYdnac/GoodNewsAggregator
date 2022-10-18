﻿using by.Reba.Core.DataTransferObjects.Comment;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace by.Reba.Application.TagHelpers
{
    public class DisplayCommentsTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        [HtmlAttributeName("asp-for")]
        public IEnumerable<CommentDTO> Comments { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public DisplayCommentsTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

            output.Content.AppendHtmlLine("<div class=\"card card-body\">");

            foreach (var comment in Comments)
            {
                await RenderComments(comment, output);
            }

            output.Content.AppendHtmlLine("</div>");
        }

        private async Task RenderComments(CommentDTO comment, TagHelperOutput output, int deepLevel = 0)
        {
            var content = await _htmlHelper.PartialAsync("CommentPartial", comment);

            // TODO : Коляска
            var col = (12 - deepLevel) > 6 ? 12 - deepLevel : 7;
            output.Content.AppendHtmlLine($"<div class=\"col-{col} offset-{deepLevel}\">");
            output.Content.AppendHtml(content);

            foreach (var innerComment in comment.InnerComments)
            {
                await RenderComments(innerComment, output, ++deepLevel);
            }

            output.Content.AppendHtmlLine("</div>");
        }
    }
}