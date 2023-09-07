using ArHack23.Models;
using ArHack23.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArHack23.Controllers;

[ApiController]
[Route("Players")]
public class PlayerController : ControllerBase
{
    public PlayerController()
    {
    }

    [HttpGet]
    public ActionResult<List<Player>> GetAll() => GameService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Player> Get(int id)
    {
        var player = GameService.Get(id);

        if (player == null)
            return NotFound();

        return player;
    }

    [HttpPost("/register")]
    public ActionResult<Player> Create(Player player)
    {
        return GameService.Add(player);
    }

    [HttpPost("/update")]
    public ActionResult<List<Player>> Update(Player player)
    {
        var id = player.Id;

        var existingPizza = GameService.Get(id);

        if (existingPizza is null)
            return NotFound();

        GameService.Update(player);

        return GameService.GetAll();
    }

    [HttpGet("/flags")]
    public ActionResult<Flag> GetFlag()
    {
        return GameService.flag;
    }

    [HttpGet("/gamestate/{id}")]
    public ActionResult<GameState> GetGameState(int id)
    {
        return GameService.GetState(id);
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = GameService.Get(id);

        if (pizza is null)
            return NotFound();

        GameService.Delete(id);

        return NoContent();
    }
}