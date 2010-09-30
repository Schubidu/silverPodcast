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
using System.ComponentModel;
using System.Xml.Serialization;

namespace SliverlightPodcast
{
    public class LinkCheckedEventArgs : EventArgs
    {
        public Uri Link { get; set; }

    }
    public delegate void LinkCheckedEventHandler (object sender, LinkCheckedEventArgs e);

    [XmlRoot("PodcastUriItem", IsNullable = false)]
    public class PodcastUriItem : INotifyPropertyChanged
    {
        
        public PodcastUriItem() { 
        }
        private Uri _Link = null;
        private string _Title = "";
        private bool _IsBusy = false;
        private bool _IsAvailable = false;

    
        [XmlIgnore]
        public Uri Link
        {
            get
            {
                return _Link;
            }
            set
            {
                _Link = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Link"));
            }
        }

    
        [XmlElement("Link")]
        public string LinkString
        {
            get
            {
                return this.Link.ToString();
            }
            set
            {
                Link = new Uri(value);
                OnPropertyChanged(new PropertyChangedEventArgs("LinkString"));
            }
        }
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

        [XmlAttribute("IsAvailable")]
        public bool IsAvailable
        {
            get
            {
                return _IsAvailable;
            }
            set
            {
                _IsAvailable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAvailable"));
            }
        }

        [XmlIgnore]
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }


        public void CheckLink()
        {
            IsBusy = true;
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null) {
                OnLinkChecked(new LinkCheckedEventArgs { Link = this.Link });
            }
        }

        private void OnLinkChecked(LinkCheckedEventArgs linkCheckedEventArgs)
        {
            if (LinkChecked != null) 
            {
                LinkChecked(this, linkCheckedEventArgs);
            }
        }
        


        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, propertyChangedEventArgs);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        

        public event LinkCheckedEventHandler LinkChecked;
        
    }
}
