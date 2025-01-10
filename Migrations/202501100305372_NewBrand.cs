namespace NguyenPhanHuy_2122110062.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewBrand : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Brand",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 250),
                        Logo = c.String(),
                        Description = c.String(),
                        Alias = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SeoTitle = c.String(maxLength: 250),
                        SeoDescription = c.String(maxLength: 500),
                        SeoKeywords = c.String(maxLength: 250),
                        Product_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            AddColumn("dbo.tb_Product", "BrandId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Brand", "Product_Id", "dbo.tb_Product");
            DropIndex("dbo.tb_Brand", new[] { "Product_Id" });
            DropColumn("dbo.tb_Product", "BrandId");
            DropTable("dbo.tb_Brand");
        }
    }
}
