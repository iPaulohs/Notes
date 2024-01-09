using Microsoft.AspNetCore.Mvc;
using Notes.DataTransfer.Input.CollectionDataTransfer;
using Notes.Domain;
using Notes.Pagination;

namespace Notes.Repository.Collections;

public interface ICollectionsRepository
{
    public Task CreateCollectionAsync(CollectionInputInclude _collectionInput, string authorId);
    public Task<List<Collection>> GetAllCollectionsAsync(string authorId, Parameters<Collection> parameters);
    public Task DeleteCollectionAsync(int collectionId);
    public Task<List<Collection>> GetCollectionByTitleAsync(string searchTerm, string authorId, Parameters<Collection> parameters);
    public Task EditCollectionAsync(int collectionId, CollectionInputUpdate updatedCollection);
}
