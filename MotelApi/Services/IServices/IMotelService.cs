using MotelApi.Models;
using MotelApi.Request;
using MotelApi.Response;

namespace MotelApi.Services.IServices
{
    public interface IMotelService : IServiceCommon<Motel>
    {
        Task<Image> CreateImage(Image model);

        Task<ImageMotel> ImageMotel(ImageMotel model);
        Task<List<Image>> GetImages(Guid id);
        Task<List<MotelResponse>> GetMotels(string? userName);
        Task<List<Image>> GetImageByUserName(string userName);
        Task<bool> Approve(Guid id);
        Task<bool> Hired(Guid id);
        Task<bool> Reject(MotelReject motelReject);
        Task<MotelDetail> CreateMotelDetails(MotelDetail model);
        Task<MotelResponse> GetMotelById(Guid id);

        Task<Motel> UpdateMotel(MotelModelRequest motel);
    }
}
