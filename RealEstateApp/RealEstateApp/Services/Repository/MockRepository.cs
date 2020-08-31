using RealEstateApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealEstateApp.Services.Repository
{
    public class MockRepository : IRepository
    {
        private List<Agent> _agents;
        private List<Property> _properties;

        public MockRepository()
        {
            LoadProperties();
            LoadAgents();
        }

        public List<Agent> GetAgents()
        {
            return _agents;
        }

        public List<Property> GetProperties()
        {
            return _properties;
        }

        public void SaveProperty(Property property)
        {
            if (property.Id == null) throw new NullReferenceException("Property.Id cannot be null");

            var existing = _properties.FirstOrDefault(x => x.Id == property.Id);

            if (existing == null)
            {
                _properties.Add(property);
            }
            else
            {
                var existingIndex = _properties.IndexOf(existing);

                _properties[existingIndex] = property;
            }
        }

        private void LoadProperties()
        {
            _properties = new List<Property>
        {
            new Property
            {
                Id = "property_1", Address = "28 Anzac Ave, Collaroy NSW 2097 Australia",
                Description =
                    "This lovely house has 180 degree ocean views and features state of the art appliances.",
                Beds = 3, Baths = 1, Parking = 1, LandSize = 500, Price = 600000, AgentId = "agent_2",
                ImageUrls = GetPropertyImageUrls(1),
                Latitude = -33.739499, Longitude = 151.300683
            },
            new Property
            {
                Id = "property_2", Address = "1011 Pittwater Rd, Collaroy NSW 2097 Australia",
                Description =
                    "Situated in a prime location close to shops, this property represents excellent value for money.",
                Beds = 4, Baths = 2, Parking = 3, LandSize = 620, Price = 750000, AgentId = "agent_2",
                ImageUrls = GetPropertyImageUrls(2),
                Latitude =  -33.737897, Longitude = 151.302152
            },
            new Property
            {
                Id = "property_3", Address = "38 Ocean Grove, Collaroy NSW 2097 Australia",
                Description = "This near new architecturally designed house is a unique offering in the area.",
                Beds = 2, Baths = 2, Parking = 1, LandSize = 300, Price = 825000, AgentId = "agent_2",
                ImageUrls = GetPropertyImageUrls(3),
                Latitude = -33.737574, Longitude = 151.300586
            },
            new Property
            {
                Id = "property_4", Address = "28 Jenkins St, Collaroy NSW 2097 Australia",
                Description =
                    "Located in one of the most sought after streets in the area, this charming house has a lot going for it.",
                Beds = 3, Baths = 1, Parking = 3, LandSize = 450, Price = 480000, AgentId = "agent_1",
                ImageUrls = GetPropertyImageUrls(4),
                Latitude = -33.729635, Longitude = 151.298172
            },
            new Property
            {
                Id = "property_5", Address = "41 Alexander St, Collaroy NSW 2097 Australia",
                Description = "Newly renovated house in great location close to the beach.",
                Beds = 1, Baths = 1, Parking = 1, LandSize = 370, Price = 610000, AgentId = "agent_1",
                ImageUrls = GetPropertyImageUrls(5),
                Latitude = -33.732073, Longitude = 151.297265
            }
        };
        }

        private void LoadAgents()
        {
            _agents = new List<Agent>
        {
            new Agent
            {
                Id = "agent_2",
                Email = "sarah@realestate.com",
                Name = "Sarah Brown",
                Phone = "555 675 1456",
                ImageUrl = $"{GlobalSettings.Instance.ImageBaseUrl}agent_2.png"
            },
            new Agent
            {
                Id = "agent_1",
                Email = "bob@realestate.com",
                Name = "Bob Smith",
                Phone = "555 123 1234",
                ImageUrl = $"{GlobalSettings.Instance.ImageBaseUrl}agent_1.png"
            }
        };
        }

        private List<string> GetPropertyImageUrls(int index)
        {
            return new List<string>
        {
            $"{GlobalSettings.Instance.ImageBaseUrl}house_{index}.jpg",
            $"{GlobalSettings.Instance.ImageBaseUrl}kitchen_{index}.jpg",
            $"{GlobalSettings.Instance.ImageBaseUrl}bed_{index}.jpg"
        };
        }
    }
}