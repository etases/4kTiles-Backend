using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.LibraryFilterDTO;

public class LibraryUserIdDTO
{
    [Required]
    public int Id { get; set; }
}