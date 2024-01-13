using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Anotations;
using Notes.DataTransfer.Input.NoteDataTransferInput;
using Notes.Domain;
using Notes.Pagination;
using Notes.Repository.Notes;
using System.Text.Json;

namespace Notes.Controllers;

[ApiController]
[Authorize]
[Route("[Controller]")]
public class NotesController(INotesRepository notesRepository) : ControllerBase
{
    private readonly INotesRepository _notesRepository = notesRepository;

    [IsActiveUser]
    [HttpPost("create-note/{authorId}")]
    public async Task<IActionResult> CreateNote([FromBody] NoteInputInclude model, string authorId)
    {
        try
        {
            await _notesRepository.CreateNoteAsync(model, authorId);
            return Ok(model);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = "Erro de validação.", message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor.", message = ex.Message });
        }
    }

    [HttpGet("get-all/{authorId}/{collectionId:int:min(1}}")]
    public async Task<IActionResult> GetAll(string authorId, int collectionId, [FromRoute] Parameters<Note> parameters)
    {
        var notes = await _notesRepository.GetAllNotesAsync(authorId, collectionId, parameters);

        if (notes == null)
        {
            return NotFound("Nenhuma nota encontrada.");
        }
        else
        {
            var metadata = new
            {
                notes.TotalCount,
                notes.PageSize,
                notes.CurrentPage,
                notes.TotalPages,
                notes.HasNext,
                notes.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(notes);
        }
    }

    [HttpGet("get-term/{authorId}/{searchTerm}")]
    public async Task<IActionResult> GetAllBySearchTerm(string searchTerm, string authorId, [FromRoute] Parameters<Note> parameters)
    {
        var notes = await _notesRepository.GetNoteByTitleAsync(searchTerm, authorId, parameters);

        if (notes == null)
        {
            return NotFound("Nenhuma collection encontrada.");
        }
        else
        {
            var metadata = new
            {
                notes.TotalCount,
                notes.PageSize,
                notes.CurrentPage,
                notes.TotalPages,
                notes.HasNext,
                notes.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            return Ok(notes);
        }
    }

    [HttpPut("edit-note/{noteId:int:min(1}")]
    public async Task<IActionResult> EditNote(int noteId, [FromBody] NoteInputUpdate model)
    {
        try
        {
            await _notesRepository.EditNoteAsync(noteId, model);
            return Ok("Nota editada com sucesso.");
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = "Erro de validação.", message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor.", message = ex.Message });
        }
    }

    [HttpDelete("delete-note/{noteId:int:min(1}")]
    public async Task<IActionResult> DeleteNote(int noteId)
    {
        try
        {
            await _notesRepository.DeleteNoteAsync(noteId);
            return Ok("Nota excluída com sucesso.");
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = "Erro de validação.", message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor.", message = ex.Message });
        }
    }
}
