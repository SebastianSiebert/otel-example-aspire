using Microsoft.EntityFrameworkCore;
using OTel.Example.Blog.CommentService.Models;

namespace OTel.Example.Blog.CommentService;

public interface ICommentRepository
{
    Task<List<Comment>> GetCommentsForPost(int postId);
    Task CreateComment(Comment comment);
    Task<Comment?> DeleteComment(int id);
}

public class CommentRepository(CommentContext context) : ICommentRepository
{
    private bool _didMigration = false;

    public async Task<List<Comment>> GetCommentsForPost(int postId)
    {
        await Migrate();

        return await context.Set<Comment>().Where(e => e.PostId == postId).ToListAsync();
    }

    public async Task CreateComment(Comment comment)
    {
        await Migrate();

        comment.CommentedAt = comment.CommentedAt.UtcDateTime;
        context.Set<Comment>().Add(comment);
        await context.SaveChangesAsync();
    }

    public async Task<Comment?> DeleteComment(int id)
    {
        await Migrate();

        var comment = await context.Set<Comment>().FindAsync(id);
        if (comment is null) return null;

        context.Set<Comment>().Remove(comment);
        await context.SaveChangesAsync();

        return comment;
    }

    private async Task Migrate()
    {
        if (_didMigration) return;

        await context.Database.MigrateAsync();
        _didMigration = true;
    }
}