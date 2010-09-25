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
using System.IO.IsolatedStorage;

namespace SliverlightPodcast
{
	public partial class App : Application
	{
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

		public App()
		{
			this.Startup += this.Application_Startup;
			this.Exit += this.Application_Exit;
			this.UnhandledException += this.Application_UnhandledException;
            if (Application.Current.IsRunningOutOfBrowser)
            {
                App.Current.CheckAndDownloadUpdateCompleted += new CheckAndDownloadUpdateCompletedEventHandler(Current_CheckAndDownloadUpdateCompleted);
                App.Current.CheckAndDownloadUpdateAsync();

            }

			InitializeComponent();
		}

        void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.Error == null && e.UpdateAvailable)
            {
                MessageBox.Show("Die Anwendung wurde aktualisiert. Bitte starten Sie die Anwendung neu.");
            }
            else
            {
                if(e.Error != null)
                    MessageBox.Show(e.Error.ToString() + ".:" + e.Error.Message);
            }


        }

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			this.RootVisual = new MainPage();
            if (appSettings.Contains("proxyUrl")) {
                appSettings.Remove("proxyUrl");
            }

            //this.Resources.Add("PodcastCollection", new PodcastItemCollection());
            appSettings.Add("proxyUrl", "http://playground.dev.schult.info/podcastPlayer/proxy.php?u=");
		}

 		private void Application_Exit(object sender, EventArgs e)
		{

		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			// If the app is running outside of the debugger then report the exception using
			// the browser's exception mechanism. On IE this will display it a yellow alert 
			// icon in the status bar and Firefox will display a script error.
			if (!System.Diagnostics.Debugger.IsAttached)
			{

				// NOTE: This will allow the application to continue running after an exception has been thrown
				// but not handled. 
				// For production applications this error handling should be replaced with something that will 
				// report the error to the website and stop the application.
				e.Handled = true;
				Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
			}
		}

		private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
		{
			try
			{
				string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
				errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

				System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
			}
			catch (Exception)
			{
			}
		}
	}
}
