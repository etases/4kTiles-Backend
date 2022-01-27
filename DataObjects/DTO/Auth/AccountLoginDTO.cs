using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.Auth
{
    public class AccountLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}