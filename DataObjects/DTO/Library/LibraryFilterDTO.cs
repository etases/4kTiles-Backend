using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.LibraryFilterDTO;

public class LibraryFilterDTO
{
    public string name { get; set; }
    public string tag { get; set; }
    public string author { get; set; }
    public string genre { get; set; }
}