using System.Numerics;

namespace ArHack23.Models;

public class Player
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public Vector3?  Location { get; set; }
    public string Color { get; set; }
}