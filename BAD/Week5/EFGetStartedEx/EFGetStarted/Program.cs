using System;
using Models.EFGetStarted;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.EntityFrameworkCore;


var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var conn = builder["ConnectionStrings:DefaultConnection"];

if (string.IsNullOrEmpty(conn))
{
    throw new Exception("Connection string is missing. Ensure you have set up user-secrets.");
}

// Konfigurer DbContext med connection string
var options = new DbContextOptionsBuilder<MyDBContext>()
    .UseSqlServer(conn)
    .Options;

using var dbContext = new MyDBContext(options);
Console.WriteLine("Database context initialized successfully.");

public class Program
{
    private static void Main()
{
    using (var db = new MyDBContext())
    {
        // Note: This sample requires the database to be created before running.

        // Create
        Console.WriteLine("Inserting a new blog");
        db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
        db.SaveChanges();

        // Read
        Console.WriteLine("Querying for a blog");
        var blog = db.Blogs
            .OrderBy(b => b.BlogId)
            .First();
        Console.WriteLine($"BlogId: {blog.BlogId} \turl:{blog.Url}");

        // Update
        Console.WriteLine("Updating the blog and adding a post");
        blog.Url = "https://devblogs.microsoft.com/dotnet";
        blog.Posts.Add(
            new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
        db.SaveChanges();

        //Delete
        Console.WriteLine("Delete the blog");
        db.Remove(blog);
        db.SaveChanges();
    }
    }
};