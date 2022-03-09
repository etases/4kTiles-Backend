using _4kTiles_Backend.DataObjects.DAO.Song;
using _4kTiles_Backend.DataObjects.DTO.Song;
using _4kTiles_Backend.Entities;
using AutoMapper;

namespace _4kTiles_Backend.Mappers;

public class SongProfile : Profile
{
    public SongProfile()
    {
        CreateMap<CreateSongDTO, CreateSongDAO>();
        CreateMap<CreateSongDAO, Song>();
        CreateMap<EditSongDTO, EditSongDAO>();
        CreateMap<EditSongDAO, Song>();
        CreateMap<AccountSong, AccountScoreDTO>();
        CreateMap<Song, SongDAO>()
            .ForMember(dao => dao.CreatorName, o => o.MapFrom(s => s.Creator.UserName))
            .ForMember(dao => dao.Genres, o => o.MapFrom(s => s.SongGenres.Select(sg => sg.Genre.GenreName).Distinct().ToList()));
        CreateMap<SongDAO, SongInfoDTO>();
        CreateMap<SongDAO, SongDTO>();
    }
}