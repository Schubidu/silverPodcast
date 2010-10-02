using System;
namespace SliverlightPodcast
{
    interface IPodcastCollection
    {
        void Reset();
        void SaveCollection();
        void SaveCollection(System.IO.Stream stream);
        void UpdateItems();
    }
}
