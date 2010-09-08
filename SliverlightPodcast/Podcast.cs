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
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;

namespace SliverlightPodcast
{
	public class Podcast
	{
//		Uri url = new Uri("http://www.ndr.de/ndr2/podcast2956.xml");
		Uri url = new Uri("podcast.xml", UriKind.Relative);
		List<PodcastItem> list = new List<PodcastItem>();
		public Podcast()
		{
			WebClient client = new WebClient();
			client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
			client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
			client.OpenReadAsync(url);
		}

		void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error != null)
			{
			}
			using (Stream s = e.Result)
			{
				XDocument doc = XDocument.Load(s);

				foreach (XElement element in doc.Descendants("item"))
				{
					PodcastItem pi = new PodcastItem(){
						Title = element.Element("title").Value.ToString(),
						Description = element.Element("description").Value.ToString(),
						PubDate = PodcastItem.Helper.PubDate( element.Element("pubDate").Value.ToString()),
						Link = PodcastItem.Helper.Link(element.Element("link").Value.ToString())
					};
					list.Add(pi);
				}
			}
		}
		void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{

		}
	}
}
