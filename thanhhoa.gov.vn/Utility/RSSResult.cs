using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace thanhhoa.gov.vn.Utility
{
    public class RSSResult : ActionResult
    {
        public SyndicationFeed feedData { get; set; }
        public string contentType = "rss";

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/atom+xml";
            //check request is for Atom or RSS
            if (context.HttpContext.Request.QueryString["type"] != null && context.HttpContext.Request.QueryString["type"].ToString().ToLower() == "atom")
            {
                //Atom Feed
                context.HttpContext.Response.ContentType = "application/atom+xml";
                var rssFormatter = new Atom10FeedFormatter(feedData);
                using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output, new XmlWriterSettings { Indent = true }))
                {
                    rssFormatter.WriteTo(writer);
                }
            }
            else
            {
                //RSS Feed
                context.HttpContext.Response.ContentType = "application/rss+xml";
                var rssFormatter = new Rss20FeedFormatter(feedData);
                using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output, new XmlWriterSettings { Indent = true }))
                {
                    rssFormatter.WriteTo(writer);
                }
            }

        }
    }
}