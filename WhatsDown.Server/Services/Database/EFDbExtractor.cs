using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Linq.Expressions;
using WhatsDown.Server.Database.Context;
using WhatsDown.Server.Database.Models;
using WhatsDown.Server.Interfaces.Services.Database;

namespace WhatsDown.Server.Services.Database;

public class EFDbExtractor : IDatabaseExtractor
{
    private readonly WhatsDownDbContext context;

    public EFDbExtractor(WhatsDownDbContext context)
    {
        this.context = context;
    }

    public bool TableExists(string tableName)
    {
        return context.Model.GetEntityTypes().Where(e => e.GetTableName() == tableName).Count() > 0;
    }

    public IEnumerator<DataRow> GetRows(string from, Expression<Func<object, bool>> expression)
    {
        throw new NotImplementedException();
    }
}