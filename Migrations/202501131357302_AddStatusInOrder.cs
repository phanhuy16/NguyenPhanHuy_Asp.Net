namespace NguyenPhanHuy_2122110062.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusInOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "Status");
        }
    }
}
