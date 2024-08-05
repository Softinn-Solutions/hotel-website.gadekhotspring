using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using EmbunLuxuryVillas.Helpers;
using EmbunLuxuryVillas.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Softinn.SiteMap.AWS;

namespace EmbunLuxuryVillas.Controllers
{
    public class SeoHelperController : Controller
    {
        protected readonly IWebHostEnvironment HostingEnvironment;

        //public SeoHelperController(IHostingEnvironment hostingEnv)
        //{
        //    this.HostingEnvironment = hostingEnv;
        //}

        private readonly IOptions<AppConfigurations> _appConfigurations;

        public SeoHelperController(IWebHostEnvironment hostingEnv, IOptions<AppConfigurations> config)
        {
            this.HostingEnvironment = hostingEnv;
            _appConfigurations = config;
        }

        [Route("robots.txt")]
        public FileContentResult GenerateRobot()
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Set robots.txt rules here
            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("disallow: /Administration/");
            stringBuilder.AppendLine("disallow: /Admin/");
            stringBuilder.AppendLine("disallow: /Account/");

            if (Request.Host != null)
            {
                stringBuilder.Append("sitemap: ");
                stringBuilder.AppendLine(Request.Host + "/sitemap.xml");
            }

            return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/plain");
        }

        [Route("sitemap.xml")]
        public async Task<ActionResult> GenerateSiteMap()
        {
            var hotelId = this._appConfigurations.Value.HotelId;

            var hostUrl = Request.Host.ToString();
            // prevent sending localhost as host into service.
            // prevent azurewebsites being capture.
            if (this.HostingEnvironment.IsDevelopment() || hostUrl.Contains("azurewebsites"))
            {
                var liteDbHelper = new LiteDbHelper();
                hostUrl = liteDbHelper.GetHotel().URLDomain;
            }

            if (string.IsNullOrEmpty(hostUrl))
            {
                return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(
                    $"no valid url detect: {hostUrl}")),
                    "text/plain");
            }

            var siteMapRequestViewModel = new SiteMapRequestViewModel
            {
                Id = hotelId,
                Host = hostUrl
            };

            var softinnSiteMapAwsService = new SoftinnSiteMapAWSService(siteMapRequestViewModel);

            var siteMapResult = await softinnSiteMapAwsService.GetSiteMap();

            return new FileStreamResult(siteMapResult.Item1, siteMapResult.Item2);
        }
    }

    public class XmlSitemapResult : ActionResult
    {
        private IEnumerable<LocationUrls_Result> _items;

        public XmlSitemapResult(IEnumerable<LocationUrls_Result> items, HttpRequest request)
        {
            _items = items;
        }

        public override void ExecuteResult(ActionContext context)
        {
            //string encoding = context.HttpContext.Request.;
            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            XDocument sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(_xmlns + "urlset",
                    new XAttribute(XNamespace.Xmlns + "image", "http://www.google.com/schemas/sitemap-image/1.1"),
                    new XAttribute(XNamespace.Xmlns + "mobile", "http://www.google.com/schemas/sitemap-mobile/1.0"),
                    from item in _items
                    select CreateItemElement(item)
                    )
                );

            context.HttpContext.Response.ContentType = "application/rss+xml";
            context.HttpContext.Response.Body.Flush();
            context.HttpContext.Response.Body.Write(Encoding.UTF8.GetBytes(sitemap.Declaration + sitemap.ToString()));
        }

        private XElement CreateItemElement(LocationUrls_Result item)
        {
            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace imageNs = "http://www.google.com/schemas/sitemap-image/1.1";
            XNamespace mobileNs = "http://www.google.com/schemas/sitemap-mobile/1.0";

            XElement itemElement = new XElement(_xmlns + "url");
            itemElement.Add(new XElement(_xmlns + "loc", item.Url.ToLower()));

            XElement imageElement = new XElement(imageNs + "image");
            foreach (var imageUrl in item.ImageUrls)
            {
                imageElement.Add(new XElement(imageNs + "loc", imageUrl.ToLower()));
            }
            itemElement.Add(imageElement);

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement(_xmlns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

            if (item.Changefreq != null)
                itemElement.Add(new XElement(_xmlns + "changefreq", item.Changefreq.ToString().ToLower()));

            if (item.Priority.HasValue)
                itemElement.Add(new XElement(_xmlns + "priority", item.Priority.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            itemElement.Add(new XElement(mobileNs + "mobile"));

            return itemElement;
        }
    }

    public class Crawl
    {
        protected readonly IWebHostEnvironment HostingEnvironment;

        public static List<LocationUrls_Result> Urls = null;
        public static List<string> PageUrls = null;
        private HttpRequest request;

        public string PrimaryUrl { get; set; }
        public string PrimaryHost { get; set; }
        string CurrentUrl { get; set; }


        public Crawl(HttpRequest request, IWebHostEnvironment hostingEnv)
        {
            this.request = request;

            Urls = new List<LocationUrls_Result>();
            PageUrls = new List<string>();
            CurrentUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

            this.HostingEnvironment = hostingEnv;
        }

        public void GetUrlsOfSite(string url)
        {
            if (this.HostingEnvironment.IsDevelopment())
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            WebRequest webRequest = WebRequest.Create(url);
            if (webRequest != null)
            {
                WebResponse webResponse = webRequest.GetResponse();
                Stream data = webResponse.GetResponseStream();
                string html = String.Empty;
                using (StreamReader sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();

                    Match m;
                    string HRefPattern = "<a\\s+(?:[^>]*?\\s+)?href=\"([^\"]*)\"";

                    try
                    {
                        m = Regex.Match(html, HRefPattern, RegexOptions.IgnoreCase);
                        while (m.Success)
                        {
                            string urlValue = m.Groups[1].Value;
                            if (urlValue.Trim().Length > 0 && !urlValue.Trim().StartsWith("#"))
                            {
                                string tempStr = urlValue;

                                if (tempStr.Contains("tel:") || tempStr.Contains("mailto:") || tempStr.Contains("fax:") ||
                                    tempStr.Contains("cdn-cgi") || tempStr.Contains("softinnstorage.blob.core.windows.net") || IsMediaExtension(tempStr))
                                    tempStr = "";

                                if (tempStr.StartsWith(".."))
                                    tempStr = tempStr.Replace("..", "").Trim();

                                if (!urlValue.Contains("http://") && !urlValue.Contains("https://"))
                                    tempStr = PrimaryUrl + tempStr;

                                if (tempStr != CurrentUrl)
                                {
                                    UriBuilder uri = new UriBuilder(tempStr);
                                    if (uri.Host == PrimaryHost)
                                    {
                                        if (!PageUrls.Contains(uri.Uri.ToString().ToLowerInvariant()))
                                        {
                                            PageUrls.Add(uri.Uri.ToString().ToLowerInvariant());
                                            GetUrlsOfSite(uri.Uri.ToString());
                                        }
                                    }
                                }
                            }
                            m = m.NextMatch();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public List<string> GetImagesUrlOfSite(string url)
        {
            var imagesUrl = new List<string>();

            if (this.HostingEnvironment.IsDevelopment())
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            WebRequest webRequest = WebRequest.Create(url);
            if (webRequest != null)
            {
                try
                {
                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        Stream data = webResponse.GetResponseStream();
                        string html = String.Empty;
                        using (StreamReader sr = new StreamReader(data))
                        {
                            html = sr.ReadToEnd();

                            Match m;
                            string HRefPattern = "<img\\s+(?:[^>]*?\\s+)?src=\"([^\"]*)\"";

                            try
                            {
                                m = Regex.Match(html, HRefPattern, RegexOptions.IgnoreCase);
                                while (m.Success)
                                {
                                    string urlValue = m.Groups[1].Value;
                                    if (urlValue.Trim().Length > 0 && !urlValue.Trim().StartsWith("#"))
                                    {
                                        string tempStr = urlValue;

                                        if (tempStr.StartsWith(".."))
                                            tempStr = tempStr.Replace("..", "").Trim();

                                        if (!urlValue.Contains("http://") && !urlValue.Contains("https://") && !urlValue.Contains("//softinnstorage.blob.core.windows.net"))
                                            tempStr = PrimaryUrl + tempStr;

                                        if (urlValue.Contains("//softinnstorage.blob.core.windows.net"))
                                        {
                                            if (!urlValue.Contains("http"))
                                            {
                                                tempStr = "https:" + tempStr;
                                            }
                                        }

                                        if (tempStr != CurrentUrl || urlValue.Contains("//softinnstorage.blob.core.windows.net"))
                                        {
                                            UriBuilder uri = new UriBuilder(tempStr);
                                            if (uri.Host == PrimaryHost || urlValue.Contains("//softinnstorage.blob.core.windows.net"))
                                            {
                                                if (IsMediaExtension(uri.Uri.ToString().ToLowerInvariant()) && !imagesUrl.Contains(uri.Uri.ToString().ToLowerInvariant()))
                                                {
                                                    imagesUrl.Add(uri.Uri.ToString().ToLowerInvariant());
                                                }
                                            }
                                        }
                                    }
                                    m = m.NextMatch();
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream asd = response.GetResponseStream())
                        using (var reader = new StreamReader(asd))
                        {
                            string text = reader.ReadToEnd();
                            Console.WriteLine(text);
                        }
                    }
                }
            }
            return imagesUrl;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public bool IsMediaExtension(string path)
        {
            string[] mediaExtensions =
            {
                ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF",
                ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA",
                ".AVI", ".MP4", ".DIVX", ".WMV"
            };
            return mediaExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);
        }
    }

    public class LocationUrls_Result
    {
        public string Url { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Changefreq { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public int? OrderBy { get; set; }
    }
}