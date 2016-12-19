using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPXSocNet.TemplateUtilities.ModalWindows
{
    public static class RedirectToActionForModalExtensions
    {
        public static ModalRedirectResult RedirectToActionForModal(this Controller controller,
            string actionName, string controllerName, object routeValues)
        {
            string url = controller.Url.Action(actionName, controllerName, routeValues);

            return new ModalRedirectResult(url);
        }

        public static ModalRedirectResult RedirectToActionForModal(this Controller controller,
            string actionName, string controllerName)
        {
            return RedirectToActionForModal(controller, actionName, controllerName, null);
        }

        public static ModalRedirectResult RedirectToActionForModal(this Controller controller,
            string actionName, object routeValues)
        {
            return RedirectToActionForModal(controller, actionName, null, routeValues);
        }

        public static ModalRedirectResult RedirectToActionForModal(this Controller controller,
            string actionName)
        {
            return RedirectToActionForModal(controller, actionName, null, null);
        }
    }
}