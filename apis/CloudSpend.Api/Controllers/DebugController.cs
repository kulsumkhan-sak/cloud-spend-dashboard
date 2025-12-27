using Microsoft.AspNetCore.Mvc;
using CloudSpend.Api.Repos;

[ApiController]
[Route("api/debug")]
public class DebugController : ControllerBase
{
    private readonly UserRepository _repo;

    public DebugController(UserRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("db")]
    public IActionResult TestDb()
    {
        var hash = _repo.GetPasswordHashByEmail("admin@cloudspend.com");
        return Ok(new
        {
            found = hash != null,
            hash = hash
        });
    }
}
