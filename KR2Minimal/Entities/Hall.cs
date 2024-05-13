using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR2Minimal.Entities
{
    internal class Hall
    {
        public Guid Hall_id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Specialization { get; set; }
        public ulong SeatsNumber { get; set; }
        public List<Client> Clients { get; set; } = new List<Client>();

        public Hall(string name, string specialization, ulong seatsNumber)
        {
            Name = name;
            Specialization = specialization;
            SeatsNumber = seatsNumber;
        }

        public bool Equals(Hall obj)
        {
            return Hall_id.GetHashCode() == obj.Hall_id.GetHashCode();
        }
    }
}
