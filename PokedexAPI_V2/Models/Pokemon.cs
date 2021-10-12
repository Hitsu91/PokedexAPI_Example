using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace PokedexAPI_V2.Models
{
    public class Pokemon : Entity
    {
        public string Name { get; set; }
        public string[] Types { get; set; }
        public double Weight { get; set; }
    }
}
