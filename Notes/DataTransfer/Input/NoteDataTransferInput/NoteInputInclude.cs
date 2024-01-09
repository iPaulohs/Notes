namespace Notes.DataTransfer.Input.NoteDataTransferInput;

public record NoteInputInclude
{

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int CollectionId { get; set; }
}
