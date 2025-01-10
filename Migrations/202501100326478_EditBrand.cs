namespace NguyenPhanHuy_2122110062.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditBrand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Brand", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_Brand", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Brand", "ModifierDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Brand", "ModifierBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Brand", "ModifierBy");
            DropColumn("dbo.tb_Brand", "ModifierDate");
            DropColumn("dbo.tb_Brand", "CreatedDate");
            DropColumn("dbo.tb_Brand", "CreatedBy");
        }
    }
}
