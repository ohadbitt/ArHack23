using ArHack23.Models;
using System.Numerics;

namespace ArHack23.Services;

public enum GameState
{
    Play,
    Win,
    Lose
}


public static class GameService
{
    static List<Player> Players { get; }
    static int nextId = 0;
    static string nextColor = "Blue";
    public static Flag flag = new Flag { Location = new System.Numerics.Vector3(10,0,0)};
    static Player? winner = null;
    static GameService()
    {
        Players = new List<Player>();
    }

    public static List<Player> GetAll() => Players;

    public static Player? Get(int id) => Players.FirstOrDefault(p => p.Id == id);

    public static Player Add(Player pizza)
    {
        pizza.Id = nextId++;
        pizza.Location = new System.Numerics.Vector3();
        pizza.Color = nextColor;
        Players.Add(pizza);
        return pizza;
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
        if (Vector3.Distance(pizza.Location.Value, flag.Location) < 1)
        {
            winner = pizza;
        }
    }

    public static GameState GetState(int id)
    {
        if (winner is null)
        {
            return GameState.Play;
        }
        if (winner.Id == id)
        {
            return GameState.Win;
        }
        return GameState.Lose;
    }
}