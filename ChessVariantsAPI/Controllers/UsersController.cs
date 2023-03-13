using DataAccess.MongoDB;
using DataAccess.MongoDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChessVariantsAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{

    private readonly DatabaseService _db;

    public UsersController(DatabaseService databaseService)
    {
        _db = databaseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await _db.Users.GetAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(User user)
    {
        user = new User { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName };
        await _db.Users.CreateAsync(user);
        return CreatedAtAction("Get", new {id = user.Id}, user);
    }
}
