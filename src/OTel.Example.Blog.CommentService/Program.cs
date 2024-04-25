using OTel.Example.Blog.CommentService;
using OTel.Example.Blog.CommentService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<CommentContext>("blog-comment-db");

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.MapGet("/comment/post/{id}", async (int id, ICommentRepository repo) => await repo.GetCommentsForPost(id))
    .WithName("GetCommentsForPost");

app.MapPost("/comment", async (Comment inputComment, ICommentRepository repo) =>
    {
        await repo.CreateComment(inputComment);

        return Results.NoContent();
    })
    .WithName("CreateComment");

app.MapDelete("/comment/{id}", async (int id, ICommentRepository repo) =>
    {
        var comment = await repo.DeleteComment(id);

        return comment is null ? Results.NotFound() : Results.NoContent();
    })
    .WithName("DeleteComment");

app.Run();