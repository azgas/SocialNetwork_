using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocNetEPX.TemplateUtilities.ModalWindows
{
    public static class ModalActionLinkExtensions
    {
        public static MvcHtmlString ModalActionLink(this AjaxHelper ajaxHelper, string linkText,
            string modalId, string modalTitle, string modalLoadingIndicatorPath, string modalWindowCSSClasses,
            string actionName, string controllerName, Object routeValues, Object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("a");

            UrlHelper urlHelper = new UrlHelper(ajaxHelper.ViewContext.RequestContext);

            tagBuilder.Attributes.Add("href", urlHelper.Action(actionName, controllerName, routeValues));

            tagBuilder.Attributes.Add("data-ajax-modal-link", "true");

            tagBuilder.Attributes.Add("data-ajax-modal-id", modalId);

            tagBuilder.Attributes.Add("data-ajax-modal-title", modalTitle);

            tagBuilder.Attributes.Add("data-ajax-modal-loading-indicator-path", modalLoadingIndicatorPath);

            if (!string.IsNullOrEmpty(modalWindowCSSClasses))
                tagBuilder.Attributes.Add("data-ajax-modal-classes", modalWindowCSSClasses);

            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            tagBuilder.InnerHtml = linkText;

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString ModalActionLink(this AjaxHelper ajaxHelper, string linkText,
            string modalId, string modalTitle, string modalLoadingIndicatorPath, string modalWindowCSSClasses,
            string actionName, Object routeValues, Object htmlAttributes)
        {
            return ModalActionLink(ajaxHelper, linkText, modalId, modalTitle, modalLoadingIndicatorPath,
                modalWindowCSSClasses, actionName, null, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ModalActionLink(this AjaxHelper ajaxHelper, string linkText,
            string modalId, string modalTitle, string modalLoadingIndicatorPath, string modalWindowCSSClasses,
            string actionName, string controllerName, Object routeValues)
        {
            return ModalActionLink(ajaxHelper, linkText, modalId, modalTitle, modalLoadingIndicatorPath,
                modalWindowCSSClasses, actionName, controllerName, routeValues, null);
        }

        public static MvcHtmlString ModalActionLink(this AjaxHelper ajaxHelper, string linkText,
            string modalId, string modalTitle, string modalLoadingIndicatorPath, string modalWindowCSSClasses,
            string actionName, string controllerName)
        {
            return ModalActionLink(ajaxHelper, linkText, modalId, modalTitle, modalLoadingIndicatorPath,
                modalWindowCSSClasses, actionName, controllerName, null, null);
        }

        public static MvcHtmlString ModalActionLink(this AjaxHelper ajaxHelper, string linkText,
            string modalId, string modalTitle, string modalLoadingIndicatorPath, string modalWindowCSSClasses,
            string actionName, Object routeValues)
        {
            return ModalActionLink(ajaxHelper, linkText, modalId, modalTitle, modalLoadingIndicatorPath,
                modalWindowCSSClasses, actionName, null, routeValues, null);
        }

        public static MvcHtmlString ModalActionLink(this AjaxHelper ajaxHelper, string linkText,
            string modalId, string modalTitle, string modalLoadingIndicatorPath, string modalWindowCSSClasses,
            string actionName)
        {
            return ModalActionLink(ajaxHelper, linkText, modalId, modalTitle, modalLoadingIndicatorPath,
                modalWindowCSSClasses, actionName, null, null, null);
        }
    }
}