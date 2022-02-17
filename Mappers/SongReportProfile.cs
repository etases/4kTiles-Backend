using _4kTiles_Backend.DataObjects.DAO.Report;
using _4kTiles_Backend.DataObjects.DTO.Report;
using _4kTiles_Backend.Entities;

using AutoMapper;

namespace _4kTiles_Backend.Mappers
{
    public class SongReportProfile : Profile
    {
        public SongReportProfile()
        {
            CreateMap<SongReport, SongReportDAO>();
            CreateMap<SongReportDAO, SongReportDTO>();
            CreateMap<CreateReportDTO, CreateReportDAO>();
            CreateMap<CreateReportDAO, SongReport>();
        }
    }
}
