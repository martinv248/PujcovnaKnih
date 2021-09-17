namespace PujcovnaKnih.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "FName", c => c.String());
            AlterColumn("dbo.Users", "LName", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "LName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "FName", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
