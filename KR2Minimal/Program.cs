using KR2Minimal.Entities;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.CompilerServices;

var book1 = new Book("War and Peace", "Tolstoy", new DateTime(), "14163146", DateTime.Now, 50);
var book2 = new Book("Dead souls", "Gogol", new DateTime(), "34532462346", DateTime.Now, 40);
var book3 = new Book("Schatten im Paradies", "Erich Maria Remarque", new DateTime(), "34534568356346", DateTime.Now, 1);

string[] paths = { "books.txt", "halls.txt", "clients.txt" };
int[] m = { 9, 5, 8 };

for (int i = 0; i< 3; i++)
{

}

List<Book> books = new List<Book>() 
{
    book1, book2, book3
};

var hall1 = new Hall("Pushkina", "Fiction", 345);
var hall2 = new Hall("Einshtein", "Science", 500);

List<Hall> halls = new List<Hall>() 
{ 
    hall1, hall2
};

book1.Hall_id = hall1.Hall_id;
book2.Hall_id = hall2.Hall_id;

var client1 = new Client("Max", 346346, DateTime.Now, "456364364", null, hall1.Hall_id);
hall1.Clients.Add(client1);

var client2 = new Client("Bob", 323457, DateTime.Now, "34346345", null, hall2.Hall_id);
hall2.Clients.Add(client2);

List<Client> clients = new List<Client>() 
{ 
    client1, client2
};

client1.Books.Add(book1);
book1.Clients.Add(client1);

client1.Books.Add(book2);
book2.Clients.Add(client1);

client1.Books.Add(book3);
book3.Clients.Add(client1);

client2.Books.Add(book2);
book2.Clients.Add(client2);


while (true)
{
    Frame();
    if(!int.TryParse(Console.ReadLine(), out var n))
    {
        Console.WriteLine("Incorrect input");
        Console.ReadLine();
        Console.Clear();
        continue;
    }

    switch (n)
    {
        case 1:
            ExecuteQuery(GetClientsBooks);
            continue;
        case 2:
            ExecuteQuery(FreeSeats);
            continue;
        case 3:
            Console.Clear();
            var bookname = Console.ReadLine();

            IsAvailableBook(bookname);

            QueryEnd();
            continue;
        case 4:
            Console.Clear();

            string author = Console.ReadLine();
            string hallname = Console.ReadLine();

            AuthorBooksCount(author, hallname);

            QueryEnd();
            continue;
        case 5:
            ExecuteQuery(ClietnsWithUniqueBooks);
            continue;

        case 6:
            ExecuteQuery(PopularBook);
            continue;
    }
}

void FileLoad(string path, int n, int m)
{
    StreamReader FileIn = new StreamReader(path);
    string str;
    switch (n)
    {
        case 0:
            while((str = FileIn.ReadLine()) != null)
            {
                string[] ms = new string[m];
                ms = str.Split(';');
                DateTime publishingDate = DateTime.ParseExact(ms[2], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime receivingDate = DateTime.ParseExact(ms[4], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ulong count = (ulong)int.Parse(ms[5]);
                books.Add(new Book(ms[0], ms[1], publishingDate, ms[3], receivingDate, count));
            }
            break;
        case 1:
            while ((str = FileIn.ReadLine()) != null)
            {
                string[] ms = new string[m];
                ms = str.Split(';');
                ulong seats = (ulong)int.Parse(ms[2]);
                halls.Add(new Hall(ms[0], ms[1], seats));
            }
            break;
        case 2:
            while ((str = FileIn.ReadLine()) != null)
            {
                string[] ms = new string[m];
                ms = str.Split(';');
                ulong seats = (ulong)int.Parse(ms[2]);
                halls.Add(new Hall(ms[0], ms[1], seats));
            }
            break;
    }
}


void ExecuteQuery(Action func)
{
    Console.Clear();
    func();
    QueryEnd();
}

void QueryEnd()
{
    Console.WriteLine("Query is end");
    Console.ReadKey();
    Console.Clear();
}


//1
void GetClientsBooks()
{
    clients.ForEach(client =>
    {
        Console.WriteLine(client.PersonalName);
        client.Books.ForEach(b => Console.WriteLine(b.Title));
        Console.WriteLine();
    });
}

//2
void FreeSeats()
{
    halls.ForEach(hall =>
    {
        Console.WriteLine("Free: " + (hall.SeatsNumber - (ulong)hall.Clients.Count));
    });
}

//3
bool IsAvailableBook(string bookName)
{
    var book = books.Find(b => b.Title == bookName);

    if ((ulong)book.Clients.Count == book.Count)
    {
        Console.WriteLine("Свободных экземпляров нет");
        return false;
    }
    Console.WriteLine("Есть свободные экземпляры");
    return true;
}

//4
int AuthorBooksCount(string author, string hallname)
{
    var hall = halls.Find(h => h.Name == hallname);

    int s = (int)books.Where(book => book.Hall_id == hall.Hall_id && book.Author == author).Sum(b => (float)b.Count);
    return s;
}

//5
void ClietnsWithUniqueBooks()
{
    var clientsRare = clients.Where(c => c.Books.Find(b => b.Count == 1) is not null);
    foreach (var c in clientsRare)
    {
        Console.WriteLine(c.PersonalName);
    }
}

//6
void PopularBook()
{
    var popularBook = books.MaxBy(b => b.Clients.Count);
    Console.WriteLine(popularBook.Title + " " + popularBook.Clients.Count);
}

static void Frame() //метод для выбора списка запросов
{
    Console.WriteLine("\tВыберете номер задачи\n");
    Console.WriteLine(" 1 - Получить справку о конкретном клиенте\n");
    Console.WriteLine(" 2 - Суммарная стоимость товара в отделе\n");
    Console.WriteLine("\tДругие запросы");
    Console.WriteLine("\n9 - Выход из программы");
}