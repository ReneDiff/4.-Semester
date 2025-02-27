using System;
using Models.EFGetStarted;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

// 🔹 Konfigurer User Secrets til at hente connection-string
var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var conn = builder["ConnectionStrings:DefaultConnection"];
if (string.IsNullOrEmpty(conn))
{
    throw new Exception("Connection string is missing. Ensure you have set up user-secrets.");
}

// 🔹 Konfigurer DbContext med korrekt options
var options = new DbContextOptionsBuilder<MyDBContext>()
    .UseSqlServer(conn)
    .Options;

// 🔹 Opret DbContext med options
using var db = new MyDBContext(options);
Console.WriteLine("Database context initialized successfully.");

// 🔹 Tilføj en ny blog (Create)
Console.WriteLine("Inserting a new blog...");
var blogs = new List<Blog> 
{ 
    new Blog {Url = "http://blogs.msdn.com/adonet"},
    new Blog {Url = "https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/"}

};
db.Blogs.AddRange(blogs);
db.SaveChanges();
foreach (var blogg in blogs)
{
    Console.WriteLine($"✅ Blog inserted with ID: {blogg.BlogId}, URL: {blogg.Url}");
}

// 🔹 Hent en blog fra databasen (Read)
Console.WriteLine("Querying for a blog...");
var blog = db.Blogs
    .OrderBy(b => b.BlogId)
    .FirstOrDefault();

if (blog != null)
{
    Console.WriteLine($"Blog found! ID: {blog.BlogId}, URL: {blog.Url}");
}
else
{
    Console.WriteLine("No blog found!");
}

// 🔹 Opdater en blog (Update)
if (blog != null)
{
    Console.WriteLine("Updating the blog and adding a post...");
    blog.Url = "https://devblogs.microsoft.com/dotnet";
    blog.Posts.Add(new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
    db.SaveChanges();
    Console.WriteLine("Blog updated!");
}

// 🔹 Slet en blog (Delete)
var allBlogs = db.Blogs.ToList();

if (allBlogs.Count > 0)
{
    Console.WriteLine($"Deleting {allBlogs.Count} blogs...");
    db.RemoveRange(allBlogs);
    db.SaveChanges();
    Console.WriteLine("Blogs deleted!");
}

Console.WriteLine("Program finished.");
