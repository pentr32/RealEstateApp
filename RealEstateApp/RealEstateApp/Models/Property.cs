using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

// Install-Package Fody
// Install-Package PropertyChanged.Fody
// Tilføj attribute:  [AddINotifyPropertyChangedInterface]

namespace RealEstateApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Property
    {
        public Property()
        {
            Id = Guid.NewGuid().ToString();

            ImageUrls = new List<string>();
        }

        public string Id { get; set; }
        public string Address { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
        public int? Beds { get; set; }
        public int? Baths { get; set; }
        public int? Parking { get; set; }
        public int? LandSize { get; set; }
        public string AgentId { get; set; }
        public List<string> ImageUrls { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string MainImageUrl => ImageUrls?.FirstOrDefault() ?? GlobalSettings.Instance.NoImageUrl;
    }
}
