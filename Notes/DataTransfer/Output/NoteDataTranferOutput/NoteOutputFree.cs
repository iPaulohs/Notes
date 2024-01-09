namespace Notes.DataTransfer.Output.NoteDataTranferOutput
{
    public record NoteOutputFree
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Author { get; set; }

        public int Collection { get; set; }
    }
}
