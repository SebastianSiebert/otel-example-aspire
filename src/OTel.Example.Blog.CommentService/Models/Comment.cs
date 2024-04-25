using System.ComponentModel.DataAnnotations;

namespace OTel.Example.Blog.CommentService.Models;

public class Comment
{
    [Key] public int Id { get; set; }
    public int PostId { get; set; }
    public string Username { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTimeOffset CommentedAt { get; set; }
}