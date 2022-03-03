using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.LibraryGenreDTO;

public class LibraryGenreDTO
{
    [Required]
    public string Name { get; set; }
}