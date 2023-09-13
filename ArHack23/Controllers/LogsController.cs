using ArHack23.Models;
using ArHack23.Services;
using Kusto.Data;
using Kusto.Ingest;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using static ArHack23.Models.GameState;

namespace ArHack23.Controllers;

[ApiController]
[Route("Logs")]
public class LogsController : ControllerBase
{
    private IKustoIngestClient cli;
    public static Player lastUp;

    public LogsController()
    {
        var csb = new KustoConnectionStringBuilder("https://ohadev.westeurope.dev.kusto.windows.net/").WithAadApplicationKeyAuthentication("d5e0a24c-3a09-40ce-a1d6-dc5ab58dae66", "AfG8Q~6Yj4Wo4ZhIS8GdKThGRwc3THRFjM.Lva1R", "microsoft.com");
        var ingest = new KustoConnectionStringBuilder("https://ingest-ohadev.westeurope.dev.kusto.windows.net/").WithAadApplicationKeyAuthentication("d5e0a24c-3a09-40ce-a1d6-dc5ab58dae66", "AfG8Q~6Yj4Wo4ZhIS8GdKThGRwc3THRFjM.Lva1R", "microsoft.com");

        cli = KustoIngestFactory.CreateManagedStreamingIngestClient(csb, ingest);
    }

    private void log(string rec)
    {
        using (var mem = new MemoryStream())
        {
            mem.Write(Encoding.UTF8.GetBytes(rec));
            mem.Seek(0, SeekOrigin.Begin);
            var res = cli.IngestFromStreamAsync(mem, new KustoIngestionProperties("ohtst", "Hack")).Result;
            Console.WriteLine(res);
        }
    }

    [HttpPost()]
    public IActionResult AddLog([FromBody] string log)
    {
        this.log(log);
        return NoContent();
    }

    [HttpGet()]
    public ActionResult<Player> GetLogs()
    {
        return lastUp;
    }

}