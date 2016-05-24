using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetrinetTool;

namespace EnvironmentMonitor
{
    public class HomeModule : IEquatable<HomeModule>
    {
        public string Name { get; set; }
        public string StateDesc { get; set; }
        public string Description
        {
            get
            {
                return string.Format("{0}({1})", Name, StateDesc);
            }
        }

        public Place Place
        {
            get
            {
                return new Place() { Name = this.Name, Id = Description };
            }
        }

        public bool Equals(HomeModule other)
        {
            if (other == null) return false;
            return (other.Name == this.Name && other.StateDesc == this.StateDesc);
        }
    }
}
