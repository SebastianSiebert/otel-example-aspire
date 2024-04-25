namespace OTel.Example.Web;

public class BlogPostApiClient(HttpClient httpClient, ILogger<BlogPostApiClient> logger)
{
    public async Task<List<BlogPost>> GetPostsAsync(CancellationToken cancellationToken = default)
    {
        var posts = await httpClient.GetFromJsonAsync<List<BlogPost>>("/post", cancellationToken);

        return posts ?? [];
    }

    public async Task<BlogPost?> GetPostAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var post = await httpClient.GetFromJsonAsync<BlogPost>($"/post/{id}", cancellationToken);

            return post;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving Post");
        }

        return null;
    }

    public async Task CreatePostAsync(BlogPost inputPost, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync("/post", inputPost, cancellationToken);
    }

    public async Task DeletePostAsync(int id, CancellationToken cancellationToken = default)
    {
        await httpClient.DeleteAsync($"/post/{id}", cancellationToken);
    }

    public async Task GetError(CancellationToken cancellationToken = default)
    {
        await httpClient.GetAsync("/post/error", cancellationToken);
    }
}

public record BlogPost(int Id, string Title, string Content, DateTimeOffset PostedAt);