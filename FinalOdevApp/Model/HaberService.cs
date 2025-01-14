using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FinalOdevApp.Model
{
    public class NewsViewModel : INotifyPropertyChanged
    {
        private readonly IHaberService _rssService;
        private ObservableCollection<RssItem> _newsItems;

        public ObservableCollection<RssItem> NewsItems
        {
            get => _newsItems;
            set
            {
                _newsItems = value;
                OnPropertyChanged(nameof(NewsItems));
            }
        }

        public NewsViewModel(IHaberService haberService = null)
        {
            _rssService = haberService ?? new HaberService();
            _newsItems = new ObservableCollection<RssItem>();
        }

        public async Task LoadNewsAsync(string category)
        {
            try
            {
                var news = await _rssService.GetNewsAsync(category);
                NewsItems.Clear();
                foreach (var item in news)
                {
                    NewsItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi burada yapılabilir
                System.Diagnostics.Debug.WriteLine($"Error loading news: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IHaberService
    {
        Task<List<RssItem>> GetNewsAsync(string category);
    }

    public class RssItem
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public string Category { get; set; }
    }

    public class HaberService : IHaberService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly Dictionary<string, string> RssUrls = new Dictionary<string, string>
        {
            { "Manşet", "https://www.trthaber.com/manset_articles.rss" },
            { "Son Dakika", "https://www.trthaber.com/sondakika_articles.rss" },
            { "Gündem", "https://www.trthaber.com/gundem_articles.rss" },
            { "Ekonomi", "https://www.trthaber.com/ekonomi_articles.rss" },
            { "Spor", "https://www.trthaber.com/spor_articles.rss" },
            { "Bilim Teknoloji", "https://www.trthaber.com/bilim_teknoloji_articles.rss" }
        };

        public async Task<List<RssItem>> GetNewsAsync(string category)
        {
            if (!RssUrls.TryGetValue(category, out string url))
            {
                return new List<RssItem>();
            }

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var doc = XDocument.Parse(response);

                return doc.Root
                    .Descendants()
                    .Where(item => item.Name.LocalName == "item")
                    .Select(item => new RssItem
                    {
                        Title = item.Elements().FirstOrDefault(i => i.Name.LocalName == "title")?.Value?.Trim(),
                        Link = item.Elements().FirstOrDefault(i => i.Name.LocalName == "link")?.Value?.Trim(),
                        Description = CleanDescription(item.Elements().FirstOrDefault(i => i.Name.LocalName == "description")?.Value),
                        PubDate = ParsePubDate(item.Elements().FirstOrDefault(i => i.Name.LocalName == "pubDate")?.Value),
                        Image = GetImageUrl(item),
                        Category = category
                    })
                    .Where(item => !string.IsNullOrEmpty(item.Title))
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching RSS feed: {ex.Message}");
                return new List<RssItem>();
            }
        }

        private static string CleanDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return string.Empty;

            // HTML etiketlerini temizle
            description = Regex.Replace(description, "<.*?>", string.Empty);
            // HTML entitylerini decode et
            description = System.Net.WebUtility.HtmlDecode(description);
            return description.Trim();
        }

        private static DateTime ParsePubDate(string pubDate)
        {
            return DateTime.TryParse(pubDate, out DateTime result) ?
                result :
                DateTime.Now;
        }

        private static string GetImageUrl(XElement item)
        {
            return item.Elements()
                .FirstOrDefault(i => i.Name.LocalName == "enclosure")
                ?.Attribute("url")?.Value
                ?? item.Elements()
                .FirstOrDefault(i => i.Name.LocalName == "media:content")
                ?.Attribute("url")?.Value;
        }
    }
}