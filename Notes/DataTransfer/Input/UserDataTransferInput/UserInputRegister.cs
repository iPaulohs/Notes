﻿namespace Notes.DataTransfer.Input.UserDataTransferInput;

public record UserInputRegister
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public DateOnly BirthDate { get; set; }
}
