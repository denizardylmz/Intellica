using AutoMapper;
using Binance.Net.Interfaces;
using NoteAPI.Repo.SqlDatabase.DTO;
using DC = NoteAPI.API.DataContracts;
using SM = NoteAPI.Services.Models;
using SqlM = NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.IoC.Configuration.AutoMapper.Profiles
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<SqlM.User, SM.User>().ReverseMap();
            CreateMap<SqlM.Note, SM.Note>().ReverseMap();

            CreateMap<IBinanceKline, BinanceKline>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Symbol, opt => opt.Ignore());
        }
    }
}
