using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalMPA.Models;


namespace FinalMPA.Data
{
    public class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();
            if (context.Magazines.Any())
            {
                return; 
            }
            var magazines = new Magazine[]
            {
 new Magazine{Title="Click",Author="Maria Ion",Price=Decimal.Parse("22")},
new Magazine{Title="Libertatea",Author="Natalia Apostol",Price=Decimal.Parse("18")},
new Magazine{Title="Cancan",Author="Ghita Marcu",Price=Decimal.Parse("16")},
new Magazine{Title="Gazeta Sporturilor",Author="Bogdan Mosoiu",Price=Decimal.Parse("11")},
new Magazine{Title="Adevarul",Author="Manuel Popescu",Price=Decimal.Parse("17")},
new Magazine{Title="Ziarul Financiar",Author="Ana Morar",Price=Decimal.Parse("15")}
            };
            foreach (Magazine b in magazines)
            {
                context.Magazines.Add(b);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

 new Customer{CustomerID=101,Name="Pop Marcel",BirthDate=DateTime.Parse("1994-03-07")},
 new Customer{CustomerID=107,Name="Bogdan Mihai",BirthDate=DateTime.Parse("1976-08-10")},

            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
 new Order{MagazineID=1,CustomerID=101,OrderDate=DateTime.Parse("02-11-2021")},
 new Order{MagazineID=3,CustomerID=107,OrderDate=DateTime.Parse("01-11-2021")},
 new Order{MagazineID=1,CustomerID=107,OrderDate=DateTime.Parse("02-11-2021")},
 new Order{MagazineID=2,CustomerID=101,OrderDate=DateTime.Parse("01-11-2021")},
 new Order{MagazineID=4,CustomerID=101,OrderDate=DateTime.Parse("01-11-2021")},
 new Order{MagazineID=6,CustomerID=101,OrderDate=DateTime.Parse("02-11-2021")},
 };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
            var publishers = new Publisher[]
            {

 new Publisher{PublisherName="Sociedad",Adress="Str. Aurel Vlaicu, nr. 18, Bucuresti"},
 new Publisher{PublisherName="Actualitate",Adress="Str. Dorobantilor, nr. 92, Cluj-Napoca"},
 new Publisher{PublisherName="Paralela 45",Adress="Str. Lunii, nr. 132, Cluj-Napoca"},
            };
            foreach (Publisher p in publishers)
            {
                context.Publishers.Add(p);
            }
            context.SaveChanges();
            var publishedmagazines = new PublishedMagazine[]
            {
 new PublishedMagazine {
 MagazineID = magazines.Single(c => c.Title == "Casa si gradina" ).ID,
 PublisherID = publishers.Single(i => i.PublisherName == "Sociedad").ID
 },
 new PublishedMagazine {
 MagazineID = magazines.Single(c => c.Title == "Secretele bucatariei" ).ID,
PublisherID = publishers.Single(i => i.PublisherName == "Actualitate").ID
 },
 new PublishedMagazine {
 MagazineID = magazines.Single(c => c.Title == "Gurmand" ).ID,
 PublisherID = publishers.Single(i => i.PublisherName == "Paralela 45").ID
 },
 new PublishedMagazine {
 MagazineID = magazines.Single(c => c.Title == "Lidl Revista Noua" ).ID,
PublisherID = publishers.Single(i => i.PublisherName == "Lidl").ID
 },
 new PublishedMagazine {
 MagazineID = magazines.Single(c => c.Title == "Kaufland Revista Noua" ).ID,
PublisherID = publishers.Single(i => i.PublisherName == "Kaufland").ID
 },
 new PublishedMagazine {
 MagazineID = magazines.Single(c => c.Title == "Auchan Revista Noua" ).ID,
 PublisherID = publishers.Single(i => i.PublisherName == "Auchan").ID
 },
            };
            foreach (PublishedMagazine pb in publishedmagazines)
            {
                context.PublishedMagazines.Add(pb);
            }
            context.SaveChanges();
        }
    }
}
