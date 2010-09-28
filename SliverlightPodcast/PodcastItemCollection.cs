﻿using System;
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
using System.Linq;
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
        int maxPodcastItems = 3;
        ObservableCollection<Uri> uris = new ObservableCollection<Uri>(){
                new Uri("http://www.ndr.de/ndr2/podcast2956.xml"),
                new Uri("http://www.ndr.de/podcastlink/angela.xml"),
                new Uri("http://www.ndr.de/n-joy/podcast4120.xml"),
                new Uri("http://www.ndr.de/ndr2/podcast2974.xml"),
                new Uri("http://www.br-online.de/podcast/tagebuch-des-taeglichen-wahnsinns/cast.xml")
        };

        ObservableCollection<ObservableCollection<PodcastItem>> temp;

        private bool _IsBusy;

        public bool IsBusy 
        {
            get {
                return _IsBusy;
            }
            set {
                _IsBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }


        public bool IsDataSource { 
            get {
                return true;
            }
            set {
               // this.Load();
 
            } 
        }
        
        int counter = 0;
        public void Load()
        {
            IsBusy = true;
            temp = new ObservableCollection<ObservableCollection<PodcastItem>>();
            foreach (Uri url in uris)
            {
                this.Load(url);
            }
        }

        private void Load(Uri url)
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
            if (e.Error == null)
            {
                using (Stream s = e.Result)
                {
                    ObservableCollection<PodcastItem> that = new ObservableCollection<PodcastItem>();

                    XNamespace itunesNameSpace = "http://www.itunes.com/DTDs/Podcast-1.0.dtd";

                    XDocument doc = XDocument.Load(s);

                    string imageUrl = "";
                    foreach (XElement element in doc.Descendants(itunesNameSpace + "link"))
                    {
                        imageUrl = element.Attribute("href").Value.ToString();
                        break;
                    }

                    if (imageUrl == "")
                    {
                        foreach (XElement element in doc.Descendants("image"))
                        {
                            imageUrl = element.Element("url").Value.ToString();
                            break;
                        }
                    }

                    
                    foreach (XElement element in doc.Descendants("title"))
                    {
                        //                   imageUrl = PodcastItem.Helper.ProxyUrl + element.Attribute("href").Value.ToString();
                        break;
                    }


                    BitmapImage bImage = new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute));


                    //  SyndicationFeed feed = 

                    foreach (XElement element in doc.Descendants("item"))
                    {
                        PodcastItem pi = new PodcastItem()
                        {
                            Title = element.Element("title").Value.ToString(),
                            Description = element.Element("description").Value.ToString(),
                            PubDate = PodcastItem.Helper.PubDate(element.Element("pubDate").Value.ToString()),
                            Link = PodcastItem.Helper.Link(element.Element("link").Value.ToString()),
                            ImageSource = bImage
                        };
                       that.Add(pi);
                    }

                    temp.Add(that);

                }
            }
            if (counter < uris.Count - 1)
            {
                this.counter++;
            }
            else
            {
                this.OnSourceCompleted();
            }
        }
		void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{

		}

       private void CompleteCollection() {
            List<PodcastItem> podcastList = new List<PodcastItem>();
            DateTime lastEntry = DateTime.Now.AddDays(-14);
            foreach (ObservableCollection<PodcastItem> podColl in this.temp)
            {
                foreach (PodcastItem pod in podColl)
                {
                    podcastList.Add(pod);
                }
            }
            podcastList = podcastList.Where(p => p.PubDate > lastEntry).OrderByDescending(p => p.PubDate).ToList();
            foreach (PodcastItem pod in podcastList)
            {
                this.Add(pod);
            }
        }

        private void OnSourceCompleted() {
            CompleteCollection();
            IsBusy = false;
        }
    }
}
