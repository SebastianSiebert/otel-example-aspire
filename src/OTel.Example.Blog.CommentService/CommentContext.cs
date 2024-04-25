using Microsoft.EntityFrameworkCore;
using OTel.Example.Blog.CommentService.Models;

namespace OTel.Example.Blog.CommentService;

public class CommentContext(DbContextOptions<CommentContext> options) : DbContext(options)
{
    public DbSet<Comment> Comments { get; set; }
}