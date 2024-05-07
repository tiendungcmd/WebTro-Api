using Microsoft.AspNetCore.Mvc;
using MotelApi.Models;
using MotelApi.Request;
using MotelApi.Response;
using MotelApi.Services.IServices;
using System.Text;

namespace MotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotelController : ControllerBase
    {
        private readonly IMotelService _service;
        private static IWebHostEnvironment _webHostEnvironment;
        public MotelController(IMotelService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<MotelResponse>>> CreateMotel([FromForm] MotelModelRequest request)
        {
            var motel = new Motel();
            motel.Id = Guid.NewGuid();
            motel.UserName = request.UserName;
            motel.Descriptions = request.Descriptions;
            motel.Price = request.Price;
            motel.Status = request.Status;
            motel.Title = request.Title;
            motel.Rate = request.Rate;
            motel.Reason = "";
            motel.CreatedTime = DateTime.Now;
            var result = await _service.Create(motel);

            var motelDetail = new MotelDetail();
            motelDetail.Id = Guid.NewGuid();
            motelDetail.MotelId = motel.Id;
            motelDetail.Address = request.Address;
            motelDetail.City = request.City;
            motelDetail.NumberBathRoom = (int)request.NumberBathRoom;
            motelDetail.NumberBedRoom = (int)request.NumberBedRoom;
            motelDetail.Acreage = (int)request.Acreage;
            motelDetail.Deposit = (int)request.Deposit;
            //motel detail
            await _service.CreateMotelDetails(motelDetail);


            //save iamge
            var image = new Image();
            image.Id = Guid.NewGuid();
            image.Name = request.UserName;

            StringBuilder fileName = new();
            //check image 
            var imageExist = await _service.GetImageByUserName(request.UserName);
            if (imageExist.Count() == 0)
            {
                fileName.Append(request.UserName);
                fileName.Append("---0---.png");
            }
            else
            {
                var number = imageExist.Count();
                fileName.Append(request.UserName);
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
                    request.File.CopyTo(fileStream);
                    fileStream.Flush();
                    image.ImageUrl = "\\Images\\" + fileName;
                }


            }
            catch (Exception ex)
            {

            }

            //image.Description = request.Images.Base64;
            await _service.CreateImage(image);
            //save image motel
            var imageMotel = new ImageMotel();
            imageMotel.ImageId = image.Id;
            imageMotel.MotelId = motel.Id;
            await _service.ImageMotel(imageMotel);

            var actual = new MotelResponse();
            actual.Id = result.Id;
            actual.UserName = request.UserName;
            actual.Descriptions = request.Descriptions;
            actual.Price = (int)request.Price;
            actual.Status = (Common.Status)(int)request.Status;
            actual.Title = request.Title;
            return Ok(new ApiResponse<MotelResponse>
            {
                Data = actual,
                StatusCode = 200,
                Messages = result == null ? "Create motel fail" : null
            });
        }

        [HttpPost("update")]
        public async Task<ActionResult<ApiResponse<MotelResponse>>> UpdateMotel([FromForm] MotelModelRequest request)
        {
            var result = await _service.UpdateMotel(request);

            return Ok(new ApiResponse<Motel>
            {
                Data = result,
                StatusCode = 200,
                Messages = result == null ? "Update motel fail" : null
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<MotelResponse>>>> GetMotels(string? userName)
        {
            var motels = await _service.GetMotels(userName);
            return Ok(new ApiResponse<List<MotelResponse>>
            {
                Data = motels,
                StatusCode = 200,
            });
        }

        [HttpPost("approve")]
        public async Task<ActionResult<ApiResponse<bool>>> ApproveMotel(Guid id)
        {
            var result = await _service.Approve(id);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpPost("hired")]
        public async Task<ActionResult<ApiResponse<bool>>> HireMotel(Guid id)
        {
            var result = await _service.Hired(id);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpPost("reject")]
        public async Task<ActionResult<ApiResponse<bool>>> RejectMotel([FromBody] MotelReject motelReject)
        {
            var result = await _service.Reject(motelReject);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteMotel(Guid id)
        {
            var result = await _service.Delete(id);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotelResponse>> GetById(Guid id)
        {
            var result = await _service.GetMotelById(id);
            return Ok(new ApiResponse<MotelResponse>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpPost("comment")]
        public async Task<ActionResult<Comment>> SendComment([FromBody] CommentRequest request)
        {
            var result = await _service.SendComment(request);
            return Ok(new ApiResponse<Comment>
            {
                Data = result,
                StatusCode = 200
            });
        }
        [HttpGet("comment")]
        public async Task<ActionResult<List<Comment>>> GetComment(Guid id)
        {
            var result = await _service.GetCommentByMotelId(id);
            return Ok(new ApiResponse<List<Comment>>
            {
                Data = result,
                StatusCode = 200
            });
        }
        [HttpDelete("comment")]
        public async Task<ActionResult<bool>> DeleteComment(Guid id)
        {
            var result = await _service.DeleteComment(id);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200
            });
        }
    }
}
