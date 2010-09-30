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
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;
using System.IO;

namespace SliverlightPodcast
{
    public class PodcastUriCollection : ObservableCollection<PodcastUriItem>
    {
        public PodcastUriCollection()
        {
        }

        internal static string filename = "podcastUriCollection.xml";

        public void SaveCollection()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(PodcastUriCollection.filename, FileMode.Create, isf))
                {

                    XmlSerializer xs = new XmlSerializer(typeof(PodcastUriCollection));
                    using (StreamWriter sw = new StreamWriter(isfs))
                    {
                       xs.Serialize(sw, this);

                        sw.Close();

                    }

                }

            }

        }
        public static PodcastUriCollection LoadCollection()
        {
            PodcastUriCollection pcuc = null;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (!isf.FileExists(PodcastUriCollection.filename))
                {
                    pcuc = new PodcastUriCollection();
                    pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/ndr2/podcast2956.xml"), IsAvailable=true });
                    pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/podcastlink/angela.xml"), IsAvailable = true });
                    pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/n-joy/podcast4120.xml"), IsAvailable = true });
                    pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/ndr2/podcast2974.xml"), IsAvailable = true });
                    pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.br-online.de/podcast/tagebuch-des-taeglichen-wahnsinns/cast.xml"), IsAvailable = true });
                    pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.tagesschau.de/export/video-podcast/tagesschau/"), IsAvailable = true });
                }
                else
                {
                    using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(PodcastUriCollection.filename, FileMode.Open, isf))
                    {

                        try
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(PodcastUriCollection));
                            using (StreamWriter sw = new StreamWriter(isfs))
                            {

                                using (StreamReader sr = new StreamReader(isfs))
                                {
                                    pcuc = xs.Deserialize(sr) as PodcastUriCollection;
                                   
                                }

                            }
                        }
                        catch (ObjectDisposedException ex) { }

                    }
                }

            }

            return pcuc;

        }


    }

}
