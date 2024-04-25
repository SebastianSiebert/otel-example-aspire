using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var postDb = postgres.AddDatabase("blog-post-db");
var commentDb = postgres.AddDatabase("blog-comment-db");

var postService = builder.AddProject<OTel_Example_BlogService>("postservice")
    .WithReference(postDb);

var commentService = builder.AddProject<OTel_Example_Blog_CommentService>("commentservice")
    .WithReference(commentDb);

var apiService = builder.AddProject<OTel_Example_ApiService>("apiservice")
    .WithReference(redis);

builder.AddProject<OTel_Example_Web>("webfrontend")
    .WithReference(apiService)
    .WithReference(postService)
    .WithReference(commentService);

builder.Build().Run();