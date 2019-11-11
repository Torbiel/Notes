using AutoMapper;
using Notes.Dtos;
using Notes.Models;
using System;

namespace Notes.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Created and Modified set to now and Version to 1
            CreateMap<NoteForAddingDto, Note>().ForMember(n => n.Version, opt => opt.MapFrom(src => 1))
                                               .ForMember(n => n.Created, opt => opt.MapFrom(src => DateTime.Now))
                                               .ForMember(n => n.Modified, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Note, NoteForGettingDto>();

            CreateMap<NoteForUpdatingDto, Note>().ForMember(n => n.Created, opt => opt.MapFrom(src => DateTime.Now))
                                                 .ForMember(n => n.Modified, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Note, NoteForHistoryDto>();
        }
    }
}
