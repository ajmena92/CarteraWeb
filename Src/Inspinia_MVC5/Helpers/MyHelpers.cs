using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace WebCartera.Helpers
{
    public static class MyHelpers
    {
        // Render BootStrap menu item with active class
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper,
                                             string text, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            var li = new TagBuilder("li");
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");
            if (string.Equals(currentAction,
                              action,
                              StringComparison.OrdinalIgnoreCase)
                && string.Equals(currentController,
                              controller,
                              StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }
            if (routeValues != null)
            {
                li.InnerHtml = (htmlAttributes != null)
                    ? htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            routeValues,
                                            htmlAttributes).ToHtmlString()
                    : htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            routeValues).ToHtmlString();
            }
            else
            {
                li.InnerHtml = (htmlAttributes != null)
                    ? htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            null,
                                            htmlAttributes).ToHtmlString()
                    : htmlHelper.ActionLink(text,
                                            action,
                                            controller).ToHtmlString();
            }
            return MvcHtmlString.Create(li.ToString());
        }


        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                             string text, string title, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder builder = new TagBuilder("a")
            {
                InnerHtml = text
            };
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }

    }


    public static class HtmlHelperExt
    {
        public static HtmlString PostLink(this HtmlHelper html, string text, string action, object routeValues)
        {
            var tbForm = new TagBuilder("form");
            tbForm.MergeAttribute("method", "POST");
            tbForm.MergeAttribute("action", action);

            var inputDict = HtmlHelper.ObjectToDictionary(routeValues);
            var inputs = new List<string>();
            foreach (var key in inputDict.Keys)
            {
                const string inputFormat = @"<input type='hidden' name='{0}' value='{1}' />";

                var input = string.Format(inputFormat, key, html.Encode(inputDict[key]));
                inputs.Add(input);
            }

            const string submitBtn = "<input type='submit' value='{0}'>";
            inputs.Add(string.Format(submitBtn, text));

            tbForm.InnerHtml = string.Join("\n", inputs.ToArray());

            // return self closing
            return new MvcHtmlString(tbForm.ToString());
        }
    }
}