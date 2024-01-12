using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.DataTransfer.Input.NoteDataTransferInput;
using Notes.DataTransfer.Output.NoteDataTransferOutput;
using Notes.Domain;
using Notes.Identity;
using Notes.Pagination;

namespace Notes.Repository.Notes;

public class NotesRepository(NotesDbContext context, ILogger<NotesRepository> logger, IMapper mapper) : INotesRepository
{
    private readonly NotesDbContext _context = context;
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task CreateNoteAsync(NoteInputInclude _noteInput, string authorId)
    {
        ArgumentNullException.ThrowIfNull(_noteInput);
        ArgumentNullException.ThrowIfNull(authorId);

        var user = await _context.Users.FindAsync(authorId);

        if (user == null)
        {
            throw new Exception("Usuário não encontrado");
        }
        else
        {
            var collection = _context.Collections.Find(_noteInput.CollectionId);
            if (collection == null)
            {
                throw new Exception("Coleção não encontrado");
            }
            else
            {
                Note note = new()
                {
                    Author = authorId,
                    Title = _noteInput.Title,
                    Description = _noteInput.Description,
                    Collection = _noteInput.CollectionId,
                    CreationDate = DateTime.UtcNow,
                    FinalDate = _noteInput.FinalDate
                };

                try
                {
                    _context.Notes.Add(note);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                }
            }
        }
    }

    public async Task DeleteNoteAsync(int noteId)
    {
        var note = _context.Notes.Find(noteId);
        if (note == null)
        {
            throw new Exception($"Nenhuma nota foi encontrada com o id {noteId}");
        }
        else
        {
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditNoteAsync(int noteId, NoteInputUpdate updatedNote)
    {
        var note = _context.Notes.Find(noteId);
        if (note == null)
        {
            throw new Exception($"Nenhuma nota foi encontrada com o id {noteId}");
        }
        else
        {
            try
            {
                _context.Entry(note).CurrentValues.SetValues(updatedNote);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
    }

    public async Task<PagedList<NoteOutput>> GetAllNotesAsync(string authorId, int collectionId, Parameters<Note> parameters)
    {
        var notes = await _context.Notes
            .Where(x => x.Author == authorId && x.Collection == collectionId)
            .OrderBy(x => x.Title)
            .ToListAsync();

        return PagedList<NoteOutput>.ToPagedList(_mapper.Map<List<NoteOutput>>(notes), parameters.PageNumber, parameters.PageSize);
    }

    public async Task<PagedList<NoteOutput>> GetNoteByTitleAsync(string searchTerm, string authorId, Parameters<Note> parameters)
    {
        var notes = await _context.Notes.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(x => x.Title)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();
        return PagedList<NoteOutput>.ToPagedList(_mapper.Map<List<NoteOutput>>(notes), parameters.PageNumber,parameters.PageSize);
    }
}