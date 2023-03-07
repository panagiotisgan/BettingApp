using System.Collections.Generic;

namespace Betting.API.DTOModels
{
    public class ResponseDTO<T>
    {
        public T Data { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public string Message { get; set; }
    }
}
