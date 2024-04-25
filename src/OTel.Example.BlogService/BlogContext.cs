using Microsoft.EntityFrameworkCore;
using OTel.Example.BlogService.Models;

namespace OTel.Example.BlogService;

public class BlogContext(DbContextOptions<BlogContext> options) : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }
}