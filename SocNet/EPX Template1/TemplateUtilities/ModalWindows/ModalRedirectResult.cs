using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EPX_Template1.TemplateUtilities.ModalWindows
{
    public class ModalRedirectResult : ActionResult
    {
        public string Url { get; set; }

        public ModalRedirectResult(string url)
        {
            Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write("<script>window.location='" + Url + "'</script>");
        }
    }
}