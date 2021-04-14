using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Bookshop
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new BookContext();
            var library = context.Books.FromSqlRaw("select * from Books").ToList();
            Console.WriteLine("Use commands get and buy");
            while (true)
            {
                var input = Console.ReadLine();
                var cmd = input.Split("--");
                switch (cmd[0])
                {
                    case "get":
                        if (cmd.Length == 1)
                        {
                            foreach (Book bo in library)
                                Console.WriteLine(bo.Id + " " + bo.author + " " + bo.name + " " + bo.date);
                        }
                        else
                        {
                            List<Book> completedlist = new List<Book>();
                            List<Book> query = new List<Book>();
                            try
                            {
                                for (int i = 1; i < cmd.Length; i++)
                                {
                                    var cmdsplitted = cmd[i].Split("=");
                                    switch (cmdsplitted[0])
                                    {
                                        case "author":
                                            query =(from b in library where b.author == cmdsplitted[1] select b).ToList();
                                            if (query.Count==0)
                                            {
                                                Console.WriteLine("No such author named " + cmdsplitted[1]);
                                            }
                                            else completedlist.AddRange(query);
                                            break;
                                        case "title":
                                            query = (from b in library where b.name.Contains(cmdsplitted[1]) select b).ToList();
                                            if (query.Count == 0)
                                            {
                                                Console.WriteLine("No such title that contains " + cmdsplitted[1]);
                                            }
                                            else completedlist.AddRange(query);
                                            break;
                                        case "date":
                                            query = (from b in library where b.date == cmdsplitted[1] select b).ToList();
                                            if (query.Count == 0)
                                            {
                                                Console.WriteLine("No such date that equals " + cmdsplitted[1]);
                                            }
                                            else completedlist.AddRange(query);
                                            break;
                                        case "order-by":
                                            if (completedlist.Count==0) 
                                            {
                                                completedlist = library;
                                            }
                                            switch (cmdsplitted[1])
                                            {
                                                case "author":
                                                    completedlist.Sort((p, q) => p.author.CompareTo(q.author));
                                                    break;
                                                case "title":
                                                    completedlist.Sort((p, q) => p.name.CompareTo(q.name));
                                                    break;
                                                case "date":
                                                    completedlist.Sort((p, q) => p.date.CompareTo(q.date));
                                                    break;
                                                default:
                                                    Console.WriteLine("order-by flag is wrong");
                                                    break;
                                            }
                                            break;
                                        default:
                                            Console.WriteLine("One of your flags is wrong");
                                            break;
                                    }
                                }
                                foreach (Book bo in completedlist)
                                    Console.WriteLine(bo.Id + " " + bo.author + " " + bo.name + " " + bo.date);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("One of your flags is wrong");
                            }
                        }
                            break;
                    case "buy":
                        try
                        {
                            Book b = library.Single(s => s.Id == int.Parse(cmd[1]));
                            Console.WriteLine("Item with id=" + b.Id + " titled " + b.name + " by " + b.author + " written " + b.date + " removed from list of displayed");
                            library.Remove(b);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Sorry, sire, we don't have that");
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong command");
                        break;
                }

            }
        }
    }
}
