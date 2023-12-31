using ArHack23.Models;
using ArHack23.Services;
using Kusto.Data;
using Kusto.Ingest;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using static ArHack23.Models.GameState;

namespace ArHack23.Controllers;

[ApiController]
[Route("Players")]
public class PlayerController : ControllerBase
{
    public static Player lastUp;

    public PlayerController()
    {
      
    }

    [HttpGet]
    public ActionResult<GameState> GetAll() => GameService.GetState();

    [HttpGet("{id}")]
    public ActionResult<Player> Get(int id)
    {
        var player = GameService.GetPlayer(id);

        if (player == null)
            return NotFound();

        return player;
    }

    [HttpPost]
    public ActionResult<Player> Create(Player player)
    {
        GameService.Add(player);
        return player;
    }

    [HttpPut("{id}")]
    public ActionResult<GameState> Update(int id, Player player)
    {
        lastUp = player;
        if (id != player.Id)
            return BadRequest();

        var existing = GameService.GetPlayer(id);

        if (existing is null)
            return NotFound();

        GameService.UpdatePlayer(player);

        return GameService.GetState();
    }

    [HttpDelete("{id}")]
    public ActionResult<GameState> Delete(int id)
    {
        var pizza = GameService.GetPlayer(id);

        if (pizza is null)
            return NotFound();

        GameService.DeletePlayer(id);

        return GameService.GetState();
    }

    [HttpDelete("")]
    public IActionResult Reset()
    {
        GameService.DeletePlayers();
        GameService.GetState().Status = GameStatus.Playing;
        return NoContent();
    }

    [HttpGet("/flags")]
    public ActionResult<Flags> GetFlags()
    {
        return GameService.Flags;
    }

    [HttpPost("/flags")]
    public IActionResult SetFlags(Flags flags)
    {
        GameService.SetFlags(flags);
        return NoContent();
    }

  
}