using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace SliverlightPodcast
{
	public class PodcastItem
	{
		public string Title { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
		public DateTime PubDate { get; set; }
		public Uri Link {get; set;}
        public BitmapImage ImageSource {get;set;}
        public long Id
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Title + "\n" + this.Description;
        }

		static public class Helper {

            static public string ProxyUrl {
                get {
                    IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
                    if (appSettings.Contains("proxyUrl")) {
                        return appSettings["proxyUrl"].ToString();
                    }
                    return "";
                }
            }

			//static string proxyUrl = "proxy.php?u=";
			static public DateTime PubDate(string pubDate)
			{
				DateTime result;
				bool parse = DateTime.TryParse(pubDate, out result);
				return result;
			}
			static public Uri Link(string link)
			{
				Uri result;
				try {
					result = new Uri(link, UriKind.RelativeOrAbsolute);
				} catch(UriFormatException ex) {
					result = null;
				}
				return result;

			}
		}
	}
}
