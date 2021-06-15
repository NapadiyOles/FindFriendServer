using AutoMapper;
using FindFriend.Business.Models;
using FindFriend.Data.Entities;

namespace FindFriend.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Add, AddDTO>().ForMember(d => d.AuthorName,
                opt => opt.MapFrom(s => s.Author.Name));

            CreateMap<AddDTO, Add>();
        }
    }
}