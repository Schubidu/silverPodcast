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
using System.Diagnostics;

namespace SliverlightPodcast.Controls
{
    public class Player : Control
    {
        StackPanel layoutRoot;
        
        public Player() : base() {
            DefaultStyleKey = typeof(Player);
        }

        public override void OnApplyTemplate()
        {
            this.layoutRoot = this.GetTemplateChild("LayoutRoot") as StackPanel;
            Debug.Assert(this.layoutRoot != null, "LayoutRoot is null");
            base.OnApplyTemplate();
        }




        #region ItemSource (DependencyProperty)

        /// <summary>
        /// A description of the property.
        /// </summary>
        public PodcastItem ItemSource
        {
            get { return (PodcastItem)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(PodcastItem), typeof(Player),
            new PropertyMetadata(0, new PropertyChangedCallback(OnItemSourceChanged)));

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Player)d).OnItemSourceChanged(e);
        }

        protected virtual void OnItemSourceChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion
        
 
        

    }
}
