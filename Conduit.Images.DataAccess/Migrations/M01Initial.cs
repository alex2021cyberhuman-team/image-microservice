using FluentMigrator;

namespace Conduit.Images.DataAccess.Migrations;

[Migration(1)]
public class Initial : Migration
{
    public override void Up()
    {
        Create.Table("article")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("slug").AsString(1000)
            .WithColumn("author_id").AsGuid();
            
        Create.Table("article_image")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("user_id").AsGuid()
            .WithColumn("storage_name").AsString(1000)
            .WithColumn("media_type").AsString(50)
            .WithColumn("uploaded").AsDateTime2().Indexed()
            .WithColumn("article_id").AsGuid().Nullable()
                .ForeignKey("article", "id");
    }

    public override void Down()
    {
        Delete.Table("article");
        Delete.Table("article_image");
    }
}
