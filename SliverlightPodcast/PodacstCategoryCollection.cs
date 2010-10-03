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
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;

namespace SliverlightPodcast
{
    //[XmlRoot("PodcastCategoryCollection")]
    public class PodacstCategoryCollection : ObservableCollection<PodcastCategoryItem>, IPodcastCollection
    {
        public static PodacstCategoryCollection LoadCollection()
        {
            PodacstCategoryCollection pcc = new PodacstCategoryCollection();
            pcc.Add(new PodcastCategoryItem() { Title = "CategoryTitle", PodcastUriCollection=PodcastUriCollection.LoadCollection() });
            return pcc;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        internal static string filename = "podcastCategoryCollection.xml";

        public void SaveCollection()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(PodacstCategoryCollection.filename, FileMode.Create, isf))
                {
                    this.SaveCollection(isfs);
                }

            }

        }

        public void SaveCollection(Stream stream)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(PodacstCategoryCollection));
            using (StreamWriter sw = new StreamWriter(stream))
            {
                xs.Serialize(sw, this);
                sw.Close();

            }
        }

        public void UpdateItems()
        {
            throw new NotImplementedException();
        }
    }
}
