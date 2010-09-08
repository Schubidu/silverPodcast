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

namespace SliverlightPodcast
{
	public class PodcastItem
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime PubDate { get; set; }
		public Uri Link {get; set;}
		static public class Helper {
			static string proxyUrl = "proxy.php?u=";
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
					result = new Uri(proxyUrl + link, UriKind.RelativeOrAbsolute);
				} catch(UriFormatException ex) {
					result = null;
				}
				return result;

			}
		}
	}
}
