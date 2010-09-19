using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SliverlightPodcast
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
            //PodcastItemCollection podcast = null;
            //podcast = App.Current.Resources["PodcastCollection"] as PodcastItemCollection;
            //PodcastList.ItemsSource = podcast;
//            podcast.SourceCompleted += new EventHandler(podcast_SourceCompleted);
		}

        void podcast_SourceCompleted(object sender, EventArgs e)
        {
            PodcastList.ItemsSource = (sender as PodcastItemCollection);
        }

        private void PodcastList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PodcastItem pi = (e.AddedItems).OfType<PodcastItem>().First<PodcastItem>();
        }
	}
}
