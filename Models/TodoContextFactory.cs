using TodoApi.Models;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
{
    public TodoContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
 
        var builder = new DbContextOptionsBuilder<TodoContext>();
 
        var connectionString = configuration.GetConnectionString("DefaultConnection");
 
        builder.UseSqlServer(connectionString);
 
        return new TodoContext(builder.Options);
    }
}