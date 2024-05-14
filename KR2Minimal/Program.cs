using KR2Minimal.Entities;

string[] paths = { "halls.txt", "clients.txt", "books.txt"  };
int[] m = { 8, 3, 6 };

List<Book> books = new List<Book>();
List<Hall> halls = new List<Hall>();
List<Client> clients = new List<Client>();

for (int i = 0; i < 3; i++)
{
    FileLoad(paths[i], i, m[i]);
}

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
            Console.WriteLine("Book title: \n");
            var bookname = Console.ReadLine();

            var flag = IsAvailableBook(bookname);

            QueryEnd();
            continue;
        case 4:
            Console.Clear();
            Console.WriteLine("Book author: \n");
            string author = Console.ReadLine();
            Console.WriteLine("Hall name: \n");
            string hallname = Console.ReadLine();

            var count = AuthorBooksCount(author, hallname);
            Console.WriteLine(author + ": " + count);
            QueryEnd();
            continue;
        case 5:
            ExecuteQuery(ClietnsWithUniqueBooks);
            continue;

        case 6:
            ExecuteQuery(PopularBook);
            continue;
        case 7:
            ExecuteQuery(AddClient);
            continue;
        case 8:
            ExecuteQuery(RemoveBook);
            continue;
        case 9:
            ExecuteQuery(AddBook);
            continue;
        case 10:
            ExecuteQuery(WriteBooks);
            continue;
        case 11:
            ExecuteQuery(WriteHalls);
            continue;
        case 12:
            ExecuteQuery(WriteClients);
            continue;
        case 13:
            ExecuteQuery(WriteToDatabase);
            continue;
        default:
            Environment.Exit(0);
            break;
    }
    Console.ReadKey();
}

void WriteToDatabase()
{

}
//10
void WriteBooks()
{
    books.ForEach(b =>
    {
        Console.WriteLine(b.Title);
        Console.WriteLine(b.Author);
        Console.WriteLine(b.Count + "\n");
    });
}
//11
void WriteHalls()
{
    halls.ForEach(h =>
    {
        Console.WriteLine(h.Name);
        Console.WriteLine(h.SeatsNumber);
        Console.WriteLine(h.Specialization + "\n");
    });
}
//12
void WriteClients()
{
    clients.ForEach(c =>
    {
        Console.WriteLine(c.PersonalName);
        Console.WriteLine(c.Birthday);
        Console.WriteLine(c.TicketNumber + "\n");
    });
}

void FileLoad(string path, int n, int m)
{
    StreamReader FileIn = new StreamReader(path);
    string str;
    switch (n)
    {
        case 2:
            while((str = FileIn.ReadLine()) != null)
            {
                string[] ms = new string[m];
                ms = str.Split(';');
                DateTime publishingDate = DateTime.ParseExact(ms[2], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime receivingDate = DateTime.ParseExact(ms[4], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ulong count = (ulong)int.Parse(ms[5]);
                var clientsNames = ms[6].Split(',');
                var hall = halls.Find(h => h.Name == ms[7]);
                var book = new Book(ms[0], ms[1], publishingDate, ms[3], receivingDate, count);
                books.Add(book);
                book.Hall_id = hall.Hall_id;
                if(clientsNames.Length > 0 )
                    foreach(var cn in clientsNames)
                    {
                        var client = clients.Find(c => c.PersonalName == cn);
                        client.Books.Add(book);
                        book.Clients.Add(client);
                    }
            }
            break;
        case 0:
            while ((str = FileIn.ReadLine()) != null)
            {
                string[] ms = new string[m];
                ms = str.Split(';');
                ulong seats = (ulong)int.Parse(ms[2]);
                halls.Add(new Hall(ms[0], ms[1], seats));
            }
            break;
        case 1:
            while ((str = FileIn.ReadLine()) != null)
            {
                string[] ms = new string[m];
                ms = str.Split(';');
                ulong count = (ulong)int.Parse(ms[1]);
                DateTime birthdayDate = DateTime.ParseExact(ms[2], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                var hall = halls.Find(h => h.Name == ms[5]);
                var guid = hall.Hall_id;
                var client = new Client(ms[0], count, birthdayDate, ms[3], ms[4], guid);
                clients.Add(client);
                hall.Clients.Add(client);
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
        Console.WriteLine(hall.Name);
        Console.WriteLine("Free: " + (hall.SeatsNumber - (ulong)hall.Clients.Count) + "\n");
    });
}

//3
bool IsAvailableBook(string bookName)
{
    var book = books.Find(b => b.Title == bookName);
    if(book is null)
    {
        Console.WriteLine("Такой книги нет");
        return false;
    }
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
    if(hall is null)
    {
        Console.WriteLine("Такого зала нет");
        return 0;
    }
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
    Console.WriteLine(popularBook.Title + ", Count: " + popularBook.Clients.Count);
}

//7
void AddClient()
{
    try
    {
        Console.WriteLine("Personal name: ");
        var personalName = Console.ReadLine();
        Console.WriteLine("Ticket number: ");
        ulong ticket = (ulong)int.Parse(Console.ReadLine());
        Console.WriteLine("Birthday date: ");
        DateTime birthdayDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        Console.WriteLine("Phone: ");
        var phone = Console.ReadLine();
        Console.WriteLine("Education: ");
        var education = Console.ReadLine();
        Console.WriteLine("Hall is");
        var hallName = Console.ReadLine();
        var hall = halls.Find(h => h.Name == hallName);
        clients.Add(new Client(personalName, ticket, birthdayDate, phone, education, hall.Hall_id));
    }
    catch
    {
        Console.WriteLine("Error");
    }
}

//8
void RemoveBook()
{
    Console.WriteLine("Book name: ");
    var bookName = Console.ReadLine();
    var book = books.Find(b => b.Title == bookName);
    if (book is null)
    {
        Console.WriteLine("Error");
    }
    foreach(var c in book.Clients)
    {
        c.Books.Remove(book);
    }
    books.Remove(book);
    Console.WriteLine("Book is removed");
}

//9
void AddBook()
{
    Console.WriteLine("Title: ");
    var title = Console.ReadLine();
    Console.WriteLine("Author: ");
    var author = Console.ReadLine();
    Console.WriteLine("Publishing date: ");
    DateTime publishingDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    Console.WriteLine("ISBN: ");
    var isbn = Console.ReadLine();
    Console.WriteLine("Receiving date: ");
    DateTime receivingDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    Console.WriteLine("Count: ");
    var count = (ulong)int.Parse(Console.ReadLine());
    Console.WriteLine("Hall name: ");
    var hallName = Console.ReadLine();
    var book = new Book(title, author, publishingDate, isbn, receivingDate, count);
    books.Add(book);
    var hall = halls.Find(h => h.Name == hallName);
    book.Hall_id = hall.Hall_id;
}

static void Frame() //метод для выбора списка запросов
{
    Console.WriteLine("\tВыберете номер задачи\n");
    Console.WriteLine(" 1 - Какие книги выданы каждому читателю\n");
    Console.WriteLine(" 2 - Сколько свободных мест в каждом зале\n");
    Console.WriteLine(" 3 - Можно ли выдать книгу читателю\n");
    Console.WriteLine(" 4 - количество книг заданного автора в читальном зале\n");
    Console.WriteLine(" 5 - читетели, взявшие книги, имеющиеся в одном экземпляре\n");
    Console.WriteLine(" 6 - книга с максимальным рейтингом\n");
    Console.WriteLine(" 7 - запись в библиотеку нового читателя\n");
    Console.WriteLine(" 8 - списать старую или потерянную книгу\n");
    Console.WriteLine(" 9 - принять книгу в фонд библиотеки\n");
    Console.WriteLine(" 10 - все книги\n");
    Console.WriteLine(" 11 - все залы\n");
    Console.WriteLine(" 12 - все клиенты\n");
    Console.WriteLine("default - Выход из программы");
}