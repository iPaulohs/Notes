﻿namespace Notes.DataTransfer.Input.NoteDataTransferInput;

public record NoteInputUpdate
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? FinalDate { get; set; } = null;
}
