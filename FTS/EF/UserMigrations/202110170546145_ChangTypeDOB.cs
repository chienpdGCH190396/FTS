namespace FTS.EF.UserMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangTypeDOB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DOB", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DOB", c => c.DateTime());
        }
    }
}
