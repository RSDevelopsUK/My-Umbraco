using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MyUmbraco.Models
{
    [TableName("BlogPostHighFives")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class BlogPostHighFive
    {
      [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
      [Column("Id")]
      public int Id { get; set; }

      [Column("BlogPostId")]
      public int BlogPostId { get; set; }

      [Column("MemberId")]
      public int MemberId { get; set; }
    }
}