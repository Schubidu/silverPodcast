using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace SliverlightPodcast
{
    public class PodcastItemCollection : ObservableCollection<PodcastItem>
    {
        private PodcastUriCollection _Uris = null;
  
        ObservableCollection<ObservableCollection<PodcastItem>> temp;

        private bool _IsBusy;

        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }


        public bool IsDataSource
        {
            get
            {
                return true;
            }
            set
            {
                // this.Load();

            }
        }


       public PodcastUriCollection Uris
        {
            get
            {
                return _Uris;
            }
            set
            {
                _Uris = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Uris"));
            }
        }

        int counter = 0;
        public void Load()
        {
            IsBusy = true;
            temp = new ObservableCollection<ObservableCollection<PodcastItem>>();
            foreach (PodcastUriItem pu in Uris)
            {
                if (pu.IsAvailable)
                {
                    this.Load(pu.Link);
                }
                else
                {
                    if (counter < Uris.Count - 1)
                    {
                        this.counter++;
                    }
                    else
                    {
                        this.OnSourceCompleted();
                    }
                }
            }
        }

        private void Load(Uri url)
        {
            WebClient client = new WebClient();
            //IsBusy = client.IsBusy;
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(url);
        }



        public PodcastItemCollection()
        {
            Uris = PodcastUriCollection.LoadCollection();
            this.Load();
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                using (Stream s = e.Result)
                {
                    ObservableCollection<PodcastItem> that = new ObservableCollection<PodcastItem>();
                    XDocument doc = XDocument.Load(s);

                    BitmapImage bImage;
                    bool hasBImage = XmlHelper.TryGetImage(doc, out bImage);
                    if (!hasBImage) throw new PodcastItemCollectionException();

                    string copyright;
                    bool hasCopyright = XmlHelper.TryGetCopyright(doc, out copyright);
                    if (!hasCopyright) throw new PodcastItemCollectionException();



                    //  SyndicationFeed feed = 

                    foreach (XElement element in doc.Descendants("item"))
                    {
                        Uri link;
                        bool hasLink = XmlHelper.TryGetItemLink(element, out link);
                        if (!hasLink) throw new PodcastItemCollectionException();

                        DateTime pubDate;
                        bool hasPubDate = XmlHelper.TryGetItemPubDate(element, out pubDate);
                        if (!hasPubDate) throw new PodcastItemCollectionException();


                        string description;
                        bool hasDescription = XmlHelper.TryGetItemDescription(element, out description);
                        if (!hasDescription) throw new PodcastItemCollectionException();


                        string title;
                        bool hasTitle = XmlHelper.TryGetItemTitle(element, out title);
                        if (!hasTitle) throw new PodcastItemCollectionException();

                        PodcastItem pi = new PodcastItem()
                       {
                           Title = title,
                           Description = description,
                           PubDate = pubDate,
                           Link = link,
                           ImageSource = bImage,
                           Copyright = copyright
                       };
                        that.Add(pi);
                    }

                    temp.Add(that);

                }
            }
            if (counter < Uris.Count - 1)
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

        private void CompleteCollection()
        {
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

            //Uris.SaveCollection();
            
        }

        private void OnSourceCompleted()
        {
            CompleteCollection();
            IsBusy = false;
        }


        private class XmlHelper
        {

            static private XNamespace itunesNameSpace = "http://www.itunes.com/DTDs/Podcast-1.0.dtd";

            static internal bool TryGetImage(XDocument doc, out BitmapImage image)
            {
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

                if (imageUrl != "")
                {
                    image = new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute));
                    return true;
                }

                image = null;
                return false;
            }

            static internal bool TryGetCopyright(XDocument doc, out string copyright)
            {
                copyright = "";

                try
                {
                    copyright = (doc.Descendants("copyright").First() as XElement).Value.ToString();
                }
                catch (Exception ex) { ;}

                if (copyright != "")
                {
                    return true;
                }
                return false;
            }

            static internal bool TryGetItemLink(XElement element, out Uri link)
            {

                string linkString = "";
                try
                {
                    linkString = element.Element("link").Value.ToString();
                }
                catch (Exception ex) { }

                if (linkString == "")
                {
                    linkString = element.Element("enclosure").Attribute("url").Value.ToString();
                }

                try
                {
                    link = new Uri(linkString, UriKind.RelativeOrAbsolute);
                    return true;
                }
                catch (UriFormatException ex)
                {
                    link = null;
                    return false;
                }
            }

            internal static bool TryGetItemPubDate(XElement element, out DateTime pubDate)
            {
                return DateTime.TryParse(element.Element("pubDate").Value.ToString(), out pubDate);
            }

            internal static bool TryGetItemDescription(XElement element, out string description)
            {
                try
                {
                    description = element.Element("description").Value.ToString();
                    return true;
                }
                catch (Exception ex)
                {
                    description = "";
                    return false;
                }
            }

            internal static bool TryGetItemTitle(XElement element, out string title)
            {
                try
                {
                    title = element.Element("title").Value.ToString();
                    return true;
                }
                catch (Exception ex)
                {

                    title = "";
                    return false;
                }
            }
        }


    }

    public class PodcastItemCollectionException : Exception
    {
    }


}
