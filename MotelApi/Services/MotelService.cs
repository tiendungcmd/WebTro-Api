using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotelApi.DBContext;
using MotelApi.Models;
using MotelApi.Request;
using MotelApi.Response;
using MotelApi.Services.IServices;

namespace MotelApi.Services
{
    public class MotelService : IMotelService
    {
        private readonly MotelContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MotelService(MotelContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<Motel> Create(Motel model)
        {
            await _context.Motels.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }
        public async Task<MotelDetail> CreateMotelDetails(MotelDetail model)
        {
            await _context.MotelDetails.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Image> CreateImage(Image model)
        {
            await _context.Images.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }
        public async Task<ImageMotel> ImageMotel(ImageMotel model)
        {
            await _context.ImageMotels.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(Guid id)
        {
            var motel = await _context.Motels.FirstOrDefaultAsync(x => x.Id == id);
            _context.Motels.Remove(motel);

            //find image
            var imageMotel = await _context.ImageMotels.FirstOrDefaultAsync(x => x.MotelId == motel.Id);
            if (imageMotel != null)
            {
                _context.ImageMotels.Remove(imageMotel);
                //delete image
                var image = await _context.Images.FirstOrDefaultAsync(x => x.Id == imageMotel.ImageId);
                if (image != null)
                {
                    _context.Images.Remove(image);
                }
                var iamgeExist = System.IO.File.Exists(_webHostEnvironment.WebRootPath + image.ImageUrl);
                if (iamgeExist)
                {
                    File.Delete(_webHostEnvironment.WebRootPath + image.ImageUrl);
                }
            }

            await _context.SaveChangesAsync();

            return motel != null;
        }

        public async Task<List<Motel>> GetAll()
        {
            return await _context.Motels.Where(x => x.Status == Common.Status.Pending).ToListAsync();
        }

        public async Task<Motel> GetById(Guid id)
        {

            throw new NotImplementedException();
        }

        public Task<Motel> Update(Motel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Image>> GetImages(Guid motelId)
        {
            var result = new List<Image>();
            var imageMotels = await _context.ImageMotels.Where(x => x.MotelId == motelId).ToListAsync();
            foreach (var item in imageMotels)
            {
                var image = _context.Images.FirstOrDefault(x => x.Id == item.ImageId);
                result.Add(image);
            }
            return result;
        }

        public async Task<List<MotelResponse>> GetMotels()
        {
            var motels = await _context.Motels.ToListAsync();
            var result = _mapper.Map<List<MotelResponse>>(motels);



            foreach (var item in result)
            {
                var motelDetail = await _context.MotelDetails.FirstOrDefaultAsync(x => x.MotelId == item.Id);
                var imageMotel = _context.ImageMotels.FirstOrDefault(x => x.MotelId == item.Id);
                if (imageMotel != null)
                {
                    item.Images = _context.Images.FirstOrDefault(x => x.Id == imageMotel.ImageId).ImageUrl.Replace("\\", "/");
                }
                if (motelDetail != null)
                {
                    item.City = motelDetail.City;
                    item.Acreage = motelDetail.Acreage;
                    item.Address = motelDetail.Address;
                    item.NumberBathRoom = motelDetail.NumberBathRoom;
                    item.NumberBedRoom = motelDetail.NumberBedRoom;
                    item.Deposit = motelDetail.Deposit;
                }

            }
            return result;
        }

        public async Task<List<Image>> GetImageByUserName(string userName)
        {
            return await _context.Images.Where(x => x.ImageUrl.Contains(userName + "---")).ToListAsync();
        }

        public async Task<bool> Approve(Guid id)
        {
            var motel = await _context.Motels.FirstOrDefaultAsync(x => x.Id == id);
            motel.Status = Common.Status.Success;
            _context.SaveChanges();
            return motel != null;
        }

        public async Task<bool> Reject(MotelReject motelReject)
        {
            var motel = await _context.Motels.FirstOrDefaultAsync(x => x.Id == motelReject.Id);
            motel.Status = Common.Status.Block;
            motel.Reason = motelReject.Reason;
            _context.SaveChanges();
            return motel != null;
        }

        public async Task<MotelResponse> GetMotelById(Guid id)
        {
            var motels = await _context.Motels.FirstOrDefaultAsync(x => x.Id == id);
            var result = _mapper.Map<MotelResponse>(motels);

            var motelDetail = await _context.MotelDetails.FirstOrDefaultAsync(x => x.MotelId == result.Id);
            var imageMotel = _context.ImageMotels.FirstOrDefault(x => x.MotelId == result.Id);
            if (imageMotel != null)
            {
                result.Images = _context.Images.FirstOrDefault(x => x.Id == imageMotel.ImageId).ImageUrl.Replace("\\", "/");
            }
            if (motelDetail != null)
            {
                result.City = motelDetail.City;
                result.Acreage = motelDetail.Acreage;
                result.Address = motelDetail.Address;
                result.NumberBathRoom = motelDetail.NumberBathRoom;
                result.NumberBedRoom = motelDetail.NumberBedRoom;
                result.Deposit = motelDetail.Deposit;
            }
            //get info user
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == result.UserName);
            if (user != null)
            {
                var userDetail = await _context.UserDetails.FirstOrDefaultAsync(x => x.Id == user.UserDetailId);
                if (userDetail != null && result != null)
                {
                    _ = result.UserPhone == userDetail.Phone;
                }

            }
            return result;
        }
    }
}
