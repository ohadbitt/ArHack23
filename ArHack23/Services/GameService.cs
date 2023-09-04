using ArHack23.Models;

namespace ArHack23.Services;

public static class GameService
{
    static List<Player> Players { get; }
    static int nextId = 3;
    static GameService()
    {
        Players = new List<Player>
        {
            new Player { Id = 1, Name = "No one", Location = "0,0,0"},
            new Player { Id = 2, Name = "Ghost", Location = "0,0,0" }
        };
    }

    public static List<Player> GetAll() => Players;

    public static Player? Get(int id) => Players.FirstOrDefault(p => p.Id == id);

    public static void Add(Player pizza)
    {
        pizza.Id = nextId++;
        Players.Add(pizza);
    }

    public static void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null)
            return;

        Players.Remove(pizza);
    }

    public static void Update(Player pizza)
    {
        var index = Players.FindIndex(p => p.Id == pizza.Id);
        if (index == -1)
            return;

        Players[index] = pizza;
    }
}