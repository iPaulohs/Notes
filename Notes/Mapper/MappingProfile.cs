using AutoMapper;
using Notes.DataTransfer.Output.NoteDataTransferOutput;
using Notes.Domain;

namespace Notes.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<Note, NoteOutput>();
    }
}
