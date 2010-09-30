using System;
using System.Windows.Media.Imaging;

namespace SliverlightPodcast
{
    public class PodcastItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public DateTime PubDate { get; set; }
        public Uri Link { get; set; }
        public BitmapImage ImageSource { get; set; }
        public long Id
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Title + "\n" + this.Description;
        }
    }

}
