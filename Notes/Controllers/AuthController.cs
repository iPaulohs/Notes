using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.DataTransfer.Input.UserDataTransferInput;
using Notes.Domain;
using Notes.Identity;

namespace Notes.Controllers;

[Route("[controller]")]
[ApiController]
public partial class AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, NotesDbContext context) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly NotesDbContext _context = context;

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UserInputRegister userInput)
    {
        ArgumentNullException.ThrowIfNull(userInput);

        User user = new()
        {
            Id = $"[{userInput.Name.GetHashCode().ToString().Replace('-', '4')}]" +
            $"@[{Guid.NewGuid().GetHashCode().ToString().Replace('-', '9')}]",
            Name = userInput.Name,
            Email = userInput.Email,
            UserName = userInput.UserName,
            BirthDate = new DateOnly(year: userInput.BirthDate.Year, month: userInput.BirthDate.Month, day: userInput.BirthDate.Day)
        };


        var result = await _userManager.CreateAsync(user, userInput.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.SetLockoutEnabledAsync(user, false);
        return Ok(GenerateToken(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserInputLogin userInput)
    {
        ArgumentNullException.ThrowIfNull(userInput);

        if (!ModelState.IsValid)
        {
            Console.WriteLine(ModelState);
            return BadRequest("Informações inválidas.");
        }

        var user = await _userManager.FindByEmailAsync(userInput.Email);

        if (user == null)
        {
            return BadRequest("Usuario não encontrado.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, userInput.Password, lockoutOnFailure: false, isPersistent: false);

        if (result.Succeeded)
        {
            return Ok(GenerateToken(user));
        }

        return BadRequest("Falha na autenticação. Verifique suas credenciais.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout bem sucedido.");
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpPost("active-user")]
    public async Task<IActionResult> Active([FromBody] UserInputLogin userInput)
    {
        ArgumentNullException.ThrowIfNull(userInput);

        var user = await _userManager.FindByEmailAsync(userInput.Email);

        if (user == null)
        {
            return BadRequest("Usuário não encontado.");
        }

        var password = await _userManager.CheckPasswordAsync(user, userInput.Password);

        if (password != true)
        {
            return BadRequest("Senha incorreta.");
        }

        try
        {
            user.IsActive = true;
            user.InactivationDate = null;
            await _context.SaveChangesAsync();
            return Ok("Conta ativada com sucesso.");
        }
        catch (Exception)
        {
            return BadRequest("Não foi possível ativar a conta. Tente novamente em instantes.");
            throw;
        }
    }

    [Authorize]
    [HttpDelete("inactive-user")]
    public async Task<IActionResult> Inactive([FromBody] UserInputLogin userInput)
    {
        ArgumentNullException.ThrowIfNull(userInput);

        var user = await _userManager.FindByEmailAsync(userInput.Email);

        if (user == null)
        {
            return BadRequest("Usuário não encontado.");
        }

        if(user.IsActive == false)
        {
            return BadRequest("Usuário já está desativado.");
        }

        var password = await _userManager.CheckPasswordAsync(user, userInput.Password);

        if(password != true)
        {
            return BadRequest("Senha incorreta.");
        }

        try
        {
            user.IsActive = false;
            user.InactivationDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok("Conta inativada com sucesso.");
        }
        catch (Exception) 
        {
            return BadRequest("Não foi possível desativar a conta. Tente novamente em instantes.");
            throw; 
        }
    }
}
