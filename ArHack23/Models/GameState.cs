
namespace ArHack23.Models
{
    public class GameState
    {
        public List<Player> Players { get; set; }
        public GameStatus Status { get; set; }
        public enum GameStatus
        {
            Playing,
            RedWin,
            BlueWin
        }

        //public Vector3 GetRedFlagLocation()
        //{
        //     return Players.FirstOrDefault(p => p.Team == Player.Color.Red && p.HasFlag)?.Location ?? Flags.RedFlagBaseLocation;
        //}

        //public Vector3 GetBlueFlagLocation()
        //{
        //    return Players.FirstOrDefault(p => p.Team == Player.Color.Blue && p.HasFlag)?.Location ?? Flags.BlueFlagBaseLocation;
        //}
    }
}
