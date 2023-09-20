namespace WhatsDown.Server.Interfaces.Services.Database;

// Revise as needed
internal interface IDatabaseExtractor
{
	Task<IEnumerable<object>> GetRowsAsync(string query, params object[] parameters);
	Task<object> ExecuteScalarAsync(string query, params object[] parameters);
	Task<int> ExecuteNonQueryAsync(string query, params object[] parameters);
}