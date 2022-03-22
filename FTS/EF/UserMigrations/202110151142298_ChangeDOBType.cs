namespace FTS.EF.UserMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDOBType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DOB", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DOB", c => c.String());
        }
    }
}
