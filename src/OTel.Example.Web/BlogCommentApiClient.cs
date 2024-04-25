namespace OTel.Example.Web;

public class BlogCommentApiClient(HttpClient httpClient)
{
    public async Task<List<BlogComment>> GetCommentsAsync(int postId, CancellationToken cancellationToken = default)
    {
        var comments = await httpClient.GetFromJsonAsync<List<BlogComment>>($"/comment/post/{postId}", cancellationToken);

        return comments ?? [];
    }

    public async Task CreateCommentAsync(BlogComment inputPost, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync("/comment", inputPost, cancellationToken);
    }

    public async Task DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
    {
        await httpClient.DeleteAsync($"/comment/{id}", cancellationToken);
    }
}

public record BlogComment(int Id, int PostId, string Username, string Content, DateTimeOffset CommentedAt);