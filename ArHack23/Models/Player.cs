using System.Numerics;

namespace ArHack23.Models;

public enum Color
{
    Red = 1,
    Blue = 2,
}

public class Player
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public Vec3? Location { get; set; }
    public Color Team { get; set; }
    public int Kills { get; set; } = 0;
    //public bool Alive { get; set; } = true;
    //// Not in use for first version. Later we can use this to show if a player has the flag or not
    //public bool HasFlag { get; set; } = false;

    internal void Validate(int nextId)
    {
        if (Location == null)
        {
            throw new InvalidDataException("Location must be set and be of format 'x,x,x'");
        }
        if (!(Team == Color.Red || Team == Color.Blue))
        {
            Team = nextId % 2 == 0 ? Color.Red : Color.Blue;
        }
    }

    public bool IsCloseToLocation(Vec3 location)
    {
        return Vector3.Distance(Location.ToVector3(), location.ToVector3()) < 1;
    }
    //public static bool IsLocationStringValid(Vector3 location)
    //{
    //    //return string.IsNullOrEmpty(location) || location.Split(",").Length != 3;
    //    return location != null;
    //}
}