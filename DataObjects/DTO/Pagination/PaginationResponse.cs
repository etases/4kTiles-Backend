using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.Pagination
{
    public record PaginationResponse<T>
    {
        public int TotalRecords { get; set; } = 0;
        public T? Payload { get; set; }
    }
}

