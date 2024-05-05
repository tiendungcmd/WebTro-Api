using AutoMapper;
using MotelApi.Models;
using MotelApi.Response;

namespace MotelApi.Common.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // CreateMap<ListeningModel, ListeningResponse>().ReverseMap();
            CreateMap<Motel, MotelResponse>().ReverseMap();
        }
    }
}
