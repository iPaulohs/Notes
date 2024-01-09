using Notes.DataTransfer.Input.NoteDataTransferInput;
using Notes.Domain;
using Notes.Pagination;

namespace Notes.Repository.Notes;

public interface INotesRepository
{
    public Task CreateNoteAsync(NoteInputInclude _noteInput, string authorId);
    public Task<PagedList<Note>> GetAllNotesAsync(string authorId, int collectionId, Parameters<Note> parameters);
    public Task<PagedList<Note>> GetNoteByTitleAsync(string searchTerm, string authorIUd, Parameters<Note> parameters);
    public Task DeleteNoteAsync(int noteId);
    public Task EditNoteAsync(int noteId, NoteInputUpdate updatedNote);
}
