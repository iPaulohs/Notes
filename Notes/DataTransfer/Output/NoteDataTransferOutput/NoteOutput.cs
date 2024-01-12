namespace Notes.DataTransfer.Output.NoteDataTransferOutput;

public record NoteOutput
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public int CollectionId { get; set; }

    public DateTime? FinalDate { get; set; }
}
