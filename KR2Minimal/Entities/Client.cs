using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR2Minimal.Entities
{
    internal class Client
    {
        public Guid Client_id { get; private set; } = Guid.NewGuid();
        public string PersonalName { get; set; }
        public ulong TicketNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string? Education { get; set; }
        public Guid HallId { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();

        public Client(string personalName, ulong ticketNumber, DateTime birthday, string phoneNumber, string? education, Guid hallId)
        {
            PersonalName = personalName;
            TicketNumber = ticketNumber;
            Birthday = birthday;
            PhoneNumber = phoneNumber;
            Education = education;
            HallId = hallId;
        }

        public bool Equals(Client obj)
        {
            return Client_id.GetHashCode() == obj.Client_id.GetHashCode();
        }
    }
}
