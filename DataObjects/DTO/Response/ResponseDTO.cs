using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.Response
{
    public record ResponseDTO<T>
    {
        public int StatusCode { get; set; }
        public int ErrorCode { get; set; } = 0;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public bool IsError
        {
            get => ErrorCode != 0;
            set => ErrorCode = value == true ? 1 : 0;
        }
    }

    public record ResponseDTO : ResponseDTO<dynamic> {}
}