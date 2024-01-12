using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notes.DataTransfer.Input.CollectionDataTransfer;
using Notes.Domain;
using Notes.Identity;
using Notes.Pagination;

namespace Notes.Repository.Collections;

public class CollectionsRepository(NotesDbContext context, ILogger<CollectionsRepository> logger) : ICollectionsRepository
{
    private readonly NotesDbContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task CreateCollectionAsync(CollectionInputInclude _collectionInput, string authorId)
    {
        ArgumentNullException.ThrowIfNull(_collectionInput);
        ArgumentNullException.ThrowIfNull(authorId);

        var user = await _context.Users.FindAsync(authorId);

        if(user == null)
        {
            throw new Exception("Usuário não encontrado");
        }
        else
        {
            Collection collection = new()
            {
                Title = _collectionInput.Title,
                Description = _collectionInput.Description,
                Author = user.Id,
                CreationDate = DateTime.UtcNow
            };

            try
            {
                _context.Collections.Add(collection);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
    }

    public async Task DeleteCollectionAsync(int collectionId)
    {
        var collection = _context.Collections.Find(collectionId);
        if (collection == null)
        {
            throw new Exception($"Coleção com o id {collectionId} não existe.");
        }
        else
        {
            try
            {
                _context.Remove(collection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
    }

    public async Task EditCollectionAsync(int collectionId, CollectionInputUpdate updatedCollection)
    {
        var collection = _context.Collections.Find(collectionId);
        if (collection == null)
        {
            throw new Exception($"Coleção com o id {collectionId} não existe.");
        }
        else 
        {
            try
            {
                _context.Entry(collection).CurrentValues.SetValues(updatedCollection);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
    }

    public async Task<List<Collection>> GetAllCollectionsAsync(string authorId, Parameters<Collection> parameters)
    {
        var collections = await _context.Collections
            .Where(x => x.Author == authorId)
            .OrderBy(x => x.Title)
            .ToListAsync();

        return PagedList<Collection>.ToPagedList(collections, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<List<Collection>> GetCollectionByTitleAsync(string searchTerm, string authorId, Parameters<Collection> parameters)
    {
        return PagedList<Collection>.ToPagedList(await _context.Collections
           .Where(x => x.Author == authorId && x.Title.Contains(searchTerm.ToLower()))
           .Skip((parameters.PageNumber - 1) * parameters.PageSize)
           .Take(parameters.PageSize)
           .ToListAsync(),
           parameters.PageNumber,
           parameters.PageSize);
    }
}
