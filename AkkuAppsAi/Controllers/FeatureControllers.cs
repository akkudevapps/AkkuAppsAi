using Microsoft.AspNetCore.Mvc;

namespace AkkuAppsAi.Controllers;

public class ToolsController : Controller
{
    public IActionResult ImageEditor() { ViewBag.ToolName = "Image Editor"; return View("ImageEditor"); }
    public IActionResult JsonCreator() { ViewBag.ToolName = "JSON Creator"; return View("JsonCreator"); }
    public IActionResult OCR() { ViewBag.ToolName = "OCR Scanner"; return View("OCR"); }
    public IActionResult PDFReader() { ViewBag.ToolName = "PDF Reader"; return View("PDFReader"); }
    public IActionResult Prompts() { ViewBag.ToolName = "Prompt Templates"; return View("Prompts"); }
    public IActionResult ImageTemplates() { ViewBag.ToolName = "Image Templates"; return View("ImageTemplates"); }
    public IActionResult PythonAutomation() { ViewBag.ToolName = "Python Automation"; return View("PythonAutomation"); }
}

public class LearnController : Controller
{
    public IActionResult Index() => View();
}

public class NewsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NewsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var newsItems = new List<NewsViewModel>();

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.GetAsync("https://akkuapps.in/News");
            if (response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();
                newsItems = ParseNewsFromHtml(html);
            }
        }
        catch
        {
            newsItems = GetFallbackNews();
        }

        return View(newsItems);
    }

    private List<NewsViewModel> ParseNewsFromHtml(string html)
    {
        var newsItems = new List<NewsViewModel>();

        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);

        var articles = doc.DocumentNode.SelectNodes("//article | //div[contains(@class,'post')] | //div[contains(@class,'article')] | //div[contains(@class,'news')]");

        if (articles != null)
        {
            foreach (var article in articles.Take(20))
            {
                var titleNode = article.SelectSingleNode(".//h1 | .//h2 | .//h3 | .//h4 | .//a[contains(@class,'title')]");
                var linkNode = article.SelectSingleNode(".//a[@href]");
                var excerptNode = article.SelectSingleNode(".//p | .//div[contains(@class,'excerpt')] | .//div[contains(@class,'content')]");
                var imageNode = article.SelectSingleNode(".//img");
                var dateNode = article.SelectSingleNode(".//time | .//span[contains(@class,'date')] | .//span[contains(@class,'time')]");

                var title = titleNode?.InnerText.Trim() ?? "Untitled";
                var link = linkNode?.GetAttributeValue("href", "#") ?? "#";
                if (!link.StartsWith("http")) link = "https://akkuapps.in" + link.TrimStart('~');
                var excerpt = excerptNode?.InnerText.Trim().Substring(0, Math.Min(150, excerptNode.InnerText.Length)) ?? "";
                var imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";
                var dateStr = dateNode?.InnerText.Trim() ?? "";

                newsItems.Add(new NewsViewModel
                {
                    Title = title,
                    Link = link,
                    Excerpt = excerpt,
                    ImageUrl = imageUrl,
                    Date = dateStr,
                    Source = "AkkuApps.in"
                });
            }
        }

        return newsItems;
    }

    private List<NewsViewModel> GetFallbackNews()
    {
        return new List<NewsViewModel>
        {
            new NewsViewModel { Title = "Welcome to AkkuApps News", Excerpt = "Stay updated with the latest AI tools, features, and announcements from AkkuApps.", Date = DateTime.Now.ToString("MMM dd, yyyy"), Source = "AkkuApps" },
            new NewsViewModel { Title = "Coin Economy System Launched", Excerpt = "Purchase coins via UPI and unlock premium AI tools and digital goods in the marketplace.", Date = DateTime.Now.AddDays(-2).ToString("MMM dd, yyyy"), Source = "AkkuApps" },
            new NewsViewModel { Title = "New AI Tools Available", Excerpt = "Image Editor, JSON Creator, OCR Scanner, PDF Reader and more AI tools now available for all users.", Date = DateTime.Now.AddDays(-5).ToString("MMM dd, yyyy"), Source = "AkkuApps" }
        };
    }
}

public class NewsViewModel
{
    public string Title { get; set; } = "";
    public string Link { get; set; } = "#";
    public string Excerpt { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string Date { get; set; } = "";
    public string Source { get; set; } = "";
}

public class GamesController : Controller
{
    public IActionResult Index() => View();
}

public class WalletController : Controller
{
    public IActionResult Index() => View();
}


