using Microsoft.EntityFrameworkCore;
using OTel.Example.BlogService.Models;

namespace OTel.Example.BlogService;

public class PostRepository(BlogContext context) : IPostRepository
{
    private bool _didMigration = false;

    public async Task<List<Post>> GetPosts()
    {
        await Migrate();

        return await context.Set<Post>().ToListAsync();
    }

    public async Task<Post?> GetPost(int id)
    {
        await Migrate();

        return await context.Set<Post>().FindAsync(id);
    }

    public async Task<Post> CreatePost(Post post)
    {
        await Migrate();

        post.PostedAt = post.PostedAt.UtcDateTime;
        context.Set<Post>().Add(post);
        await context.SaveChangesAsync();

        return post;
    }

    public async Task<Post?> UpdatePost(int id, Post inputPost)
    {
        await Migrate();

        var post = await context.Set<Post>().FindAsync(id);
        if (post is null) return null;

        post.Title = inputPost.Title;
        post.Content = inputPost.Content;

        await context.SaveChangesAsync();

        return post;
    }

    public async Task<Post?> DeletePost(int id)
    {
        await Migrate();

        var post = await context.Set<Post>().FindAsync(id);
        if (post is null) return null;

        context.Set<Post>().Remove(post);
        await context.SaveChangesAsync();

        return post;
    }

    private async Task Migrate()
    {
        if (_didMigration) return;

        await context.Database.MigrateAsync();
        _didMigration = true;
    }
}

public interface IPostRepository
{

    Task<List<Post>> GetPosts();
    Task<Post?> GetPost(int id);
    Task<Post> CreatePost(Post post);
    Task<Post?> UpdatePost(int id, Post inputPost);
    Task<Post?> DeletePost(int id);
}