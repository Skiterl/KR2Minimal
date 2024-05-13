using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR2Minimal.Entities
{
    internal class Book
    {
        public Book(string title, string author, DateTime publishingDate, string iSBN, DateTime receivingDate, ulong count)
        {
            Title = title;
            Author = author;
            PublishingDate = publishingDate;
            ISBN = iSBN;
            ReceivingDate = receivingDate;
            Count = count;
        }
        public Guid Book_id { get; private set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishingDate { get; set; }
        public string ISBN { get; set; }
        public DateTime ReceivingDate { get; set; }
        public ulong Count { get; set; }
        public List<Client> Clients { get; set; } = new List<Client>();
        public Guid Hall_id { get; set; }

        public bool Equals(Book obj)
        {
            return Book_id.GetHashCode() == obj.Book_id.GetHashCode();
        }
    }
}
