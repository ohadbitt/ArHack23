using ArHack23.Models;
using System.Numerics;
using System.Reflection;
using static ArHack23.Models.GameState;

namespace ArHack23.Services;

public static class GameService
{
    static List<Player> Players { get; }
    static GameState State;
    static int nextId = 3;
    public static Flags Flags { get; set; }

    static GameService()
    {
        Players = new List<Player>
        {
            new Player { Id = 1, Name = "No one", Location = new Vec3(),Team= Color.Blue},
            new Player { Id = 2, Name = "Ghost", Location = new Vec3(),Team= Color.Red}
        };
        State = new GameState
        {
            Players = Players,
            Status = GameStatus.Playing
        };
        Flags = new Flags()
        {
            RedFlagBaseLocation = new Vec3(10, 0, 0),
            BlueFlagBaseLocation = new Vec3(0, 10, 0),
        };
    }

    public static GameState GetState() => State;

    public static Player? GetPlayer(int id) => Players.FirstOrDefault(p => p.Id == id);

    public static void Add(Player player)
    {
        player.Location = player.Location ?? new Vec3();
        player.Validate(nextId);
        player.Id = nextId++;
        Players.Add(player);
    }

    // Kill in v1
    public static void DeletePlayer(int id)
    {
        var player = GetPlayer(id);
        if (player is null)
            return;

        Players.Remove(player);
    }
    public static void DeletePlayers()
    {
        Players.Clear();
    }

    public static void UpdatePlayer(Player player)
    {
        if (State.Status != GameStatus.Playing)
        {
            return;
        }

        var index = Players.FindIndex(p => p.Id == player.Id);
      
        switch (player.Team)
        {
            case Color.Blue:
                //if (player.HasFlag)
                //{
                //    if (player.IsCloseToLocation(Flags.BlueFlagBaseLocation))
                //    {
                //        State.Status = GameStatus.BlueWin;
                //    }
                //}
                //else
                //{
                if (player.IsCloseToLocation(Flags.RedFlagBaseLocation))
                {
                    //player.HasFlag = true;
                    State.Status = GameStatus.RedWin;  // Delete in v1
                }
                //}
                break;
            case Color.Red:
                //if (player.HasFlag)
                //{
                //    if (player.IsCloseToLocation(Flags.RedFlagBaseLocation))
                //    {
                //        State.Status = GameStatus.RedWin;
                //    }
                //}
                //else
                //{
                if (player.IsCloseToLocation(Flags.BlueFlagBaseLocation))
                {
                    // player.HasFlag = true;
                    State.Status = GameStatus.RedWin;  // Delete in v2
                }
                //}
                break;
        }
        Players[index] = player;
    }

    public static void UpdateState(GameState state)
    {
        State = state;
    }

    public static void SetFlags(Flags flags)
    {
        Flags = flags;
    }
}