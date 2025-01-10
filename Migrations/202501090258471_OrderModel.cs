namespace NguyenPhanHuy_2122110062.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.tb_Order", "Ward", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Order", "Ward", c => c.String(nullable: false));
            DropColumn("dbo.tb_Order", "Address");
        }
    }
}
