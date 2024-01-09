namespace Notes.DataTransfer.Input.UserDataTransferInput;

public record UserInputLogin
{
    public string? Email { get; set; }

    public string? Password { get; set; }
}
