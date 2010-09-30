using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace SliverlightPodcast
{
    [XmlRoot("PodcastUriItem", IsNullable = false)]
    public class PodcastUriItem : INotifyPropertyChanged
    {
        
        public PodcastUriItem() { 
        }
        private Uri _Link = null;
        private string _Title = "";
        private bool _IsBusy = false;
        private bool _IsAvailable = false;
        private bool _CanAccess = false;

    
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
        [Required(ErrorMessage="Please add an valide URL.")]
        public string LinkString
        {
            get
            {
                if (Link != null)
                {
                    return this.Link.ToString();
                }
                else
                {
                    return String.Empty;
                }

            }
            set
            {
                try
                {
                    Link = new Uri(value);
                }
                catch (UriFormatException ex) {
                    Link = null;
                }
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

        [XmlAttribute("CanAccess")]
        public bool CanAccess
        {
            get
            {
                return _CanAccess;
            }
            set
            {
                _CanAccess = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CanAccess"));
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


        public void UpdateItem()
        {
            IsBusy = true;
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(this.Link);
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                using (Stream s = e.Result)
                {
                    try
                    {
                        XDocument doc = XDocument.Load(s);
                        this.Title = (doc.Descendants("title").First() as XElement).Value.ToString();
                        this.IsAvailable = true;
                    }
                   catch (Exception ex)
                    {
                        this.Title = "[Unable to load Title]";
                        this.IsAvailable = false;
                    }
                }
            }
            else
            {
                this.Title = "[Unable to load Title]";
                this.IsAvailable = false;
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


        

        
    }
}
