using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MyUmbraco.ViewModels
{
  [TableName("BlogPostCommentInfo")]
  [ExplicitColumns]
  public class BlogPostCommentViewModel
  {
    [Column("MemberId")]
    public int MemberId { get; set; }

    [Column("BlogPostId")]
    public int BlogPostId { get; set; }

    [Column("Comment")]
    public string Comment { get; set; }

    [Column("FirstName")]
    public string FirstName { get; set; }

    [Column("LastName")]
    public string LastName { get; set; }

    [Column("Avatar")]
    public string Avatar { get; set; }

    public string FullName => $"{FirstName} {LastName}";
  }
}