using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MyUmbraco.Models
{
  [TableName("BlogPostComments")]
  [PrimaryKey("Id", AutoIncrement = true)]
  [ExplicitColumns]
  public class BlogPostComment
  {
    [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("BlogPostId")]
    public int BlogPostId { get; set; }

    [Column("MemberId")]
    public int MemberId { get; set; }

    [Column("Comment")]
    public string Comment { get; set; }
  }
}