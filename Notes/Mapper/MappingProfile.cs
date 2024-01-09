using AutoMapper;
using Notes.DataTransfer.Output.NoteDataTranferOutput;
using Notes.Domain;

namespace Notes.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Note, NoteOutputFree>();
        }
    }
}
