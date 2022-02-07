using _4kTiles_Backend.DataObjects.DAO.Song;
using _4kTiles_Backend.DataObjects.DTO.SongDTO;
using _4kTiles_Backend.Entities;
using AutoMapper;

namespace _4kTiles_Backend.Mappers;

public class SongProfile : Profile
{
    public SongProfile()
    {
        CreateMap<SongDTO, CreateSongDAO>();
        CreateMap<CreateSongDAO, Song>();
        CreateMap<EditSongDTO, EditSongDAO>();
        CreateMap<EditSongDAO, Song>();
    }
}