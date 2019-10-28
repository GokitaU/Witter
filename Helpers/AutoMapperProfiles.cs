using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Data;
using Witter.Dtos;
using Witter.Models;

namespace Witter.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        private readonly IMatchRepository matchRepository;

        public AutoMapperProfiles(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForReturnDto>();
            CreateMap<MatchForCreateDto, Match>();
            CreateMap<User, UserForAdminDto>();
            CreateMap<LeagueForCreateDto, League>();
            CreateMap<UserForBanDto, User>();
            CreateMap<League, LeagueForListDto>();
            CreateMap<Bet, BetForClientDto>()
                .ForMember(dest => dest.Match, opt =>
                    opt.MapFrom(src => matchRepository.GetMatchSync(src.MatchId)));
        }
    }
}
