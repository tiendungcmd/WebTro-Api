using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotelApi.DBContext;
using MotelApi.Models;
using MotelApi.Request;
using MotelApi.Response;
using MotelApi.Services.IServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<Models.Image> CreateImage(Models.Image model)
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
            return await _context.Motels.Where(x => x.Status == Common.Status.Waiting).ToListAsync();
        }

        public async Task<Motel> GetById(Guid id)
        {

            return await _context.Motels.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Motel> Update(Motel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Image>> GetImages(Guid motelId)
        {
            var result = new List<Models.Image>();
            var imageMotels = await _context.ImageMotels.Where(x => x.MotelId == motelId).ToListAsync();
            foreach (var item in imageMotels)
            {
                var image = _context.Images.FirstOrDefault(x => x.Id == item.ImageId);
                result.Add(image);
            }
            return result;
        }

        public async Task<List<MotelResponse>> GetMotels(string? userName)
        {
            var motels = new List<Motel>();
            if (userName != null)
            {
                motels = await _context.Motels.Where(x => x.UserName == userName).ToListAsync();
            }
            else
            {
                motels = await _context.Motels.ToListAsync();
            }

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

        public async Task<List<Models.Image>> GetImageByUserName(string userName)
        {
            return await _context.Images.Where(x => x.ImageUrl.Contains(userName + "---")).ToListAsync();
        }

        public async Task<bool> Approve(Guid id)
        {
            var motel = await _context.Motels.FirstOrDefaultAsync(x => x.Id == id);
            motel.Status = Common.Status.InProgress;
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

        public async Task<Motel> UpdateMotel(MotelModelRequest motelModelRequest)
        {
            var motel = await _context.Motels.FirstOrDefaultAsync(x => x.Id == motelModelRequest.Id);
            motel.UserName = motelModelRequest.UserName;
            motel.Title = motelModelRequest.Title;
            motel.Descriptions = motelModelRequest.Descriptions;
            motel.Price = motelModelRequest.Price;
            motel.Status = motelModelRequest.Status;
            _context.Motels.Update(motel);

            var motelDetail = await _context.MotelDetails.FirstOrDefaultAsync(x => x.MotelId == motel.Id);
            if (motelDetail != null)
            {
                _ = motelDetail.Address == motelModelRequest.Address;
                motelDetail.Acreage = motelModelRequest.Acreage;
                motelDetail.City = motelModelRequest.City;
                motelDetail.NumberBedRoom = motelModelRequest.NumberBedRoom;
                motelDetail.NumberBathRoom = motelModelRequest.NumberBathRoom;
                motelDetail.Deposit = motelModelRequest.Deposit;
            }
            _context.MotelDetails.Update(motelDetail);

            //check image 
            if (motelModelRequest.File != null)
            {
                var imageMotel = await _context.ImageMotels.FirstOrDefaultAsync(x => x.MotelId == motel.Id);
                var image = await _context.Images.FirstOrDefaultAsync(x => x.Id == imageMotel.ImageId);
                StringBuilder fileName = new();

                var imageExist = await GetImageByUserName(motelModelRequest.UserName);
                if (imageExist.Count() == 0)
                {
                    fileName.Append(motelModelRequest.UserName);
                    fileName.Append("---0---.png");
                }
                else
                {
                    var number = imageExist.Count();
                    fileName.Append(motelModelRequest.UserName);
                    fileName.Append("---");
                    fileName.Append(number);
                    fileName.Append("---.png");
                }
                try
                {
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + ".\\Images\\"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + ".\\Images\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + ".\\Images\\" + fileName))
                    {
                        motelModelRequest.File.CopyTo(fileStream);
                        fileStream.Flush();
                        image.ImageUrl = "\\Images\\" + fileName;
                    }


                }
                catch (Exception ex)
                {

                }
                _context.Images.Update(image);
            }

            _context.SaveChanges();
            return motel;
        }

        public async Task<bool> Hired(Guid id)
        {
            var motel = await _context.Motels.FirstOrDefaultAsync(x => x.Id == id);
            motel.Status = Common.Status.Success;
            _context.SaveChanges();
            return motel != null;
        }

        public async Task<Comment> SendComment(CommentRequest commentRequest)
        {
            var comment = new Comment();
            comment.Id = Guid.NewGuid();
            comment.UserName = commentRequest.UserName;
            comment.Descriptions = commentRequest.Descriptions;
            comment.CreatedTime = DateTime.Now;
            comment.UserId = Guid.NewGuid();
            comment.MotelId = commentRequest.MotelId;
            await _context.Comments.AddAsync(comment);
            _context.SaveChanges();
            return comment;
        }

        public async Task<List<Comment>> GetCommentByMotelId(Guid id)
        {
            return await _context.Comments.Where(x => x.MotelId == id).ToListAsync();
        }

        public async Task<bool> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return comment != null;
        }
    }
}
