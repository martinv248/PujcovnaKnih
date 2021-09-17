namespace PujcovnaKnih.Migrations
{
    using PujcovnaKnih.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PujcovnaKnih.Models.DbEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(PujcovnaKnih.Models.DbEntities context)
        {
            context.Users.AddOrUpdate(i => i.Email,
                new Users
                {
                    FName = "Jon",
                    LName = "Doe",
                    Email = "doe@gmail.com",
                    Password = "12345"
                }
                );
            
            context.Books.AddOrUpdate(i => i.Title,
                new Book
                {
                    Title = "Harry Potter and the Prisoner of Azkaban",
                    Genre = "Adventure",
                    Author = "J. K. Rowling",
                    Price = 20M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "The Godfather",
                    Genre = "Krimi",
                    Author = "Mario Puzo",
                    Price = 10M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "Game of Thrones",
                    Genre = "Fantasy",
                    Author = "George R. R. Martin",
                    Price = 30M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "The Two Towers",
                    Genre = "Fantasy",
                    Author = "J. R. R. Tolkien",
                    Price = 15M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "A Clash of Kings",
                    Genre = "Fantasy",
                    Author = "George R. R. Martin",
                    Price = 30M,
                    IsAvailable = "Zapůjčená"
                },
                new Book
                {
                    Title = "Rychlé šípy",
                    Genre = "Adventure",
                    Author = "Jaroslav Foglar",
                    Price = 5M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "Pod kupolí",
                    Genre = "Horor",
                    Author = "Stephen King",
                    Price = 5M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "Osvícení",
                    Genre = "Horor",
                    Author = "Stephen King",
                    Price = 10M,
                    IsAvailable = "Zapůjčená"
                },
                new Book
                {
                    Title = "Policie",
                    Genre = "Krimi",
                    Author = " Jo Nesbo",
                    Price = 10M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "Ztracený symbol",
                    Genre = "Krimi",
                    Author = "Dan Brown",
                    Price = 15M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "Meč osudu",
                    Genre = "Fantasy",
                    Author = "Andrzej Sapkowski",
                    Price = 15M,
                    IsAvailable = "Volná"
                },
                new Book
                {
                    Title = "Zelená míle",
                    Genre = "Horor",
                    Author = "Stephen King",
                    Price = 20M,
                    IsAvailable = "Zapůjčená"
                }
            );
        }
    }
}
