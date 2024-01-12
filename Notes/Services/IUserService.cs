namespace Notes.Services;

public interface IUserService
{
    Task<bool> IsUserActiveAsync(string userId);
}
