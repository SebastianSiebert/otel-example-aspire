﻿using System.ComponentModel.DataAnnotations;

namespace OTel.Example.BlogService.Models;

public class Post
{
    [Key] public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public DateTimeOffset PostedAt { get; set; }

}