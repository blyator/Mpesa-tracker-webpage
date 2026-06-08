using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace MpesaDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly NpgsqlDataSource _db;

        public DashboardController(NpgsqlDataSource db) => _db = db;

        public async Task<IActionResult> Index()
        {
            await using var conn = await _db.OpenConnectionAsync();

            var transactions = await conn.QueryAsync(
                "SELECT * FROM mpesa_transactions ORDER BY parsed_date DESC LIMIT 10"
            );

            var categories = await conn.QueryAsync(@"
                SELECT category, COUNT(*) AS count, SUM(amount) AS total
                FROM mpesa_transactions
                GROUP BY category
                ORDER BY total DESC
            ");

            ViewBag.Transactions = transactions;
            ViewBag.Categories = categories;

            return View();
        }
    }
}