using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.LibraryFilterDTO;

public class LibraryFilterDTO
{
    public string Name { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
}