using Conduit.Images.DataAccess.Articles;
using Dapper.FluentMap;
using Dapper.FluentMap.Configuration;

namespace Conduit.Images.DataAccess.Extensions;

public class DataAccessLayerInitializer
{
    public async Task InitializeAsync()
    {
        FluentMapper.Initialize(InitializeFluentMapper);
        await Task.CompletedTask;
    }

    private static void InitializeFluentMapper(FluentMapConfiguration configuration)
    {
        configuration.AddMap(new ArticleDbModel.EntityMap());
    }
}
