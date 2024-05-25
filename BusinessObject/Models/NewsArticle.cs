using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class NewsArticle
{
    public string NewsArticleId { get; set; } = null!;

    public string? NewsTitle { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public short? CreatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SystemAccount? CreatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public string TagsAsString => string.Join(", ", Tags.Select(tag => tag.TagName));
    public string CategoryName => Category?.CategoryName ?? "Unknown";
}
