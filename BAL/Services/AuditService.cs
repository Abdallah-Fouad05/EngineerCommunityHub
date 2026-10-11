using System.Data.SqlClient;
using BAL;
using DAL;
using Microsoft.Data.SqlClient;

namespace BAL.Services
{
    public static class AuditService
    {
        public static async Task LogAsync(string action, int performedBy, int targetId, string details)
        {
            using var connection = new SqlConnection(Setting.Connection);

            using var command = new SqlCommand(@"
            INSERT INTO AuditLogs (Action, PerformedBy, TargetId, Details)
            VALUES (@Action, @PerformedBy, @TargetId, @Details)", connection);

            command.Parameters.AddWithValue("@Action", action);
            command.Parameters.AddWithValue("@PerformedBy", performedBy);
            command.Parameters.AddWithValue("@TargetId", targetId);
            command.Parameters.AddWithValue("@Details", details);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

    }
}