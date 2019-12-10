using System;
using Umbraco.Core.Logging;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MyUmbraco.Components
{
  public class BlogPostCommentsComposer : ComponentComposer<BlogPostCommentsComponent>
  {
  }

  public class BlogPostCommentsComponent : IComponent
  {
    private IScopeProvider _scopeProvider;
    private IMigrationBuilder _migrationBuilder;
    private IKeyValueService _keyValueService;
    private ILogger _logger;

    public BlogPostCommentsComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
    {
      _scopeProvider = scopeProvider;
      _migrationBuilder = migrationBuilder;
      _keyValueService = keyValueService;
      _logger = logger;
    }

    public void Initialize()
    {
      var migrationPlan = new MigrationPlan("BlogPostComments");
      migrationPlan.From(string.Empty)
        .To<BlogPostCommentsTable>("Create-BlogPostCommentsTable")
        .To<BlogPostCommentInfoView>("Create-BlogPostCommentInfoView")
        .To<AddBlogCommentDateColumn>("Add-BlogPostCommentedOnColumn");
      var upgrader = new Upgrader(migrationPlan);

      upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
    }

    public void Terminate()
    {
    }

    private class BlogPostCommentsTable : MigrationBase
    {
      public BlogPostCommentsTable(IMigrationContext context) : base(context)
      {
      }

      public override void Migrate()
      {
        Logger.Debug<BlogPostCommentsTableSchema>("Running migration {MigrationStep}", "BlogPostComments");

        if (TableExists("BlogPostComments") == false)
        {
          Create.Table<BlogPostCommentsTableSchema>().Do();
        }
        else
        {
          Logger.Debug<BlogPostCommentsTableSchema>("The database table {DbTable} already exists, skipping", "BlogPostComments");
        }
      }

      [TableName("BlogPostComments")]
      [PrimaryKey("Id", AutoIncrement = true)]
      [ExplicitColumns]
      private class BlogPostCommentsTableSchema
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

    private class BlogPostCommentInfoView : MigrationBase
    {
      public BlogPostCommentInfoView(IMigrationContext context) : base(context)
      {
      }

      public override void Migrate()
      {
        Logger.Debug<BlogPostCommentInfoView>("Running migration {MigrationStep}", "BlogPostCommentInfo");

        if (TableExists("BlogPostCommentInfo") == false)
        {
          const string sql = @"IF object_id(N'dbo.BlogPostCommentInfo', 'V') IS NOT NULL
	                              DROP VIEW dbo.BlogPostCommentInfo
                              GO

                              CREATE VIEW dbo.BlogPostCommentInfo AS
                              SELECT
                              MAX(bpc.BlogPostId) AS BlogPostId,
                              MAX(bpc.MemberId) AS MemberId,
                              MAX(bpc.Comment) AS Comment,
                              MAX(CASE WHEN d.propertyTypeId = 50 THEN d.varcharValue else '' END) AS FirstName,
                              MAX(CASE WHEN d.propertyTypeId = 51 THEN d.varcharValue else '' END) AS LastName,
                              MAX(CASE WHEN d.propertyTypeId = 52 THEN d.varcharValue else '' END) AS Avatar
                              FROM
                              dbo.cmsMemberType AS mt 
                              INNER JOIN dbo.cmsPropertyType AS p ON mt.propertytypeId = p.id
                              INNER JOIN dbo.umbracoPropertyData AS d ON p.id = d.propertytypeid
                              INNER JOIN dbo.umbracoContentVersion AS ucv ON ucv.id = d.versionId
                              INNER JOIN dbo.BlogPostComments AS bpc ON bpc.MemberId = ucv.nodeId
                              GROUP BY bpc.Id";
          Execute.Sql(sql).Do();
        }
        else
        {
          Logger.Debug<BlogPostCommentInfoView>("The database table {DbTable} already exists, skipping", "BlogPostCommentInfo");
        }
      }
    }

    private class AddBlogCommentDateColumn : MigrationBase
    {
      public AddBlogCommentDateColumn(IMigrationContext context) : base(context)
      {
      }

      public override void Migrate()
      {
        Logger.Debug<BlogPostCommentsTable>("Running migration {MigrationStep}", "AddCommentedOnColumn");

        if (!ColumnExists("BlogPostComments", "CommentedOn"))
          Create.Column("CommentedOn").OnTable("BlogPostComments").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now.ToLongDateString()).Do();

        const string sql = @"IF object_id(N'dbo.BlogPostCommentInfo', 'V') IS NOT NULL
	                              DROP VIEW dbo.BlogPostCommentInfo
                              GO

                              CREATE VIEW dbo.BlogPostCommentInfo AS
                              SELECT
                              MAX(bpc.BlogPostId) AS BlogPostId,
                              MAX(bpc.MemberId) AS MemberId,
                              MAX(bpc.Comment) AS Comment,
							                MAX(bpc.CommentedOn) AS CommentedOn,
                              MAX(CASE WHEN d.propertyTypeId = 50 THEN d.varcharValue else '' END) AS FirstName,
                              MAX(CASE WHEN d.propertyTypeId = 51 THEN d.varcharValue else '' END) AS LastName,
                              MAX(CASE WHEN d.propertyTypeId = 52 THEN d.varcharValue else '' END) AS Avatar
                              FROM
                              dbo.cmsMemberType AS mt 
                              INNER JOIN dbo.cmsPropertyType AS p ON mt.propertytypeId = p.id
                              INNER JOIN dbo.umbracoPropertyData AS d ON p.id = d.propertytypeid
                              INNER JOIN dbo.umbracoContentVersion AS ucv ON ucv.id = d.versionId
                              INNER JOIN dbo.BlogPostComments AS bpc ON bpc.MemberId = ucv.nodeId
                              GROUP BY bpc.Id";
        Execute.Sql(sql).Do();
      }
    }

  }
}