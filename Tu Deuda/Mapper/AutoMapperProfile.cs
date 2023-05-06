using AutoMapper;
using Tu_Deuda.Model;

namespace Tu_Deuda.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MClientSupabase, MClient>();
        }
    }
}