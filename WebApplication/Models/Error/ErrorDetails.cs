using Newtonsoft.Json;

namespace WebApplication.Models.Error
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
 
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}