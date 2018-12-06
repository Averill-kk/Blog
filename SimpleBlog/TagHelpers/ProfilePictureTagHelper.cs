using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SimpleBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlog.TagHelpers
{
    public class ProfilePictureTagHelper : TagHelper
    {
       
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionAccessor;
        public ProfilePictureTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionAccessor)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionAccessor = actionAccessor;
        }

        public ApplicationUser Profile { get; set; }
        public int? SizePx { get; set; }
        private bool IsDefaultPicture => String.IsNullOrWhiteSpace(this.Profile.PictureUrl);
        private IUrlHelper UrlHelper => this.urlHelperFactory.GetUrlHelper(this.actionAccessor.ActionContext);
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //Render nothing if there is no profile or profile doesn't have a picture url

            if (this.Profile == null )
            {
                output.SuppressOutput();
                return;
            }
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "profile-picture");

            var img = new TagBuilder("img");
            img.Attributes.Add("src", this.GetPictureUrl());
            if (this.IsDefaultPicture && this.SizePx.HasValue)
            {
                img.Attributes.Add("style",
                  $"height:{this.SizePx.Value}px;width:{this.SizePx.Value}px");
            }
            output.Content.SetHtmlContent(img);
        }
        private string GetPictureUrl()
        {
            if (this.IsDefaultPicture)
            {
                return this.UrlHelper.Content("~/images/placeholder.png");
            }

            var imgUriBuilder = new UriBuilder(this.Profile.PictureUrl);
            if (this.SizePx.HasValue)
            {
                var query = QueryString.FromUriComponent(imgUriBuilder.Query);
                query = query.Add("sz", this.SizePx.Value.ToString());
                imgUriBuilder.Query = query.ToString();
            }

            return imgUriBuilder.Uri.ToString();
        }
    }
}
