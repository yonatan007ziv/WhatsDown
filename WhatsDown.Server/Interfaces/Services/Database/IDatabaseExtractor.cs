using System.Data;
using System.Linq.Expressions;

namespace WhatsDown.Server.Interfaces.Services.Database;

public interface IDatabaseExtractor
{
    bool TableExists(string tableName);
    IEnumerator<DataRow> GetRows(string tableName, Expression<Func<object, bool>> expression);
}