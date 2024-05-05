using System.ComponentModel.DataAnnotations;

namespace MotelApi.Models
{
    public class ImageMotel
    {
        public Guid ImageId { get; set; }
        public Guid MotelId { get; set; }
    }
}
