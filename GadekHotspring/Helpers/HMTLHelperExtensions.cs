using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace GadekHotspring.Helpers
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string notAction = null, string attractionType = null, string name = null, string cssClass = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "current-menu-item";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            string currentName = (string)html.ViewContext.RouteData.Values["name"];
            string currentAttractionType = (string)html.ViewContext.RouteData.Values["attractionType"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            if (name != null)
            {
                if (name == currentName)
                {
                    return cssClass;
                }
                else if (currentName != name)
                {
                    return String.Empty;
                }
            }

            if (attractionType != null)
            {
                if (attractionType == currentAttractionType)
                {
                    return cssClass;
                }
                else if (currentAttractionType != attractionType)
                {
                    return String.Empty;
                }
            }

            return
                controller == currentController &&
                action == currentAction &&
                action != notAction
                ? cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        //public static string ToSeoFriendly(this HtmlHelper helper, string str)
        //{
        //    if (String.IsNullOrEmpty(str))
        //    {
        //        return "";
        //    }

        //    StringBuilder sb = new StringBuilder();
        //    foreach (char c in str)
        //    {
        //        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
        //        {
        //            sb.Append(c);
        //        }
        //        else if (Char.IsWhiteSpace(c) || c == '-')
        //        {
        //            sb.Append('-');
        //        }
        //    }
        //    return sb.ToString();
        //}
    }
}
