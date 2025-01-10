namespace NguyenPhanHuy_2122110062.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiedEmailinOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Order", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Order", "Email", c => c.String(nullable: false));
        }
    }
}
