using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace EPX_Template1.TemplateUtilities.ModalWindows
{
    public static class ModalWindowStructureHelpers
    {
        public class ModalWindowHeader : IDisposable
        {
            private bool disposed = false;

            private HtmlHelper htmlHelper;

            public ModalWindowHeader(HtmlHelper htmlHelper)
            {
                this.htmlHelper = htmlHelper;
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    htmlHelper.ViewContext.Writer.Write(htmlHelper.EndModalHeader().ToString());
                    disposed = true;
                }
            }

            public override string ToString()
            {
                return string.Empty;
            }
        }

        public static ModalWindowHeader BeginModalHeader(this HtmlHelper htmlHelper, Object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("div");

            tagBuilder.Attributes.Add("class", "modal-header");

            if (htmlAttributes != null)
            {
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                var classProperty = (from pi in htmlAttributes.GetType().GetProperties()
                                     where pi.Name == "class"
                                     select pi).FirstOrDefault();

                if (classProperty != null)
                {
                    tagBuilder.Attributes["class"] = tagBuilder.Attributes["class"] +
                        " " + classProperty.GetValue(htmlAttributes);
                }
            }

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new ModalWindowHeader(htmlHelper);
        }

        public static ModalWindowHeader BeginModalHeader(this HtmlHelper htmlHelper)
        {
            return BeginModalHeader(htmlHelper, null);
        }

        public static MvcHtmlString EndModalHeader(this HtmlHelper htmlHelper)
        {
            TagBuilder tagBuilder = new TagBuilder("div");

            TagBuilder buttonTagBuilder = new TagBuilder("button");
            buttonTagBuilder.Attributes.Add("type", "button");
            buttonTagBuilder.Attributes.Add("class", "close");
            buttonTagBuilder.Attributes.Add("data-dismiss", "modal");
            buttonTagBuilder.Attributes.Add("aria-hidden", "true");
            buttonTagBuilder.InnerHtml = "&times;";

            return new MvcHtmlString(buttonTagBuilder.ToString() + " " +
                tagBuilder.ToString(TagRenderMode.EndTag));
        }

        public class ModalWindowBody : IDisposable
        {
            private bool disposed = false;

            private HtmlHelper htmlHelper;

            public ModalWindowBody(HtmlHelper htmlHelper)
            {
                this.htmlHelper = htmlHelper;
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    htmlHelper.ViewContext.Writer.Write(htmlHelper.EndModalBody().ToString());
                    disposed = true;
                }
            }

            public override string ToString()
            {
                return string.Empty;
            }
        }

        public static ModalWindowBody BeginModalBody(this HtmlHelper htmlHelper, Object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("div");

            tagBuilder.Attributes.Add("class", "modal-body");

            if (htmlAttributes != null)
            {
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                var classProperty = (from pi in htmlAttributes.GetType().GetProperties()
                                     where pi.Name == "class"
                                     select pi).FirstOrDefault();

                if (classProperty != null)
                {
                    tagBuilder.Attributes["class"] = tagBuilder.Attributes["class"] +
                        " " + classProperty.GetValue(htmlAttributes);
                }
            }

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new ModalWindowBody(htmlHelper);
        }

        public static ModalWindowBody BeginModalBody(this HtmlHelper htmlHelper)
        {
            return BeginModalBody(htmlHelper, null);
        }

        public static MvcHtmlString EndModalBody(this HtmlHelper htmlHelper)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.EndTag));
        }

        public class ModalWindowFooter : IDisposable
        {
            private bool disposed = false;

            private HtmlHelper htmlHelper;

            public ModalWindowFooter(HtmlHelper htmlHelper)
            {
                this.htmlHelper = htmlHelper;
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    htmlHelper.ViewContext.Writer.Write(htmlHelper.EndModalFooter().ToString());
                    disposed = true;
                }
            }

            public override string ToString()
            {
                return string.Empty;
            }
        }

        public static ModalWindowFooter BeginModalFooter(this HtmlHelper htmlHelper, Object htmlAttributes, string cancelButtonText)
        {
            TagBuilder tagBuilder = new TagBuilder("div");

            tagBuilder.Attributes.Add("class", "modal-footer");

            if (htmlAttributes != null)
            {
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                var classProperty = (from pi in htmlAttributes.GetType().GetProperties()
                                     where pi.Name == "class"
                                     select pi).FirstOrDefault();

                if (classProperty != null)
                {
                    tagBuilder.Attributes["class"] = tagBuilder.Attributes["class"] +
                        " " + classProperty.GetValue(htmlAttributes);
                }
            }

            TagBuilder buttonTagBuilder = new TagBuilder("button");
            buttonTagBuilder.Attributes.Add("class", "btn btn-default btn-flat");
            buttonTagBuilder.Attributes.Add("data-dismiss", "modal");
            if (string.IsNullOrEmpty(cancelButtonText))
                buttonTagBuilder.InnerHtml = "Cancel";
            else
                buttonTagBuilder.InnerHtml = cancelButtonText;

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag) +
                " " + buttonTagBuilder.ToString());

            return new ModalWindowFooter(htmlHelper);
        }

        public static ModalWindowFooter BeginModalFooter(this HtmlHelper htmlHelper, Object htmlAttributes)
        {
            return BeginModalFooter(htmlHelper, null, null);
        }

        public static ModalWindowFooter BeginModalFooter(this HtmlHelper htmlHelper)
        {
            return BeginModalFooter(htmlHelper, null);
        }

        public static MvcHtmlString EndModalFooter(this HtmlHelper htmlHelper)
        {
            TagBuilder tagBuilder = new TagBuilder("div");

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.EndTag));
        }
    }
}