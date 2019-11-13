using NPoco;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace MyUmbraco.Components
{
  // First we will create a Composer deriving from the Umbraco ComponentComposer class
  // We will type our composer to that of our new component which we will create next
  public class BlogPostHighFiveComposer : ComponentComposer<BlogPostHighFiveComponent>
  {
  }

  // Now that our ComponentComposer is created it's time to create our Component deriving from the IComponent Interface
  public class BlogPostHighFiveComponent : IComponent
  {
    // These private variables will be used for dependency injection
    private readonly IScopeProvider _scopeProvider;
    private readonly IMigrationBuilder _migrationBuilder;
    private readonly IKeyValueService _keyValueService;
    private readonly ILogger _logger;

    // We will need to interface with a few Umbraco services, we will use dependency injection to achieve this  
    public BlogPostHighFiveComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
    {
      _scopeProvider = scopeProvider;
      _migrationBuilder = migrationBuilder;
      _keyValueService = keyValueService;
      _logger = logger;
    }

    // The Initialize function will run each time the application is started up
    // It is here that we will add our logic to add our high five custom table
    public void Initialize()
    {
      // We start by creating a Migration plan and giving it a unique name based on the feature/functionality that we want to add.
      var migrationPlan = new MigrationPlan("BlogPostHighFives");

      // We now tell the migration plan where to start from, in this case an empty string as we have no previous versions
      // We also start our upgrade chain using the To call passing in a class of BlogPostHighFivesTable which we will create further down
      // We will be passing in a string to describe the migration step we are taking, in this case we are Creating the Blog Post High Fives Table
      migrationPlan.From(string.Empty)
        .To<BlogPostHighFivesTable>("Create-BlogPostHighFivesTable");

      // Now we will create an Upgrader and pass the migration plan into it
      var upgrader = new Upgrader(migrationPlan);

      // In order for an Upgrader to be executed we have to pass in the 4 services which we have already injected into the Component
      upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
    }

    // The Terminate function will be run when the application is shut down
    // For our example we do not need any custom code to run so we will leave this blank for now
    public void Terminate()
    {
    }

    // We will need a class that extends from the MigrationBase class and that will contain all of our custom logic to create the new custom table
    // NOTE: After the Migration has run DO NOT CHANGE THE CODE!!! Any changes you need to make should be done with a new Migration
    // This is to ensure that when this code is run on your development, staging and production servers they will all be in sync!
    public class BlogPostHighFivesTable : MigrationBase
    {
      //Using dependency injection, we will pass the current migration context to the MigrationBase base class
      public BlogPostHighFivesTable(IMigrationContext context) : base(context)
      {
      }

      // The Migrate function is where we will add our own logic to create our Custom Table
      public override void Migrate()
      {
        // We will want to add logging through out our migration
        // Note: Do not use string interpolation! The Debug call takes 2 strings
        Logger.Debug<BlogPostHighFivesTable>("Running migration {MigrationStep}", "BlogPostHighFivesTable");

        // The MigrationBase class  has a lot of handy functions that you should check out
        // For this migration we will want to check that the table we are about to create doesn't already exist
        if (TableExists("BlogPostHighFives") == false)
        {
          // If it does not exist then we should create it using a schema we have declared below
          Create.Table<BlogPostHighFivesTableSchema>().Do();
        }
        else
        {
          // However if it already exists we should log that we did not run the migration and give a reason why
          Logger.Debug<BlogPostHighFivesTable>("The database table {DbTable} already exists, skipping", "BlogPostHighFives");
        }
      }
      
      // The Custom table schema is declared here using NPoco attributes.
      // A little database/SQL knowledge is required here but it should be pretty straight forward to understand these attributes
      [TableName("BlogPostHighFives")]
      [PrimaryKey("Id", AutoIncrement = true)]
      [ExplicitColumns]
      public class BlogPostHighFivesTableSchema
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

  }
}