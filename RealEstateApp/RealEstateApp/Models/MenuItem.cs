using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateApp.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type Target { get; set; }
    }
}
