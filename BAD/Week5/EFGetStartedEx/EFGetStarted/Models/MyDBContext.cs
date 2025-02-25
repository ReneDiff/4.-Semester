using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Models.EFGetStarted;
public class MyDBContext : DbContext
{
    public DbSet<Blog> Blogs {get; set;}
    public DbSet<Post> Posts {get; set;}

    public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) {}

    public MyDBContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            // Hent connection string fra User Secrets
            var config = new ConfigurationBuilder()
                .AddUserSecrets<MyDBContext>()
                .Build();

            var conn = config["ConnectionStrings:DefaultConnection"];

            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("Database connection string is missing. Ensure you have set up user-secrets.");
            }

            optionsBuilder.UseSqlServer(conn);
        }
    }
}