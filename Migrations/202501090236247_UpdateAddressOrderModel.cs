namespace NguyenPhanHuy_2122110062.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAddressOrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Ward", c => c.String(nullable: false));
            AddColumn("dbo.tb_Order", "District", c => c.String());
            AddColumn("dbo.tb_Order", "City", c => c.String());
            DropColumn("dbo.tb_Order", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "Address", c => c.String(nullable: false));
            DropColumn("dbo.tb_Order", "City");
            DropColumn("dbo.tb_Order", "District");
            DropColumn("dbo.tb_Order", "Ward");
        }
    }
}
