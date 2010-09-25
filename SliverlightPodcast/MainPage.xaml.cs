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
            if (Application.Current.InstallState == InstallState.Installed) {
                OobButton.Visibility = System.Windows.Visibility.Collapsed;
            }
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
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            ProgressBarLoading.Value = 0;
        }

        private void OobButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Install();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            //(this.Resources.First().Value as PodcastItemCollection).Refresh();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Play();
            PlayButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
            ProgressBarPlaying.Maximum = this.MyMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Pause();
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }

        private void MyMediaElement_DownloadProgressChanged(object sender, RoutedEventArgs e)
       {
           ProgressBarLoading.Value = MyMediaElement.DownloadProgress;
       }
	}
}
