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
using System.Collections.Specialized;

namespace SliverlightPodcast
{
    public class PodcastUriCollection : ObservableCollection<PodcastUriItem>
    {
        public PodcastUriCollection()
        {
           
        }

        public void UpdateItems() {
            foreach (PodcastUriItem pui in this)
            {
                pui.IsAvailable = false;
                pui.UpdateItem();
            }
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

        private static PodcastUriCollection ResetCollection()
        {
            PodcastUriCollection pcuc = new PodcastUriCollection();
            pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/ndr2/podcast2956.xml"), IsAvailable = true });
            pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/podcastlink/angela.xml"), IsAvailable = true });
            pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/n-joy/podcast4120.xml"), IsAvailable = true });
            pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.ndr.de/ndr2/podcast2974.xml"), IsAvailable = true });
            pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.br-online.de/podcast/tagebuch-des-taeglichen-wahnsinns/cast.xml"), IsAvailable = true });
            pcuc.Add(new PodcastUriItem { Link = new Uri("http://www.tagesschau.de/export/video-podcast/tagesschau/"), IsAvailable = true });
            return pcuc;
        }

        public void Reset() {

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
               if (isf.FileExists(PodcastUriCollection.filename))
                {
                    isf.DeleteFile(PodcastUriCollection.filename);
               }
            }

            this.Clear();

            foreach (PodcastUriItem pui in PodcastUriCollection.ResetCollection())
            {
                this.Add(pui);
            }
        }

        public static PodcastUriCollection LoadCollection()
        {
            PodcastUriCollection pcuc = null;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (!isf.FileExists(PodcastUriCollection.filename))
                {
                    pcuc = PodcastUriCollection.ResetCollection();
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
