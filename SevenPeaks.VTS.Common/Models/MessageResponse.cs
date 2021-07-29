using System.Text.Json.Serialization;

namespace SevenPeaks.VTS.Common.Models
{
    public class BaseMessageResponse
    {
        [JsonIgnore]
        public bool IsSuccessResponse { get; set; }
        public int ResponseCode { get; set; }
        public string Message { get; set; }
    }

    public class MessageResponse<T> : BaseMessageResponse
    {
        public MessageResponse(string msg = null)
        {
            base.Message = msg;
        }
        public T Result { get; set; }
    }
}
