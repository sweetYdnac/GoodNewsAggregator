using by.Reba.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace by.Reba.Application.TagHelpers
{
    public class PaginatorTagHelper : TagHelper
    {
        public PagingInfo PageModel { get; set; }

        public string PageClass { get; set; } = string.Empty;
        public string PageClassNormal { get; set; } = string.Empty;
        public string PageClassSelected { get; set;  } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var result = new TagBuilder("div");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                var submit = new TagBuilder("button");
                submit.Attributes["type"] = "submit";
                submit.Attributes["name"] = "page";
                submit.Attributes["value"] = i.ToString();
                submit.InnerHtml.Append(i.ToString());

                submit.AddCssClass(PageClass);
                submit.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);

                result.InnerHtml.AppendHtml(submit);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }

    }
}
