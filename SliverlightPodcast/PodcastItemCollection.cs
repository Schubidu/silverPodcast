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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Windows.Media.Imaging;
namespace SliverlightPodcast
{
	public class PodcastItemCollection : ObservableCollection<PodcastItem>
	{
		Uri url = new Uri("http://www.ndr.de/ndr2/podcast2956.xml");
//		Uri url = new Uri("podcast.xml", UriKind.Relative);
        private void Load()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(url);
        }
        
        public PodcastItemCollection()
		{
            this.Load();
		}

		void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error != null)
			{
			}
			using (Stream s = e.Result)
			{
                XNamespace itunesNameSpace = "http://www.itunes.com/DTDs/Podcast-1.0.dtd";
                
                XDocument doc = XDocument.Load(s);

                string imageUrl = "";
                foreach (XElement element in doc.Descendants(itunesNameSpace + "link")) {
                    imageUrl = PodcastItem.Helper.ProxyUrl + element.Attribute("href").Value.ToString();
                    break;
                }

                
                BitmapImage bImage = new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute));


             //  SyndicationFeed feed = 

				foreach (XElement element in doc.Descendants("item"))
				{
					PodcastItem pi = new PodcastItem(){
						Title = element.Element("title").Value.ToString(),
						Description = element.Element("description").Value.ToString(),
						PubDate = PodcastItem.Helper.PubDate( element.Element("pubDate").Value.ToString()),
						Link = PodcastItem.Helper.Link(element.Element("link").Value.ToString()),
                        ImageSource = bImage
					};
                    if (this.Count > 10) {
                        break;
                    }
					this.Add(pi);
				}
                this.OnSourceCompleted();
			}
		}
		void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{

		}

        private void OnSourceCompleted() {
            if (this.SourceCompleted != null) {
                this.SourceCompleted(this, new EventArgs());
            }
        }
        public event EventHandler SourceCompleted;
    }
}
