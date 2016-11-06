namespace Vocabulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeModelPopularWord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PopularWords",
                c => new
                    {
                        WordId = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WordId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PopularWords");
        }
    }
}
