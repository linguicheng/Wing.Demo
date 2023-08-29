using _5._2.Entity;
using System.Threading.Tasks;

namespace _5._2.Service
{
    public interface IFreeSqlDemoService
    {
        Task<int> Add(FreeSqlDemo entity);
    }
}
