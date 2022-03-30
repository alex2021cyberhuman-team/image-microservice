using Conduit.Images.Domain.Images.Common;

namespace Conduit.Images.Domain.Images.UploadArticleImage;

public class UploadArticleImageResponse : BaseResponse
{
    public UploadArticleImageResponse(Error error)
    {        
    }

    public UploadArticleImageResponse(Model data)
    {
        Data = data;
    }

    public Model? Data { get; }

    public class Model
    {

        public Model(ArticleImageResponseModel image)
        {
            Image = image;
        }

        public ArticleImageResponseModel Image { get; }
    }
}
