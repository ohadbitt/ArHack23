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

    [HttpPost]
    public IActionResult Create(Player player)
    {
        GameService.Add(player);
        return CreatedAtAction(nameof(Get), new { id = player.Id }, player);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Player player)
    {
        if (id != player.Id)
            return BadRequest();

        var existingPizza = GameService.Get(id);

        if (existingPizza is null)
            return NotFound();

        GameService.Update(player);

        return NoContent();
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