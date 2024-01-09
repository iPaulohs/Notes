namespace Notes.DataTransfer.Output.Token;

public record TokenOutput
{
    public bool Authenticated { get; init; }
    public DateTime Expiration { get; init; }
    public string? Token { get; init; }
    public string? Message { get; init; }
    public string? authorId { get; init; }
    public string? UserName { get; init; }
}
