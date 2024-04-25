using Microsoft.EntityFrameworkCore;
using OTel.Example.BlogService;
using OTel.Example.BlogService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<BlogContext>("blog-post-db");

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IPostRepository, PostRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/post", async (IPostRepository repo) => await repo.GetPosts())
    .WithName("GetBlogPosts");

app.MapGet("/post/{id}", async (int id, IPostRepository repo) =>
    {
        var post = await repo.GetPost(id);

        return post is null ? Results.NotFound() : Results.Ok(post);
    })
    .WithName("GetBlogPost");

app.MapPost("/post", async (Post inputPost, IPostRepository repo) =>
    {
        var post = await repo.CreatePost(inputPost);

        return Results.Created($"/blogs/{post.Id}", post);
    })
    .WithName("CreateBlogPost");

app.MapPut("/post/{id}", async (int id, Post inputPost, IPostRepository repo) =>
    {
        var post = await repo.UpdatePost(id, inputPost);

        return post is null ? Results.NotFound() : Results.NoContent();
    })
    .WithName("UpdateBlogPost");

app.MapDelete("/post/{id}", async (int id, IPostRepository repo) =>
    {
        var post = await repo.DeletePost(id);

        return post is null ? Results.NotFound() : Results.NoContent();
    })
    .WithName("DeleteBlogPost");

app.MapGet("/post/error", () =>
{
    throw new NotSupportedException();
});

app.MapDefaultEndpoints();

app.Run();