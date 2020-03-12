namespace KafeKod.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrunStokDurum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Urunler", "BittiMi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Urunler", "BittiMi");
        }
    }
}
