using by.Reba.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace by.Reba.Application.TagHelpers
{
    public class PaginatorTagHelper : TagHelper
    {
        private const int NEIGHBOURS_COUNT = 3;
        private readonly IHtmlHelper _htmlHelper;

        public PaginatorTagHelper(IHtmlHelper htmlHelper) =>
            _htmlHelper = htmlHelper;

        public PagingInfo PageModel { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public string OutputClass { get; set; } = string.Empty;
        public string PageItemClass { get; set; } = string.Empty;
        public string PageLinkClass { get; set; } = string.Empty;
        public string SelectedClass { get; set; } = string.Empty;
        public string DefaultClass { get; set; } = string.Empty;

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is not null && PageModel is not null)
            {
                (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

                output.TagName = "ul";
                output.Attributes.SetAttribute("class", OutputClass);
                var result = new TagBuilder("div");

                if (PageModel.CurrentPage > 1)
                {
                    result.InnerHtml.AppendHtml(CreatePageLink(PageModel.CurrentPage - 1, "&laquo;"));
                }

                result.InnerHtml.AppendHtml(CreatePageLink(1, "1"));

                if (PageModel.CurrentPage > NEIGHBOURS_COUNT)
                {
                    result.InnerHtml.AppendHtml(await CreateEnterPageLink("&hellip;", "enterPageModalLeftId"));
                }

                var i = Math.Max(2, PageModel.CurrentPage - (NEIGHBOURS_COUNT / 2));
                var j = Math.Min(PageModel.CurrentPage + (NEIGHBOURS_COUNT / 2) + 1, PageModel.TotalPages);

                for (; i < j; i++)
                {
                    result.InnerHtml.AppendHtml(CreatePageLink(i, i.ToString()));
                }

                if (PageModel.TotalPages - Math.Ceiling((decimal)NEIGHBOURS_COUNT / 2)> PageModel.CurrentPage)
                {
                    result.InnerHtml.AppendHtml(await CreateEnterPageLink("&hellip;", "enterPageModalRightId"));
                }

                if (PageModel.TotalPages > 1)
                {
                    result.InnerHtml.AppendHtml(CreatePageLink(PageModel.TotalPages, PageModel.TotalPages.ToString()));
                }

                if (PageModel.CurrentPage < PageModel.TotalPages)
                {
                    result.InnerHtml.AppendHtml(CreatePageLink(PageModel.CurrentPage + 1, "&raquo;"));
                }

                output.Content.AppendHtml(result.InnerHtml);
            }
        }

        private TagBuilder CreatePageLink(int page, string content)
        {
            var li = new TagBuilder("li");
            li.AddCssClass(PageItemClass);

            var submit = new TagBuilder("button");
            submit.Attributes["type"] = "submit";
            submit.Attributes["name"] = "page";
            submit.Attributes["value"] = page.ToString();
            submit.AddCssClass(PageLinkClass);

            if (PageModel.CurrentPage == page)
            {
                submit.AddCssClass(SelectedClass);
            }
            else
            {
                submit.AddCssClass(DefaultClass);
            }

            submit.InnerHtml.AppendHtml(content);
            li.InnerHtml.AppendHtml(submit);

            return li;
        }

        private async Task<TagBuilder> CreateEnterPageLink(string content, string targetId)
        {
            var li = new TagBuilder("li");
            li.AddCssClass(PageItemClass);

            var submit = new TagBuilder("button");
            submit.Attributes["type"] = "button";
            submit.Attributes["data-bs-toggle"] = "modal";
            submit.Attributes["data-bs-target"] = $"#{targetId}";
            submit.AddCssClass(PageLinkClass);
            submit.AddCssClass(DefaultClass);
            submit.InnerHtml.AppendHtml(content);

            var modal = await _htmlHelper.PartialAsync("_EnterPagePartial", $"{targetId}");
            li.InnerHtml.AppendHtml(submit);
            li.InnerHtml.AppendHtml(modal);

            return li;
        }
    }
}
