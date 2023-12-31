using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TMPro;
using UnityEngine;

    public enum Color
    {
        Red = 1,
        Blue = 2,
    }

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Vec3? Location { get; set; }
    public Color Team { get; set; }
    //public int Kills { get; set; } = 0;
    //public bool Alive { get; set; } = true;
    //// Not in use for first version. Later we can use this to show if a player has the flag or not
    //public bool HasFlag { get; set; } = false;
    internal void Validate()
    {
        if (Location == null)
        {
            throw new InvalidDataException("Location must be set and be of format 'x,x,x'");
        }
        if (!(Team == Color.Red || Team == Color.Blue))
        {
            throw new InvalidDataException("Color value is either 1 or 2");
        }
    }

    public bool IsCloseToLocation(Vec3 location)
    {
        return Vector3.Distance(Location.ToVector3(), location.ToVector3()) < 1;
    }
}
public class Flags
{
    public Vec3 RedFlagBaseLocation { get; set; }
    public Vec3 BlueFlagBaseLocation { get; set; }

}
public class Vec3
{
    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public Vec3(Vector3 v) : this(v.x, v.y, v.z) { }
    public Vec3()
    {

    }

    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

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

}
public class ARLocationReporter : MonoBehaviour
{
    public TextMeshPro text;
    public GameObject ARSession;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject redFlagPrefab;
    public GameObject blueFlagPrefab;
    public GameObject deadPrefab;
    public GameObject winPrefab;
    public GameObject losePrefab;


    public static string name;
    private Player m_player;
    private readonly Dictionary<int, GameObject> m_players = new();
    private GameState m_lastGameState = new();
    private readonly HttpClient m_client = new()
    {
        BaseAddress = new Uri("https://arhack2320230904145536.azurewebsites.net"),
    };
    private readonly string m_id = Guid.NewGuid().ToString();

    // Start is called before the first frame update
    void Start()
    {
        Register();
        InitiateFlag();
        Task.Run(UpdateOffline);
    }

        private void InitiateFlag()
        {
            var response = m_client.GetAsync("/flags").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var flag = JsonConvert.DeserializeObject<Flags>(content);
            var position = flag.BlueFlagBaseLocation.ToVector3();
            Instantiate(blueFlagPrefab, position, Quaternion.identity);
            position = flag.RedFlagBaseLocation.ToVector3();
        Instantiate(redFlagPrefab, position, Quaternion.identity);
    }

    private void Register()
        {
            var player = new Player {  };
            var response = m_client.PostAsync("/players", new StringContent(JsonConvert.SerializeObject(player), Encoding.UTF8, "application/json")).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var players = JsonConvert.DeserializeObject<Player>(content);
            m_player = players;
            m_player.Location = new Vec3();
            m_player.Name = name;
        }

    // Update is called once per frame
    void Update()
    {
        m_player.Location.X = ARSession.transform.position.x;
        m_player.Location.Y = ARSession.transform.position.y;
        m_player.Location.Z = ARSession.transform.position.z;
        if (m_player.Location.X == 5)
        {
            Dead();
        }
        if (m_lastGameState.Status != GameState.GameStatus.Playing)
        {
            Win();
            return;
        }

        var otherPlayerData = m_lastGameState.Players;
        if (otherPlayerData == null)
        {
            return;
        }
        if (!otherPlayerData.Any(p => p.Id == m_player.Id))
        {
            Dead();
        }

        foreach (var player in otherPlayerData)
        {
            if (player.Id == m_player.Id)
            {
                continue;
            }

            if (m_players.ContainsKey(player.Id))
            {
                m_players[player.Id].transform.position = player.Location.ToVector3();
            }
            else
            {
                var prefab = player.Team == Color.Blue ? bluePrefab : redPrefab;
                var newPlayer = Instantiate(prefab, player.Location.ToVector3(), Quaternion.identity);
                m_players.Add(player.Id, newPlayer);
            }
        }
    }

    public void Kill(GameObject obj)
    {
        try
        {
            var playerToKill = m_players.First(x => x.Value == obj);
            var playterData = m_lastGameState.Players.First(p => p.Id == playerToKill.Key);
            if (playterData.Team == m_player.Team)
            {
                return;
            }
            m_client.DeleteAsync($"/players/{playerToKill.Key}");
            Destroy(playerToKill.Value);
            m_players.Remove(playerToKill.Key);
        }
        catch (Exception e)
        {
            return;
        }
    }

    private void Dead()
    {
        Instantiate(deadPrefab);
        enabled = false;
    }

    private async Task UpdateOffline()
    {
        while (m_lastGameState == null || m_lastGameState.Status == GameState.GameStatus.Playing)
        {

            try
            {
                m_player.Name = name;
                string playerJson = JsonConvert.SerializeObject(m_player);
                var content = new StringContent(playerJson, Encoding.UTF8, "application/json");
                var response = await m_client.PutAsync($"/players/{m_player.Id}", content);
                text.text = $"{response.StatusCode}";
                var resContent = response.Content.ReadAsStringAsync().Result;
                m_lastGameState = JsonConvert.DeserializeObject<GameState>(resContent);
            } catch (Exception ex)
            {
                Debug.Log($"UpdateOffline got exception: {ex}");
            }
            await Task.Delay(100);
        }
    }

    private void Lose()
    {
        Instantiate(losePrefab);
    }

    private void Win()
    {
        Instantiate(winPrefab);
    }
}
