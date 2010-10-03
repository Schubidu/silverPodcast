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
using System.Xml.Serialization;

namespace SliverlightPodcast
{
    //[XmlRoot(ElementName="PodcastCategory")]
    public class PodcastCategoryItem
    {
        public string Title
        {
            get;
            set;
        }
        [XmlAttribute()]
        public int Id
        {
            get;
            set;
        }

        public PodcastUriCollection PodcastUriCollection { get; set; }
    }
}
