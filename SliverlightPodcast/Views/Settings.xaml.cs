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
    public partial class Settings : Page
    {
        private PodcastUriCollection pcuc;
        public Settings()
        {
            InitializeComponent();
            this.Unloaded += new RoutedEventHandler(Settings_Unloaded);
            pcuc = PodcastUriCollection.LoadCollection();
            LayoutRoot.DataContext = pcuc;
        }

        void Settings_Unloaded(object sender, RoutedEventArgs e)
        {
            pcuc.SaveCollection();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void UpdateFromServers_Click(object sender, RoutedEventArgs e)
        {
            pcuc.UpdateItems();

        }

        private void SaveUris_Click(object sender, RoutedEventArgs e)
        {
            pcuc.SaveCollection();
        }

        private void ResetUris_Click(object sender, RoutedEventArgs e)
        {
            pcuc.Reset();
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            PodcastUriCollection _pcuc = new PodcastUriCollection();
            foreach (var item in dataGrid1.SelectedItems)
            {
                _pcuc.Add((PodcastUriItem)item);
            }
            foreach (PodcastUriItem pui in _pcuc)
            {
                pcuc.Remove(pui);
            }
        }



        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            pcuc.Add(new PodcastUriItem());
        }

    }
}
