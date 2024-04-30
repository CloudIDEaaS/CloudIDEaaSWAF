using System.Linq;
using System.Linq.Expressions;

namespace Utils.Queryable
{
    public interface IQueryProviderUtility : IQueryProvider
    {
        object Execute(Expression expression);
        string GetQueryText(Expression expression);
    }
}