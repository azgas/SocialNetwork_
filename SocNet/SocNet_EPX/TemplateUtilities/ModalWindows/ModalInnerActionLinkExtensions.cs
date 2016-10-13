using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocNet_EPX.TemplateUtilities.ModalWindows
{
    public static class ModalInnerActionLinkExtensions
    {
        public static MvcHtmlString ModalInnerActionLink(this HtmlHelper htmlHelper, string linkText,
            string actionName, string controllerName, Object routeValues, Object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("a");

            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            tagBuilder.Attributes.Add("href", urlHelper.Action(actionName, controllerName, routeValues));

            tagBuilder.Attributes.Add("data-ajax-inner-modal-link", "true");

            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            tagBuilder.InnerHtml = linkText;

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString ModalInnerActionLink(this HtmlHelper htmlHelper, string linkText,
            string actionName, Object routeValues, Object htmlAttributes)
        {
            return ModalInnerActionLink(htmlHelper, linkText, actionName, null, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ModalInnerActionLink(this HtmlHelper htmlHelper, string linkText,
            string actionName, string controllerName)
        {
            return ModalInnerActionLink(htmlHelper, linkText, actionName, controllerName, null, null);
        }

        public static MvcHtmlString ModalInnerActionLink(this HtmlHelper htmlHelper, string linkText,
            string actionName, Object routeValues)
        {
            return ModalInnerActionLink(htmlHelper, linkText, actionName, null, routeValues, null);
        }

        public static MvcHtmlString ModalInnerActionLink(this HtmlHelper htmlHelper, string linkText,
            string actionName)
        {
            return ModalInnerActionLink(htmlHelper, linkText, actionName, null, null, null);
        }
    }
}