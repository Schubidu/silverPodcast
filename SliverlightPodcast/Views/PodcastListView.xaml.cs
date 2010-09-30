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
using System.Windows.Navigation;

namespace SliverlightPodcast.Views
{
    public partial class PodcastListView : Page
    {
        public PodcastListView()
        {
			InitializeComponent();
            if (Application.Current.InstallState == InstallState.Installed) {
                OobButton.Visibility = System.Windows.Visibility.Collapsed;
            }
		}

        void podcast_SourceCompleted(object sender, EventArgs e)
        {
            PodcastList.ItemsSource = (sender as PodcastItemCollection);
        }

        private bool isFirstDetailData = true;
        private void PodcastList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowPlayButton();
            ProgressBarLoading.Value = 0;
            if (isFirstDetailData)
            {
                NewDetailDataFirst.Begin();
                isFirstDetailData = false;
            }
            else 
            {
                NewDetailData.Begin();
            }
        }

        private void ShowPlayButton(bool showIt = true) {
            if (showIt)
            {
                PlayButton.Visibility = System.Windows.Visibility.Visible;
                PauseButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                PlayButton.Visibility = System.Windows.Visibility.Collapsed;
                PauseButton.Visibility = System.Windows.Visibility.Visible;
            }
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
            ShowPlayButton(false);
            ProgressBarPlaying.Maximum = this.MyMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Pause();
            ShowPlayButton();
        }

        private void MyMediaElement_DownloadProgressChanged(object sender, RoutedEventArgs e)
       {
           ProgressBarLoading.Value = MyMediaElement.DownloadProgress;
       }
	
    }
}
