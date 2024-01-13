using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Anotations;
using Notes.DataTransfer.Input.CollectionDataTransfer;
using Notes.Domain;
using Notes.Pagination;
using Notes.Repository.Collections;

namespace Notes.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CollectionsController(ICollectionsRepository collectionsRepository) : ControllerBase
{
    private readonly ICollectionsRepository _collectionsRepository = collectionsRepository;

    [IsActiveUser]
    [HttpPost("create-collection/{authorId}")]
    public async Task<IActionResult> CreateCollection([FromBody] CollectionInputInclude model, string authorId)
    {
        try
        {
            await _collectionsRepository.CreateCollectionAsync(model, authorId);
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

    [HttpGet("get-all/{authorId}")]
    public async Task<IActionResult> GetAll(string authorId, [FromRoute] Parameters<Collection> parameters)
    {
        List<Collection> collections = await _collectionsRepository.GetAllCollectionsAsync(authorId, parameters);

        if (collections == null)
        {
            return NotFound("Nenhuma coleção encontrada.");
        }
        else
        {
            return Ok(collections);
        }
    }

    [HttpGet("get-term/{authorId}/{searchTerm}")]
    public async Task<IActionResult> GetAllBySearchTerm(string searchTerm, string authorId, [FromRoute] Parameters<Collection> parameters)
    { 
        List<Collection> collections = await _collectionsRepository.GetCollectionByTitleAsync(searchTerm, authorId, parameters);
        
        if(collections == null)
        {
            return NotFound("Nenhuma collection encontrada.");
        }
        else
        {
            return Ok(collections);
        }
    }

    [HttpPut("edit-collection/{collectionId:int:min(1)}")]
    public async Task<IActionResult> EditCollection(CollectionInputUpdate model, int collectionId)
    {
        try
        {
            await _collectionsRepository.EditCollectionAsync(collectionId, model);
            return Ok("Coleção editada com sucesso.");
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

    [HttpDelete("delete-collection/{collectionId:int:min(1)}")]
    public async Task<IActionResult> DeleteCollection(int collectionId)
    {
        try
        {
            await _collectionsRepository.DeleteCollectionAsync(collectionId);
            return Ok("Coleção excluída com sucesso.");
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
