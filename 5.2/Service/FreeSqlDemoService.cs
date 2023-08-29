using _5._2.Entity;
using System.Threading.Tasks;

namespace _5._2.Service
{
    public class FreeSqlDemoService : IFreeSqlDemoService
    {
        private readonly IFreeSql _fsql;

        public FreeSqlDemoService(IFreeSql fsql)
        {
            _fsql = fsql;
        }
        public Task<int> Add(FreeSqlDemo entity)
        {
            return _fsql.Insert(entity).ExecuteAffrowsAsync();
        }
    }
}
