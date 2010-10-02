using System.Windows;
using System.Windows.Controls;
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
            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            
            base.OnNavigatingFrom(e);
            pcuc.SaveCollection();
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

        private void DownloadSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog textDialog = new SaveFileDialog();
            textDialog.Filter = "XML Files | *.xml";
            textDialog.DefaultExt = "xml";

            bool? result = textDialog.ShowDialog();
            if (result == true)
            {
                System.IO.Stream fileStream = textDialog.OpenFile();
                pcuc.SaveCollection(fileStream);
                
            }
            
       }

        private void UploadSettings_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog textDialog = new OpenFileDialog();

            // Set filter options and filter index.
            textDialog.Filter = "XML Files (.xml)|*.xml|All Files (*.*)|*.*";
            textDialog.FilterIndex = 1;

            textDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = textDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = textDialog.File.OpenRead();
                PodcastUriCollection _pcuc = PodcastUriCollection.LoadCollection(fileStream);
                if (_pcuc != null)
                {
                    pcuc = _pcuc;
                    LayoutRoot.DataContext = pcuc;
                }
                
            }


        }

    }
}
