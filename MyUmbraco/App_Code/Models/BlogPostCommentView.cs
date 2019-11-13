using System.Web.UI.WebControls;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MyUmbraco.Models
{
  [TableName("BlogPostCommentInfo")]
  [ExplicitColumns]
  public class BlogPostCommentView
  {
    [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
    [Column("MemberId")]
    public int MemberId { get; set; }

    [Column("BlogPostId")]
    public int BlogPostId { get; set; }

    [Column("Comment")]
    public string Comment { get; set; }

    [Column("MemberData")]
    public string MemberData { get; set; }
  }
}