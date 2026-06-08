using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("api/mpesa")]
public class MpesaController : ControllerBase
{
    private readonly NpgsqlDataSource _db;

    public MpesaController(NpgsqlDataSource db) => _db = db;

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        await using var conn = await _db.OpenConnectionAsync();
        var rows = await conn.QueryAsync(
            "SELECT * FROM mpesa_transactions ORDER BY parsed_date DESC LIMIT 100"
        );
        return Ok(rows);
    }

    [HttpGet("summary/daily")]
    public async Task<IActionResult> DailySummary()
    {
        await using var conn = await _db.OpenConnectionAsync();
        var rows = await conn.QueryAsync(@"
            SELECT 
                category,
                direction,
                COUNT(*) AS count,
                SUM(amount) AS total
            FROM mpesa_transactions
            WHERE parsed_date::date = CURRENT_DATE
            GROUP BY category, direction
            ORDER BY total DESC
        ");
        return Ok(rows);
    }

    [HttpGet("summary/monthly")]
    public async Task<IActionResult> MonthlySummary()
    {
        await using var conn = await _db.OpenConnectionAsync();
        var rows = await conn.QueryAsync(@"
            SELECT 
                category,
                direction,
                COUNT(*) AS count,
                SUM(amount) AS total
            FROM mpesa_transactions
            WHERE month = TO_CHAR(CURRENT_DATE, 'YYYY-MM')
            GROUP BY category, direction
            ORDER BY total DESC
        ");
        return Ok(rows);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> CategoryBreakdown()
    {
        await using var conn = await _db.OpenConnectionAsync();
        var rows = await conn.QueryAsync(@"
            SELECT 
                category,
                COUNT(*) AS count,
                SUM(amount) AS total,
                AVG(amount) AS average
            FROM mpesa_transactions
            GROUP BY category
            ORDER BY total DESC
        ");
        return Ok(rows);
    }
}