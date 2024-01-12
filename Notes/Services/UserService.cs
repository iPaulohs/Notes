using Microsoft.AspNetCore.Identity;
using Notes.Domain;

namespace Notes.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<bool> IsUserActiveAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.IsActive ?? false;
    }
}
