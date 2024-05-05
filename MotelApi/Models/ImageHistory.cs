using System.ComponentModel.DataAnnotations;

namespace MotelApi.Models
{
    public class ImageHistory
    {
        public Guid ImageId { get; set; }
        public Guid HistoryId { get; set; }
    }
}
