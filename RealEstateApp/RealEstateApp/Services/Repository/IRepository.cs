using RealEstateApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateApp.Services.Repository
{
    public interface IRepository
    {
        List<Agent> GetAgents();
        List<Property> GetProperties();
        void SaveProperty(Property property);
    }
}
